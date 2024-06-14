using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� UI ������
/// </summary>
public class WeaponUIController : MonoBehaviour
{
	// �ýű�������������� UI����Ҫ��׼�����

	#region ��������ͱ���

	[Header("��ҽ�ɫ������")]
	[SerializeField] private PlayerController playerController;
	[Header("���׼�Ǹ�����")]
	[SerializeField] private GameObject frontSightParent;

	#endregion

	#region �����������ں���

	private void OnEnable()
	{
		playerController.eventHandler_Player.PlayerStateChangeEvent += ShowAndHideFrontSight;
	}

	private void OnDisable()
	{
		playerController.eventHandler_Player.PlayerStateChangeEvent -= ShowAndHideFrontSight;
	}

	#endregion

	#region ���׼����ع���

	private void ShowAndHideFrontSight(PlayerBaseState playerState)
	{
		if (playerState == playerController.playerControlState)
		{
			frontSightParent.SetActive(true);
		}
		else
		{
			frontSightParent.SetActive(false);
		}
	}

	#endregion
}
