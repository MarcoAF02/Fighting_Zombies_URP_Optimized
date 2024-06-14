using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������������Ч������Ϸ��Ч
// ������ 3D ��Ч

// TODO: δ���ĸ��£����������

/// <summary>
/// �����������������Ч��������������ͳ����ջ���Ч
/// </summary>
public class WeaponManagerSound : MonoBehaviour
{
	#region ��������ͱ���

	[Header("�ó�װ������Դ���")]
	[SerializeField] private AudioSource getItemAudioSource;

	[Header("��Ч��������")]
	[Header("�ó���ǹ����Ч����")]
	[SerializeField] private float getPistolAudioVolume;
	[Header("�ó�С������Ч����")]
	[SerializeField] private float getKinfeAudioVolume;
	[Header("�ó�ҽ��ע��������Ч����")]
	[SerializeField] private float getSyringeAudioVolume;

	[Header("�ó���ǹ����Ƶ")]
	[SerializeField] private AudioClip getPistolAudioClip;
	[Header("�ó�С������Ƶ")]
	[SerializeField] private AudioClip getKinfeAudioClip;
	[Header("�ó�ҽ��ע��������Ƶ")]
	[SerializeField] private AudioClip getSyringeAudioClip;

	#endregion

	#region �ó�������Ч����

	/// <summary>
	/// �����ó�С������Ч
	/// </summary>
	public void PlayGetKinfeSound()
	{
		getItemAudioSource.volume = getKinfeAudioVolume;
		getItemAudioSource.clip = getKinfeAudioClip;
		getItemAudioSource.Play();
	}

	/// <summary>
	/// �����ó���ǹ����Ч
	/// </summary>
	public void PlayGetPistolSound()
	{
		getItemAudioSource.volume = getPistolAudioVolume;
		getItemAudioSource.clip = getPistolAudioClip;
		getItemAudioSource.Play();
	}

	/// <summary>
	/// �����ó�ҽ��ע��������Ч
	/// </summary>
	public void PlayGetSyringeSound()
	{
		getItemAudioSource.volume = getSyringeAudioVolume;
		getItemAudioSource.clip = getSyringeAudioClip;
		getItemAudioSource.Play();
	}

	#endregion

}
