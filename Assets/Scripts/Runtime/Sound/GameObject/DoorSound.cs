using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Ž�������Ч
/// </summary>
public class DoorSound : MonoBehaviour
{
	#region ��������ͱ���

	[Header("����Դ���")]
	[Tooltip("��ͬ���ʵ�����ЧӦ�����в�ͬ�������Ч�زĲ��������")]
	[SerializeField] private AudioSource doorAudioSource;
	[Header("��������������Ƶ")]
	[SerializeField] private AudioClip doorOpenClip;
	[Header("�������رյ���Ƶ")]
	[SerializeField] private AudioClip doorCloseClip;
	[Header("��������ͼ��������Ƶ")]
	[SerializeField] private AudioClip lockedClip;
	[Header("��Կ�׽����ŵ���Ƶ")]
	[SerializeField] private AudioClip unlockedClip;

	#endregion

	#region ����Ƶ�Ĳ���

	/// <summary>
	/// ��������������Ч
	/// </summary>
	public void PlayDoorOpenSound()
	{
		doorAudioSource.clip = doorOpenClip;
		doorAudioSource.Play();
	}

	/// <summary>
	/// �������رյ���Ч
	/// </summary>
	public void PlayDoorCloseSound()
	{
		doorAudioSource.clip = doorCloseClip;
		doorAudioSource.Play();
	}

	/// <summary>
	/// �ű������ˣ�����������Ч
	/// </summary>
	public void PlayDoorLockedSound()
	{
		doorAudioSource.clip = lockedClip;
		doorAudioSource.Play();
	}

	/// <summary>
	/// Կ�ײ���Ž��Ž���
	/// </summary>
	public void PlayDoorUnlockedSound()
	{
		doorAudioSource.clip = unlockedClip;
		doorAudioSource.Play();
	}

	#endregion
}
