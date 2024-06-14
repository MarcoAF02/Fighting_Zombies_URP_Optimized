using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家和门的交互类
/// </summary>
public class PlayerDoorInteractive : MonoBehaviour
{
	#region 基本组件和变量

	private FPS_Play_Action fpsPlayAction;

	[Header("玩家提示信息控制器")]
	[SerializeField] private TipMessageController tipMessageController;
	[Header("玩家控制器")]
	[SerializeField] private PlayerController playerController;
	[Header("玩家物品栏")]
	[SerializeField] private PlayerInventory playerInventory;
	[Header("玩家能和门交互的距离")]
	[SerializeField] private float interactiveDistance;
	[Header("玩家和门交互的 CD 时间")]
	[SerializeField] private float interactiveCDTime;
	[Header("玩家与门交互后，下方提示信息的显示时长")]
	[SerializeField] private float interTipMessageDisplayTime;
	[Header("玩家与门交互后，下方提示信息的背景")]
	[SerializeField] private Color interTipMessageBGColor;
	[Header("玩家对门的检测：检测自己在门的前方还是后方")]
	[Header("检测中心点")]
	[SerializeField] private Transform doorCheckPoint;
	[Header("检测范围")]
	[SerializeField] private float doorCheckRadius;
	[Header("检测的 Layer Index")]
	[SerializeField] private int checkLayerIndex;

	private Dictionary<string, float> distanceDic = new Dictionary<string, float>();
	[HideInInspector] public float interactiveTotalTime;
	private float checkPlayerPos;

	#endregion

	#region 基本生命周期函数

	private void Awake()
	{
		fpsPlayAction = new FPS_Play_Action();
		fpsPlayAction.Enable();

		interactiveTotalTime = interactiveCDTime;
	}

	private void OnDestroy()
	{
		fpsPlayAction?.Disable();
	}

	#endregion

	#region 玩家对门的操作

	/// <summary>
	/// 玩家操作门
	/// </summary>
	public void PlayerOperateDoor()
	{
		interactiveTotalTime = interactiveTotalTime + Time.deltaTime;
		if (interactiveTotalTime > 32768f) interactiveTotalTime = 32768f;

		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Interactive.WasPressedThisFrame() &&
			interactiveTotalTime > interactiveCDTime)
		{
			if (playerController.playerCurrentState != playerController.playerControlState) return;
			PlayerInteractiveWithDoor();
		}
	}

	// 和门交互核心功能
	private void PlayerInteractiveWithDoor()
	{
		// 注意：这里用忽略 Layer 而不是选择 Layer 实现，是为了不要浪费 Layer 的数量
		int ignoreLayerIndex = (1 << playerController.layerAndTagCollection_Player.playerLayerIndex) | (1 << playerController.layerAndTagCollection_Player.airWallLayerIndex) | (1 << playerController.layerAndTagCollection_Player.checkDoorDirLayerIndex) | (1 << playerController.layerAndTagCollection_Player.enemyLayerIndex);
		ignoreLayerIndex = ~(ignoreLayerIndex);

		RaycastHit hitObj;

		if (Physics.Raycast(playerController.playerCameraController.playerCamera.transform.position, playerController.playerCameraController.playerCamera.transform.forward, out hitObj, interactiveDistance, ignoreLayerIndex))
		{
			if (hitObj.collider.CompareTag(playerController.layerAndTagCollection_Player.doorTag))
			{
				interactiveTotalTime = 0f;
				checkPlayerPos = 0f;

				hitObj.collider.GetComponentInParent<MainDoorController>().SetCheckDirCollider(true);
				checkPlayerPos = CheckDoorDir();
				hitObj.collider.GetComponentInParent<MainDoorController>().SetCheckDirCollider(false);

				// Debug.Log(checkPlayerPos);

				// 如果门需要钥匙，则检查玩家背包中是否有钥匙
				if (hitObj.collider.GetComponentInParent<MainDoorController>().keyID != string.Empty &&
					hitObj.collider.GetComponentInParent<MainDoorController>().isOnceOpen)
				{
					if (playerInventory.CheckPlayerHaveKey(hitObj.collider.GetComponentInParent<MainDoorController>().keyID))
					{
						hitObj.collider.GetComponentInParent<MainDoorController>().locked = false;
					}
					else // 如果门上锁了，给玩家发送提示信息
					{
						tipMessageController.ShowInteractiveMessage(hitObj.collider.GetComponentInParent<MainDoorController>().tipLockedMessage, interTipMessageDisplayTime, interTipMessageBGColor);

						if (hitObj.collider.GetComponentInParent<MainDoorController>().canNoticePlayer)
						{
							if (GameProgressManager.Instance != null)
							{
								GameProgressManager.Instance.NoticePlayerGameTarget(hitObj.collider.GetComponentInParent<MainDoorController>().gameTargetTipState);

								// 教程信息提示玩家检查游戏目标（Tab 检查物品栏）
								playerController.tutorialTrigger.TutorialPlayerCheckGameTarget();
							}
						}
					}

					if (!hitObj.collider.GetComponentInParent<MainDoorController>().locked)
					{
						playerInventory.RemoveKey(hitObj.collider.GetComponentInParent<MainDoorController>().keyID); // 玩家用钥匙开门后，移除这把钥匙
					}
				}
		
				hitObj.collider.GetComponentInParent<MainDoorController>().InteractiveWithDoor(checkPlayerPos);

				// 如果该门需要钥匙，在和门交互一次后，门的第一次开门状态设置为 false
				if (hitObj.collider.GetComponentInParent<MainDoorController>().keyID != string.Empty)
				{
					if (hitObj.collider.GetComponentInParent<MainDoorController>().locked == false) // 执行到这里表示上面的钥匙成功把门打开了
					{
						hitObj.collider.GetComponentInParent<MainDoorController>().isOnceOpen = false;
					}
				}
			}
		}
	}

	/// <summary>
	/// 检测玩家在门的前方还是后方
	/// </summary>
	private float CheckDoorDir()
	{
		// 注意：门的方位检查点一定要打上 Door_In 或 Door_Out Tag，Layer 一定要设置为 CheckDoorDir（编号为 8）

		distanceDic.Clear();

		Collider[] doorCheckColliderArray = Physics.OverlapSphere(doorCheckPoint.position, doorCheckRadius, 1 << playerController.layerAndTagCollection_Player.checkDoorDirLayerIndex);

		if (doorCheckColliderArray.Length != 2)
		{
			Debug.LogWarning("玩家没有正确检测到门方位点（或是近距离内有别的门）。请调整玩家和门的参数");
			return -2;
		}

		// 将检查点的 Tag 和 与玩家的距离 储存在字典数据结构中
		for (int i = 0; i < doorCheckColliderArray.Length; i ++)
		{
			if (!distanceDic.ContainsKey(doorCheckColliderArray[i].tag))
			{
				distanceDic.Add(doorCheckColliderArray[i].tag, Vector3.Distance(doorCheckColliderArray[i].transform.position, transform.position));
			}
			else
			{
				Debug.LogWarning("门检查点的 Tag 未设置正确，请检查游戏设置");
				return -2;
			}
		}

		if (distanceDic["Door_In"] < distanceDic["Door_Out"])
		{
			return 2;
		}
		else
		{
			return -2;
		}
	}

	#endregion

	#region DEBUG 用函数

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(doorCheckPoint.position, doorCheckRadius);
	}

	#endregion
}
