using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// С����Ч����Ϸ��Ч
// ������ 3D ��Ч

/// <summary>
/// ���С����Ч����
/// </summary>
public class PlayerKinfeSound : MonoBehaviour
{
	#region ��������ͱ���

	[Header("С��������Դ���")]
	[SerializeField] private AudioSource kinfeWaveAudioSource;

	[Header("С��������������")]
	[SerializeField] private float kinfeWaveVolume;

	[Header("С��������Ƶ�б�")]
	[SerializeField] private List<AudioClip> kinfeWaveAudioClipList = new List<AudioClip>();

	#endregion

	#region С��������Ч����

	public void PlayKinfeWaveSound()
	{
		kinfeWaveAudioSource.volume = kinfeWaveVolume;
		int randomIndex = UnityEngine.Random.Range(0, kinfeWaveAudioClipList.Count);
		kinfeWaveAudioSource.clip = kinfeWaveAudioClipList[randomIndex];
		kinfeWaveAudioSource.Play();
	}

	#endregion
}
