using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������Ч������Ϸ��Ч
// �� 3D ��Ч

public class BulletShellSound : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��Դ���")]
	[SerializeField] private AudioSource bulletShellAudioSource;
	[Header("��ǹ���ǵ��ڵ��ϵ���ƵƬ��")]
	[SerializeField] private List<AudioClip> bulletShellClipList = new List<AudioClip>();

	[Header("��ǹ���ǵ��ڵ��ϵ�����")]
	[SerializeField] private float pistolBulletShellVolume;

	#endregion

	#region ������Ч���Ź���

	public void PlayPistolBulletShellSound()
	{
		int randomIndex = UnityEngine.Random.Range(0, bulletShellClipList.Count);
		bulletShellAudioSource.clip = bulletShellClipList[randomIndex];
		bulletShellAudioSource.volume = pistolBulletShellVolume;
		bulletShellAudioSource.Play();
	}

	#endregion
}
