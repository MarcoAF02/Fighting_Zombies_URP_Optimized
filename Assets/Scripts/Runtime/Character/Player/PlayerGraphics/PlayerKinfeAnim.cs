using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// С���Ķ���������
/// </summary>
public class PlayerKinfeAnim : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��������ͱ���")]
	[Header("С����һ�˳ƽ�ɫģ��")]
	[SerializeField] private GameObject kinfeArm;
	[Header("С������������")]
	[SerializeField] private Animator kinfeAnimator;
	[Header("��ɫ������")]
	[SerializeField] private PlayerController playerController;

	#endregion

	#region �����������ں���

	private void Update()
	{
		MoveAnimation();
	}

	#endregion

	#region �������º���

	private void MoveAnimation()
	{
		if (playerController.playerIsSliding)
		{
			kinfeAnimator.SetFloat("moveSpeed", 0f);
		}
		else
		{
			kinfeAnimator.SetFloat("moveSpeed", playerController.moveVelocity);
		}
	}

	public void PlayAttackAnim()
	{
		kinfeAnimator.SetTrigger("attack");
	}

	public void PlayHideAnim()
	{
		kinfeAnimator.SetTrigger("hide");
	}

	#endregion
}
