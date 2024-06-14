using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������ܵ��������Ч������
/// </summary>
public class PlayerHitSound : MonoBehaviour
{
	#region ��������ͱ���

	// ע�⣺Ŀǰ�����ܻ����������С�ǹ̶���

	[Header("���������Դ���")]
	[SerializeField] private AudioSource hitAudioSource;

	[Header("��������ܻ������Ƶ")]
	[SerializeField] private List<AudioClip> hitAudioClipList = new List<AudioClip>();

	#endregion

	#region ���������ܻ������Ч

	/// <summary>
	/// ������������ܻ������Ƶ
	/// </summary>
	public void PlayGetHitSound()
	{
		int randomIndex = Random.Range(0, hitAudioClipList.Count);
		hitAudioSource.clip = hitAudioClipList[randomIndex];
		hitAudioSource.Play();
	}

	#endregion
}
