using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 弹壳音效属于游戏音效
// 是 3D 音效

public class BulletShellSound : MonoBehaviour
{
	#region 基本组件和变量

	[Header("音源组件")]
	[SerializeField] private AudioSource bulletShellAudioSource;
	[Header("手枪弹壳掉在地上的音频片段")]
	[SerializeField] private List<AudioClip> bulletShellClipList = new List<AudioClip>();

	[Header("手枪弹壳掉在地上的音量")]
	[SerializeField] private float pistolBulletShellVolume;

	#endregion

	#region 弹壳音效播放功能

	public void PlayPistolBulletShellSound()
	{
		int randomIndex = UnityEngine.Random.Range(0, bulletShellClipList.Count);
		bulletShellAudioSource.clip = bulletShellClipList[randomIndex];
		bulletShellAudioSource.volume = pistolBulletShellVolume;
		bulletShellAudioSource.Play();
	}

	#endregion
}
