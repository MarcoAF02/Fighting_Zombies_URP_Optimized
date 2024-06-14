using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家打开物品栏时需要执行的操作
/// </summary>
public class PlayerOpenInventoryState : PlayerBaseState
{
	public override void EnterState(PlayerController playerController)
	{
		Cursor.lockState = CursorLockMode.None; // 解锁鼠标

		playerController.PlayerStopMove();
		playerController.weaponManager.GetDownCurrentWeapon();
		playerController.playerInventory.inventoryAndGameTargetController.inventoryUIRootObj.SetActive(true);

		playerController.flashLight.enabled = true;
		playerController.fpsItemViewLight.enabled = false;

		playerController.playerInventory.openTotalTime = playerController.playerInventory.openCDTime;
	}

	public override void OnUpdate(PlayerController playerController)
	{
		playerController.playerCameraController.ChangeCameraFOVWhenAiming(false);

		// 进入本状态，才会计算物品栏交互的 CD 时间
		playerController.playerInventory.openTotalTime -= Time.deltaTime;
		if (playerController.playerInventory.openTotalTime <= 0f) playerController.playerInventory.openTotalTime = 0f;
		playerController.playerInventory.PlayerCloseInventory();
	}

}
