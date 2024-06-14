using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ҵ���Ʒ��ʱ��Ҫִ�еĲ���
/// </summary>
public class PlayerOpenInventoryState : PlayerBaseState
{
	public override void EnterState(PlayerController playerController)
	{
		Cursor.lockState = CursorLockMode.None; // �������

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

		// ���뱾״̬���Ż������Ʒ�������� CD ʱ��
		playerController.playerInventory.openTotalTime -= Time.deltaTime;
		if (playerController.playerInventory.openTotalTime <= 0f) playerController.playerInventory.openTotalTime = 0f;
		playerController.playerInventory.PlayerCloseInventory();
	}

}
