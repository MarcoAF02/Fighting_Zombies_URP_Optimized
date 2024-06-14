using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����������ƽ�ɫ��״̬
/// </summary>
public class PlayerControlState : PlayerBaseState
{
	public override void EnterState(PlayerController playerController)
	{
		Cursor.lockState = CursorLockMode.Locked;

		playerController.playerCameraController.SetupPlayerView(); // ����������Ƕ�����

		if (playerController.playerLastState != playerController.gamePauseState)
		{
			playerController.weaponManager.PlayerRestartWeapon(); // ���������ϴη��µ�����
		}

		playerController.flashLight.enabled = true;
		playerController.fpsItemViewLight.enabled = false; // ��ҵ�һ�˳�����

		playerController.tutorialTrigger.TutorialPlayerMove(); // ��ҽ�ѧ��Ϣ
		playerController.tipMessageController.ClearInteractiveMessage(); // ÿ�ν����״̬��ʱ�����������ʾ��Ϣ

		playerController.playerInventory.inventoryAndGameTargetController.inventoryUIRootObj.SetActive(false); // �ر���Ʒ��

		// ���¼�����Ʒ����ʱ��
		playerController.playerInventory.closeTotalTime = playerController.playerInventory.closeCDTime;

		// ������ҿ���״̬�������»���Ч�� CD ʱ��
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
		playerController.playerFootstepSound.PerformFootstepSound(); // ������ҽŲ���

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
		playerController.playerInventory.PlayerOpenInventory(); // ����Ʒ��

		playerController.playerItemInteractive.PlayerCollectItem();
		playerController.playerDoorInteractive.PlayerOperateDoor();
		playerController.playerFurnitureInteractive.PerformInteractiveFurniture();

		playerController.playerCameraController.PlayerViewRotate();
		playerController.playerCameraController.ChangeCameraFOVWhenAiming(playerController.isAiming);

		playerController.weaponManager.PlayerPerformSwitchToSecondaryWeapon();
		playerController.weaponManager.PlayerPerformSwitchToMeleeWeapon();
		playerController.weaponManager.PlayerPerformSwitchToMedicine();

		// ��ǹ����������
		if (playerController.weaponManager.playerPistolShooting.enabled)
		{
			playerController.weaponManager.playerPistolShooting.ComputeSpreadValue();
			playerController.weaponManager.playerPistolShooting.PlayerPerformAim();
			playerController.weaponManager.playerPistolShooting.PlayerPerformShoot();
			playerController.weaponManager.playerPistolShooting.PlayerPerformReload();
			playerController.weaponManager.playerPistolShooting.PlayerPerformReloadWhenEmpty();
		}

		// С��
		if (playerController.weaponManager.playerKinfeAttacking.enabled)
		{
			playerController.weaponManager.playerKinfeAttacking.PlayerPerformKinfeAttack();
		}

		// ҽ�ư�
		if (playerController.weaponManager.playerSyringeUsing.enabled)
		{
			playerController.weaponManager.playerSyringeUsing.PlayerPerformRestore();
		}

		playerController.tutorialTrigger.PlayerFirstViewEnemy_Tutorial(); // ��ҽ�ѧ��Ϣ

		// ����ͣ�˵�����ʱ���зǳ����ݵ�ʱ�䲻����׼
		playerController.fixAimActionCurTime -= Time.deltaTime;
		if (playerController.fixAimActionCurTime <= 0f) playerController.fixAimActionCurTime = 0f;
	}

}
