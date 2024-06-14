using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家成功完成游戏状态
/// </summary>
public class PlayerCompleteState : PlayerBaseState
{
	public override void EnterState(PlayerController playerController)
	{
		Cursor.lockState = CursorLockMode.None; // 解锁鼠标
		playerController.PlayerStopMove();
		playerController.characterController.enabled = false; // 防止敌人持续追击
		playerController.enemyViewTarget.SetActive(false);

		GameProgressManager.Instance.PerformChangeGameProgress(GameProgress.GameComplete); // 改变游戏进度
	}

	public override void OnUpdate(PlayerController playerController)
	{
		playerController.playerCameraController.ChangeCameraFOVWhenAiming(false); // 取消瞄准视角放大
	}

}
