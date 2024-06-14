using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ǹ�Ķ���������
/// </summary>
public class PlayerPistolAnim : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��������ͱ���")]
	[Header("��ǹ��һ�˳ƽ�ɫģ��")]
	[SerializeField] private GameObject pistolArm;
	[Header("��ǹ����������")]
	[SerializeField] private Animator pistolAnimator;
	[Header("��ɫ������")]
	[SerializeField] private PlayerController playerController;

	#endregion

	#region �����������ں���

	private void Update()
	{
		MoveAnimation();
		AimStateAnimation();
	}

	#endregion

	#region �������º���

	private void MoveAnimation()
	{
		if (playerController.playerIsSliding)
		{
			pistolAnimator.SetFloat("moveSpeed", 0f);
		}
		else
		{
			pistolAnimator.SetFloat("moveSpeed", playerController.moveVelocity);
		}
	}

	private void AimStateAnimation()
	{
		pistolAnimator.SetBool("shootState", playerController.isAiming);
	}

	public void PlayHideAnim()
	{
		pistolAnimator.SetTrigger("hide");
	}

	public void PlayShootAnim()
	{
		pistolAnimator.SetTrigger("shoot");
	}

	public void PlayReloadAnim()
	{
		pistolAnimator.SetTrigger("reload");
	}

	#endregion
}
