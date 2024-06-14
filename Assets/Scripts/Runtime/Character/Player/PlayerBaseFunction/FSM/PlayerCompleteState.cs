using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ҳɹ������Ϸ״̬
/// </summary>
public class PlayerCompleteState : PlayerBaseState
{
	public override void EnterState(PlayerController playerController)
	{
		Cursor.lockState = CursorLockMode.None; // �������
		playerController.PlayerStopMove();
		playerController.characterController.enabled = false; // ��ֹ���˳���׷��
		playerController.enemyViewTarget.SetActive(false);

		GameProgressManager.Instance.PerformChangeGameProgress(GameProgress.GameComplete); // �ı���Ϸ����
	}

	public override void OnUpdate(PlayerController playerController)
	{
		playerController.playerCameraController.ChangeCameraFOVWhenAiming(false); // ȡ����׼�ӽǷŴ�
	}

}
