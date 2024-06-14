using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ҽ��ע����ʹ����Ч
/// </summary>
public class PlayerSyringeSound : MonoBehaviour
{
	#region ��������ͱ���

	[Header("ҽ��ע����Դ���")]
	[SerializeField] private AudioSource syringeAudioSource;
	[Header("ҽ��ע����ƵƬ��")]
	[SerializeField] private AudioClip syringeAudioClip;
	[Header("ҽ��ע����������")]
	[SerializeField] private float syringeAudioVolume;

	#endregion

	#region ҽ��ע������Ч����

	/// <summary>
	/// ����ҽ��ע����ʹ����Ч
	/// </summary>
	public void PlaySyringeUseSound()
	{
		syringeAudioSource.volume = syringeAudioVolume;
		syringeAudioSource.clip = syringeAudioClip;
		syringeAudioSource.Play();
	}

	#endregion
}
