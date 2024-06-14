using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家自主控制角色的状态
/// </summary>
public class PlayerControlState : PlayerBaseState
{
	public override void EnterState(PlayerController playerController)
	{
		Cursor.lockState = CursorLockMode.Locked;

		playerController.playerCameraController.SetupPlayerView(); // 设置摄像机角度限制

		if (playerController.playerLastState != playerController.gamePauseState)
		{
			playerController.weaponManager.PlayerRestartWeapon(); // 重新拿起上次放下的武器
		}

		playerController.flashLight.enabled = true;
		playerController.fpsItemViewLight.enabled = false; // 玩家第一人称照明

		playerController.tutorialTrigger.TutorialPlayerMove(); // 玩家教学信息
		playerController.tipMessageController.ClearInteractiveMessage(); // 每次进入该状态的时候，清除交互提示信息

		playerController.playerInventory.inventoryAndGameTargetController.inventoryUIRootObj.SetActive(false); // 关闭物品栏

		// 重新计算物品栏打开时间
		playerController.playerInventory.closeTotalTime = playerController.playerInventory.closeCDTime;

		// 进入玩家控制状态，重置下滑音效的 CD 时间
		playerController.playerFootstepSound.slideFootstepTotalTime = playerController.playerFootstepSound.slideFootstepIntervalTime;
	}

	public override void OnUpdate(PlayerController playerController)
	{
		playerController.PlayerPerformGravity();
		playerController.PlayerActionCheck();
		playerController.PlayerGroundCheck();
		playerController.PlayerSlopeCheck();
		playerController.CheckPlayerMoveForward();
		playerController.CheckPlayerMoveBack();
		playerController.PlayerPerformRun();
		playerController.PlayerPerformCrouch();
		playerController.PlayerPerformMove();
		playerController.PlayerSlideOnSlope();
		playerController.ChangeCameraPos();

		playerController.playerFootstepSound.GroundMaterialCheck();
		playerController.playerFootstepSound.PerformFootstepSound(); // 播放玩家脚步声

		if (playerController.playerIsMove && playerController.weaponManager.playerGunState == PlayerGunState.Standby)
		{
			if (playerController.isAiming)
			{
				playerController.playerBreathSound.PlayAimingBreathSound();
			}
			else
			{
				if (playerController.playerIsRun)
				{
					playerController.playerBreathSound.PlayRunBreathSound();
				}
				else
				{
					playerController.playerBreathSound.PlayWalkBreathSound();
				}
			}
		}

		playerController.playerInventory.closeTotalTime -= Time.deltaTime;
		if (playerController.playerInventory.closeTotalTime <= 0f) playerController.playerInventory.closeTotalTime = 0f;
		playerController.playerInventory.PlayerOpenInventory(); // 打开物品栏

		playerController.playerItemInteractive.PlayerCollectItem();
		playerController.playerDoorInteractive.PlayerOperateDoor();
		playerController.playerFurnitureInteractive.PerformInteractiveFurniture();

		playerController.playerCameraController.PlayerViewRotate();
		playerController.playerCameraController.ChangeCameraFOVWhenAiming(playerController.isAiming);

		playerController.weaponManager.PlayerPerformSwitchToSecondaryWeapon();
		playerController.weaponManager.PlayerPerformSwitchToMeleeWeapon();
		playerController.weaponManager.PlayerPerformSwitchToMedicine();

		// 手枪（副武器）
		if (playerController.weaponManager.playerPistolShooting.enabled)
		{
			playerController.weaponManager.playerPistolShooting.ComputeSpreadValue();
			playerController.weaponManager.playerPistolShooting.PlayerPerformAim();
			playerController.weaponManager.playerPistolShooting.PlayerPerformShoot();
			playerController.weaponManager.playerPistolShooting.PlayerPerformReload();
			playerController.weaponManager.playerPistolShooting.PlayerPerformReloadWhenEmpty();
		}

		// 小刀
		if (playerController.weaponManager.playerKinfeAttacking.enabled)
		{
			playerController.weaponManager.playerKinfeAttacking.PlayerPerformKinfeAttack();
		}

		// 医疗包
		if (playerController.weaponManager.playerSyringeUsing.enabled)
		{
			playerController.weaponManager.playerSyringeUsing.PlayerPerformRestore();
		}

		playerController.tutorialTrigger.PlayerFirstViewEnemy_Tutorial(); // 玩家教学信息

		// 从暂停菜单返回时，有非常短暂的时间不能瞄准
		playerController.fixAimActionCurTime -= Time.deltaTime;
		if (playerController.fixAimActionCurTime <= 0f) playerController.fixAimActionCurTime = 0f;
	}

}
