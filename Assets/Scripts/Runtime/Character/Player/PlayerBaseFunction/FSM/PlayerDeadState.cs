using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������״̬
/// </summary>
public class PlayerDeadState : PlayerBaseState
{
	public override void EnterState(PlayerController playerController)
	{
		Cursor.lockState = CursorLockMode.None; // �������
		playerController.PlayerStopMove();

		// ֹͣ����������һ�ж���
		playerController.weaponManager.playerKinfeAttacking.StopAllCoroutines();
		playerController.weaponManager.playerPistolShooting.StopAllCoroutines();
		playerController.weaponManager.playerSyringeUsing.StopAllCoroutines();

		playerController.weaponManager.GetDownCurrentWeapon(); // �������е�����
		playerController.playerInventory.inventoryAndGameTargetController.inventoryUIRootObj.SetActive(false); // �ر���Ʒ�� UI ��ʾ

		playerController.characterController.enabled = false;
		playerController.enemyViewTarget.SetActive(false);
		playerController.playerCameraController.ChangeCameraPosWhenDie();

		GameProgressManager.Instance.PerformChangeGameProgress(GameProgress.GameFail); // �ı���Ϸ����

		Debug.Log("��ҽ�������״̬");
	}

	public override void OnUpdate(PlayerController playerController)
	{
		playerController.playerCameraController.ChangeCameraFOVWhenAiming(false); // ȡ����׼�ӽǷŴ�
	}

}
