using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ҵ�Ϣ����Ч����
/// </summary>
public class PlayerBreathSound : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��ҵ���")]
	[SerializeField] private AudioSource playerMouth;

	[Header("��·ʱ�Ĵ�������")]
	[SerializeField] private float walkBreathVolume;
	[Header("�ܲ�ʱ�Ĵ�������")]
	[SerializeField] private float runBreathVolume;
	[Header("��׼ʱ�Ĵ�������")]
	[SerializeField] private float aimingBreathVolume;
	[Header("����ʱ��е�����")]
	[SerializeField] private float attackRoarVolume;
	[Header("����ʱ��е�����")]
	[SerializeField] private float getHurtRoarVolume;
	[Header("����ʱ��е�����")]
	[SerializeField] private float deadRoarVolume;
	[Header("����ֵ�ָ�ʱ���ɴ�������")]
	[SerializeField] private float healthRecoverVolume;

	[Header("��·���������ʱ��")]
	[SerializeField] private float walkBreathIntervalTime;
	private float walkBreathTotalTime;
	[Header("�ܲ����������ʱ��")]
	[SerializeField] private float runBreathIntervalTime;
	private float runBreathTotalTime;
	[Header("��׼���������ʱ��")]
	[SerializeField] private float aimingBreathIntervalTime;
	private float aimingBreathTotalTime;

	[Header("��Һ���������Ƶ")]
	[SerializeField] private AudioClip breathAudioClip;
	[Header("���������Ƶ")]
	[SerializeField] private List<AudioClip> attackRoarClipList = new List<AudioClip>();

	[Header("���˺����Ƶ")]
	[SerializeField] private List<AudioClip> getHurtClipList = new List<AudioClip>();
	[Header("���������Ƶ")]
	[SerializeField] private List<AudioClip> deadClipList = new List<AudioClip>();
	[Header("����ֵ�ָ�ʱ�ķ�����Ƶ")]
	[SerializeField] private List<AudioClip> healthRecoverClipList = new List<AudioClip>();

	#endregion

	#region ����ƶ�������

	/// <summary>
	/// ��·ʱ����Ҵ�����
	/// </summary>
	public void PlayWalkBreathSound()
	{
		walkBreathTotalTime += Time.deltaTime;
		if (walkBreathTotalTime > 32768f) walkBreathTotalTime = 32768f;

		if (walkBreathTotalTime > walkBreathIntervalTime)
		{
			walkBreathTotalTime = 0f;
			playerMouth.volume = walkBreathVolume;
			playerMouth.clip = breathAudioClip;
			playerMouth.Play();
		}
	}

	/// <summary>
	/// �ܲ�ʱ����Ҵ�����
	/// </summary>
	public void PlayRunBreathSound()
	{
		runBreathTotalTime += Time.deltaTime;
		if (runBreathTotalTime > 32768f) runBreathTotalTime = 32768f;

		if (runBreathTotalTime > runBreathIntervalTime)
		{
			runBreathTotalTime = 0f;
			playerMouth.volume = runBreathVolume;
			playerMouth.clip = breathAudioClip;
			playerMouth.Play();
		}
	}

	/// <summary>
	/// ��׼ʱ����Ҵ�����
	/// </summary>
	public void PlayAimingBreathSound()
	{
		aimingBreathTotalTime += Time.deltaTime;
		if (aimingBreathTotalTime > 32768f) aimingBreathTotalTime = 32768f;

		if (aimingBreathTotalTime > aimingBreathIntervalTime)
		{
			aimingBreathTotalTime = 0f;
			playerMouth.volume = aimingBreathVolume;
			playerMouth.clip = breathAudioClip;
			playerMouth.Play();
		}
	}

	#endregion

	#region ��ҹ��������

	/// <summary>
	/// ����ý�ս��������ʱ�ĺ����
	/// </summary>
	public void PlayAttackRoarSound()
	{
		playerMouth.volume = attackRoarVolume;

		walkBreathTotalTime = 0f;
		runBreathTotalTime = 0f;

		int randomIndex = Random.Range(0, attackRoarClipList.Count);
		playerMouth.clip = attackRoarClipList[randomIndex];
		playerMouth.Play();
	}

	#endregion

	#region ������˺����������

	/// <summary>
	/// ����������˺����Ч
	/// </summary>
	public void PlayGetHurtRoarSound()
	{
		playerMouth.volume = getHurtRoarVolume;

		walkBreathTotalTime = 0f;
		runBreathTotalTime = 0f;

		int randomIndex = Random.Range(0, getHurtClipList.Count);
		playerMouth.clip = getHurtClipList[randomIndex];
		playerMouth.Play();
	}

	/// <summary>
	/// ����������������Ч
	/// </summary>
	public void PlayDeadRoarSound()
	{
		playerMouth.volume = deadRoarVolume;

		walkBreathTotalTime = 0f;
		runBreathTotalTime = 0f;

		int randomIndex = Random.Range(0, deadClipList.Count);
		playerMouth.clip = deadClipList[randomIndex];
		playerMouth.Play();
	}

	#endregion

	#region ����ֵ�ָ��ķ��ɴ�����

	/// <summary>
	/// ����ֵ�ָ��ķ��ɴ�����
	/// </summary>
	public void PlayRecoverBreathSound()
	{
		playerMouth.volume = healthRecoverVolume;

		walkBreathTotalTime = 0f;
		runBreathTotalTime = 0f;

		int randomIndex = Random.Range(0, healthRecoverClipList.Count);
		playerMouth.clip = healthRecoverClipList[randomIndex];
		playerMouth.Play();
	}

	#endregion

}
