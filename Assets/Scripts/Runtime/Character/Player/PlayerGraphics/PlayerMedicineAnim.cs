using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ҽ�ư�����������
/// </summary>
public class PlayerMedicineAnim : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��������ͱ���")]
	[Header("ҽ�ư���һ�˳�ģ��")]
	[SerializeField] private GameObject syringeArm;
	[Header("ҽ�ư�����������")]
	[SerializeField] private Animator syringeAnimator;
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
			syringeAnimator.SetFloat("moveSpeed", 0f);
		}
		else
		{
			syringeAnimator.SetFloat("moveSpeed", playerController.moveVelocity);
		}
	}

	public void PlayUseAnim()
	{
		syringeAnimator.SetTrigger("use");
	}

	public void PlayHideAnim()
	{
		syringeAnimator.SetTrigger("hide");
	}

	#endregion
}
