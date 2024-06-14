using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 记录玩家手臂的状态
/// </summary>
public enum ArmState
{
	None, // 什么也没发生
	TopWall // 顶住墙了
}

public class FPSBodyController : MonoBehaviour
{
	#region 基本组件和变量

	[Header("第一人角色控制器")]
	[SerializeField] private PlayerController playerController;

	[Header("第一人称手臂模型根物体")]
	public GameObject fpsArmRoot;

	[Header("模型初始位置")]
	[SerializeField] private Transform originalTrans;
	[Header("模型下移目标位置")]
	[SerializeField] private Transform targetTrans;

	[Header("角色前方顶住障碍物时，手臂模型下移的位置")]
	[SerializeField] private Transform topWallTrans;
	[Header("判断顶住障碍物的距离")]
	[SerializeField] private float checkWallDistance;

	[Header("插值移动速度")]
	[Header("走路时的插值速度")]
	[SerializeField] private float walkLerpSpeed;
	[Header("跑步时的插值速度")]
	[SerializeField] private float runLerpSpeed;
	[Header("下蹲时的插值速度")]
	[SerializeField] private float crouchLerpSpeed;
	[Header("瞄准时的插值速度")]
	[SerializeField] private float aimingLerpSpeed;
	[Header("顶墙时的插值速度")]
	[SerializeField] private float topWallLerpSpeed;

	[Header("记录玩家手臂当前的状态")]
	public ArmState currentArmState = ArmState.None;
	[Header("检测手臂顶墙的点位")]
	[SerializeField] private List<Transform> checkArmTopWallTransList = new List<Transform>();

	private float moveLerpSpeed; // 实际使用的插值速度

	#endregion

	#region 基本生命周期函数

	private void Update()
	{
		if (currentArmState == ArmState.None)
		{
			CheckPlayerMoveState();
			FPSArmFollow();
		}

		ArmDownWhenTopWall();
	}

	#endregion

	#region 第一人称手臂顶墙收回功能

	/// <summary>
	/// 玩家手臂顶到墙时，将手臂收回
	/// </summary>
	public void ArmDownWhenTopWall()
	{
		// 这里发射多条射线，有一条碰到了就算顶墙

		// 注意：这里用忽略 Layer 而不是选择 Layer 实现，是为了不要浪费 Layer 的数量
		int ignoreLayerIndex = (1 << playerController.layerAndTagCollection_Player.playerLayerIndex) | (1 << playerController.layerAndTagCollection_Player.airWallLayerIndex) | (1 << playerController.layerAndTagCollection_Player.checkDoorDirLayerIndex) | (1 << playerController.layerAndTagCollection_Player.doorInteractiveLayerIndex) | (1 << playerController.layerAndTagCollection_Player.enemyLayerIndex);
		ignoreLayerIndex = ~(ignoreLayerIndex);

		RaycastHit hitObj;

		for (int i = 0; i < checkArmTopWallTransList.Count; i ++)
		{
			if (Physics.Raycast(checkArmTopWallTransList[i].position, checkArmTopWallTransList[i].forward, out hitObj, checkWallDistance, ignoreLayerIndex))
			{
				if (!hitObj.collider.isTrigger)
				{
					currentArmState = ArmState.TopWall;
					fpsArmRoot.transform.localPosition = Vector3.Lerp(fpsArmRoot.transform.localPosition, topWallTrans.localPosition, topWallLerpSpeed * Time.deltaTime);

					// Debug.Log(currentArmState);
					return;
				}
			}
		}

		currentArmState = ArmState.None;
		// Debug.Log(currentArmState);
	}

	#endregion

	#region 插值跟随功能

	/// <summary>
	/// 根据玩家控制器检查玩家移动状态
	/// </summary>
	private void CheckPlayerMoveState()
	{
		if (playerController.isAiming)
		{
			moveLerpSpeed = aimingLerpSpeed;
			return;
		}

		if (playerController.playerIsRun)
		{
			moveLerpSpeed = runLerpSpeed;
			return;
		}

		if (playerController.playerIsCrouch)
		{
			moveLerpSpeed = crouchLerpSpeed;
			return;
		}

		else
		{
			moveLerpSpeed = walkLerpSpeed;
		}
	}

	private void FPSArmFollow()
	{
		if (playerController.isAiming)
		{
			fpsArmRoot.transform.localPosition = Vector3.Lerp(fpsArmRoot.transform.localPosition, originalTrans.localPosition, moveLerpSpeed * Time.deltaTime);
			return;
		}

		if (playerController.moveVelocity > 0.5f)
		{
			fpsArmRoot.transform.localPosition = Vector3.Lerp(fpsArmRoot.transform.localPosition, targetTrans.localPosition, moveLerpSpeed * Time.deltaTime);
		}
		else
		{
			fpsArmRoot.transform.localPosition = Vector3.Lerp(fpsArmRoot.transform.localPosition, originalTrans.localPosition, moveLerpSpeed * Time.deltaTime);
		}
	}

	#endregion
}
