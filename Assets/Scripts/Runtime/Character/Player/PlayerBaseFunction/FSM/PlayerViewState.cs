using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ڶ���ĳ�����ʱ��Ҫִ�е�����
/// </summary>
public class PlayerViewState : PlayerBaseState
{
	public override void EnterState(PlayerController playerController)
	{
		playerController.PlayerStopMove();
		playerController.weaponManager.GetDownCurrentWeapon();
		playerController.playerItemInteractive.hideViewItemTotalTime = 0f; // ���ö��� CD ʱ��

		playerController.flashLight.enabled = false;
		playerController.fpsItemViewLight.enabled = true;
	}

	public override void OnUpdate(PlayerController playerController)
	{
		playerController.playerCameraController.ChangeCameraFOVWhenAiming(false);

		// ���뱾״̬���Ż������Ʒչʾ�� CD ʱ��
		playerController.playerItemInteractive.hideViewItemTotalTime = playerController.playerItemInteractive.hideViewItemTotalTime + Time.deltaTime;
		playerController.playerItemInteractive.PlayerHideShowItemObj();
	}

}
