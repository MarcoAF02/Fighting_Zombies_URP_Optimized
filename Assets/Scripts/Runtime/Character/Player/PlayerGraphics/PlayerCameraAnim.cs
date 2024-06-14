using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� FPS �������������
/// </summary>
public class PlayerCameraAnim : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��ҽ�ɫ������")]
	[SerializeField] private PlayerController playerController;
	[Header("����������")]
	[SerializeField] private Animator fpsCameraAnimator;

	[Header("�Ƿ�Ҫ�����ӽ�ҡ�Σ���Ϸ��ʼʱ�� SettingLoader ��ȡ")]
	public bool turnOnThePerspectiveShake;

	#endregion

	#region �����������ں���

	private void OnEnable()
	{
		playerController.eventHandler_Player.HealthChangeEvent += TakeDamageVibrateAnim;
	}

	private void Update()
	{
		MoveAnim();
	}

	private void OnDisable()
	{
		playerController.eventHandler_Player.HealthChangeEvent -= TakeDamageVibrateAnim;
	}

	#endregion

	#region ���ֶ�������

	// �ƶ��ӽ�ҡ�ζ�������
	private void MoveAnim()
	{
		if (!turnOnThePerspectiveShake) return;
		if (playerController.playerIsSliding)
		{
			fpsCameraAnimator.SetFloat("moveSpeed", 0f);
		}
		else
		{
			fpsCameraAnimator.SetFloat("moveSpeed", playerController.moveVelocity);
		}
	}

	// ��ǹ���ʱ�����
	public void PistolShootVibrateAnim()
	{
		fpsCameraAnimator.SetTrigger("pistolShoot");
	}
	
	/// <summary>
	/// С������ʱ�����
	/// </summary>
	public void KinfeAttackVibrateAnim()
	{
		fpsCameraAnimator.SetTrigger("kinfeAttack");
	}

	/// <summary>
	/// �ܻ�����ʱ�����
	/// </summary>
	/// <param name="isDamage"></param>
	/// <param name="health"></param>
	public void TakeDamageVibrateAnim(bool isDamage, float health)
	{
		if (isDamage)
		{
			fpsCameraAnimator.SetTrigger("hit");
		}
	}

	#endregion
}
