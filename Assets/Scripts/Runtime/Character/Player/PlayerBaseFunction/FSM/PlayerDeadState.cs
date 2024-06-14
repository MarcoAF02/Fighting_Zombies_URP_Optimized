using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家死亡状态
/// </summary>
public class PlayerDeadState : PlayerBaseState
{
	public override void EnterState(PlayerController playerController)
	{
		Cursor.lockState = CursorLockMode.None; // 解锁鼠标
		playerController.PlayerStopMove();

		// 停止手中武器的一切动作
		playerController.weaponManager.playerKinfeAttacking.StopAllCoroutines();
		playerController.weaponManager.playerPistolShooting.StopAllCoroutines();
		playerController.weaponManager.playerSyringeUsing.StopAllCoroutines();

		playerController.weaponManager.GetDownCurrentWeapon(); // 放下手中的武器
		playerController.playerInventory.inventoryAndGameTargetController.inventoryUIRootObj.SetActive(false); // 关闭物品栏 UI 显示

		playerController.characterController.enabled = false;
		playerController.enemyViewTarget.SetActive(false);
		playerController.playerCameraController.ChangeCameraPosWhenDie();

		GameProgressManager.Instance.PerformChangeGameProgress(GameProgress.GameFail); // 改变游戏进度

		Debug.Log("玩家进入死亡状态");
	}

	public override void OnUpdate(PlayerController playerController)
	{
		playerController.playerCameraController.ChangeCameraFOVWhenAiming(false); // 取消瞄准视角放大
	}

}
