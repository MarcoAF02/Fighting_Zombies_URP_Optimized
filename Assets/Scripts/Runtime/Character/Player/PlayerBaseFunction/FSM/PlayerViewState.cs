using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家在端详某个物件时需要执行的内容
/// </summary>
public class PlayerViewState : PlayerBaseState
{
	public override void EnterState(PlayerController playerController)
	{
		playerController.PlayerStopMove();
		playerController.weaponManager.GetDownCurrentWeapon();
		playerController.playerItemInteractive.hideViewItemTotalTime = 0f; // 重置端详 CD 时间

		playerController.flashLight.enabled = false;
		playerController.fpsItemViewLight.enabled = true;
	}

	public override void OnUpdate(PlayerController playerController)
	{
		playerController.playerCameraController.ChangeCameraFOVWhenAiming(false);

		// 进入本状态，才会计算物品展示的 CD 时间
		playerController.playerItemInteractive.hideViewItemTotalTime = playerController.playerItemInteractive.hideViewItemTotalTime + Time.deltaTime;
		playerController.playerItemInteractive.PlayerHideShowItemObj();
	}

}
