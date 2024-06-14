using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 教程提示信息触发器（控制玩家教程信息的出现时机）
/// </summary>
public class TutorialTipTrigger : MonoBehaviour
{
	#region 基本组件和变量

	[Header("教程信息是否要开启（该变量来源于玩家设置）")]
	public bool openTutorial;

	[Header("教完移动后过多长时间教玩家奔跑和下蹲")]
	[SerializeField] private float tutorialRunDelayTime;
	[Header("一般教程的显示时间")]
	[SerializeField] private float tutorialDisplayTime = 6;
	[Header("提示玩家检查物品栏的延迟时间")]
	[SerializeField] private float tutorialCheckGameTargetDelayTime;
	[Header("提示玩家使用医疗包治疗的延迟时间")]
	[SerializeField] private float tutorialUseMedicineDelayTime;
	[Header("教程视野长度")]
	[SerializeField] private float tutorialRayDistance;

	[Header("玩家教学用直线视野")]
	[Header("玩家主控制器")]
	[SerializeField] private PlayerController playerController;

	// 教程信息
	private bool teachMove; // 是否已经教过玩家怎么移动
	private bool teachUseEquipItem; // 是否教过玩家物品栏的使用
	private bool teachUseGun; // 是否教过玩家使用枪械
	private bool teachUseMedicine; // 是否教过玩家治疗
	private bool teachCheckGameTarget; // 是否教过玩家检查游戏目标
	private bool firstViewEnemy; // 是否是第一次看见敌人

	// 协程
	private Coroutine tutorialPlayerMove_IECor;
	private Coroutine tutorialPlayerCheckGameTarget_IECor;
	private Coroutine tutorialPlayerUseMedicine_IE_TECor;

	#endregion

	#region 教程信息触发功能

	/// <summary>
	/// 教玩家怎么移动
	/// </summary>
	public void TutorialPlayerMove()
	{
		if (!openTutorial) return;
		if (teachMove) return;

		tutorialPlayerMove_IECor = StartCoroutine(TutorialPlayerMove_IE());
		teachMove = true;
	}

	// 教玩家怎么移动
	private IEnumerator TutorialPlayerMove_IE()
	{
		playerController.tipMessageController.ShowTutorialMessage("FirstMove", tutorialDisplayTime);
		yield return new WaitForSeconds(tutorialRunDelayTime);
		TutorialPlayerRunAndCrouch();
	}

	// 教玩家奔跑和下蹲
	private void TutorialPlayerRunAndCrouch()
	{
		if (!openTutorial) return;
		playerController.tipMessageController.ShowTutorialMessage("FirstRunAndCrouch", tutorialDisplayTime);
	}

	/// <summary>
	/// 教玩家按下 Tab 检查游戏目标（检查物品栏）
	/// </summary>
	public void TutorialPlayerCheckGameTarget()
	{
		if (!openTutorial) return;
		if (teachCheckGameTarget) return;
		tutorialPlayerCheckGameTarget_IECor = StartCoroutine(TutorialPlayerCheckGameTarget_IE());
	}

	// 教玩家按下 Tab 检查游戏目标（检查物品栏）
	private IEnumerator TutorialPlayerCheckGameTarget_IE()
	{
		yield return new WaitForSeconds(tutorialCheckGameTargetDelayTime);

		playerController.tipMessageController.ShowTutorialMessage("FirstUseInventory", tutorialDisplayTime);
		teachCheckGameTarget = true;
	}

	/// <summary>
	/// 教玩家按 4 进行治疗
	/// </summary>
	public void TutorialPlayerUseMedicine()
	{
		if (!openTutorial) return;
		if (teachUseMedicine) return;
		tutorialPlayerUseMedicine_IE_TECor = StartCoroutine(TutorialPlayerUseMedicine_IE());
	}

	// 如果玩家受伤了，提示玩家用医疗物品治疗
	private IEnumerator TutorialPlayerUseMedicine_IE()
	{
		yield return new WaitForSeconds(tutorialUseMedicineDelayTime);
		playerController.tipMessageController.ShowTutorialMessage("FirstUseMedicine", tutorialUseMedicineDelayTime);
		teachUseMedicine = true;
	}

	// 如果玩家看见了第一个敌人，触发物品栏操作教程。同时如果切到了手枪，触发射击操作教程
	private void TutorialPlayerUseEquipItem()
	{
		if (!openTutorial) return;
		playerController.tipMessageController.ShowTutorialMessage("FirstUseEquipItem", tutorialDisplayTime);
		teachUseEquipItem = true;
	}

	// 如果玩家看见敌人了并且切换至任意枪械
	public void TutorialPlayerFirstUseGun()
	{
		if (!openTutorial) return;
		if (!teachUseEquipItem) return;
		if (teachUseGun) return;

		playerController.tipMessageController.ShowTutorialMessage("FirstUseGun", tutorialDisplayTime);
		teachUseGun = true;
	}

	#endregion

	#region 教程信息视野功能

	/// <summary>
	///	教程信息触发器（看见敌人）
	/// </summary>
	public void PlayerFirstViewEnemy_Tutorial()
	{
		if (!openTutorial) return;
		if (firstViewEnemy) return;

		RaycastHit hitObj;

		if (Physics.Raycast(playerController.playerCameraController.playerCamera.transform.position, playerController.playerCameraController.playerCamera.transform.forward, out hitObj, tutorialRayDistance))
		{
			if (hitObj.collider.CompareTag(playerController.layerAndTagCollection_Player.enemyTag))
			{
				firstViewEnemy = true;
				TutorialPlayerUseEquipItem();
			}
		}
	}

	#endregion
}
