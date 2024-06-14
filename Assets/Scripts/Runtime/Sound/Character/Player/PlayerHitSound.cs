using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家身体受到击打的音效控制器
/// </summary>
public class PlayerHitSound : MonoBehaviour
{
	#region 基本组件和变量

	// 注意：目前身体受击打的音量大小是固定的

	[Header("玩家身体音源组件")]
	[SerializeField] private AudioSource hitAudioSource;

	[Header("玩家身体受击打的音频")]
	[SerializeField] private List<AudioClip> hitAudioClipList = new List<AudioClip>();

	#endregion

	#region 播放身体受击打的音效

	/// <summary>
	/// 播放玩家身体受击打的音频
	/// </summary>
	public void PlayGetHitSound()
	{
		int randomIndex = Random.Range(0, hitAudioClipList.Count);
		hitAudioSource.clip = hitAudioClipList[randomIndex];
		hitAudioSource.Play();
	}

	#endregion
}
