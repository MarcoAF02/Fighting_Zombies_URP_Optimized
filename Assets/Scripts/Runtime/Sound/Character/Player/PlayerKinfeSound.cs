using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 小刀音效是游戏音效
// 不属于 3D 音效

/// <summary>
/// 玩家小刀音效管理
/// </summary>
public class PlayerKinfeSound : MonoBehaviour
{
	#region 基本组件和变量

	[Header("小刀挥舞音源组件")]
	[SerializeField] private AudioSource kinfeWaveAudioSource;

	[Header("小刀挥舞音量设置")]
	[SerializeField] private float kinfeWaveVolume;

	[Header("小刀挥舞音频列表")]
	[SerializeField] private List<AudioClip> kinfeWaveAudioClipList = new List<AudioClip>();

	#endregion

	#region 小刀挥舞音效播放

	public void PlayKinfeWaveSound()
	{
		kinfeWaveAudioSource.volume = kinfeWaveVolume;
		int randomIndex = UnityEngine.Random.Range(0, kinfeWaveAudioClipList.Count);
		kinfeWaveAudioSource.clip = kinfeWaveAudioClipList[randomIndex];
		kinfeWaveAudioSource.Play();
	}

	#endregion
}
