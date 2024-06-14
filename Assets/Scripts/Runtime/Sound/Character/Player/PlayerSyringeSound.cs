using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家医疗注射器使用音效
/// </summary>
public class PlayerSyringeSound : MonoBehaviour
{
	#region 基本组件和变量

	[Header("医疗注射音源组件")]
	[SerializeField] private AudioSource syringeAudioSource;
	[Header("医疗注射音频片段")]
	[SerializeField] private AudioClip syringeAudioClip;
	[Header("医疗注射音量设置")]
	[SerializeField] private float syringeAudioVolume;

	#endregion

	#region 医疗注射针音效播放

	/// <summary>
	/// 播放医疗注射针使用音效
	/// </summary>
	public void PlaySyringeUseSound()
	{
		syringeAudioSource.volume = syringeAudioVolume;
		syringeAudioSource.clip = syringeAudioClip;
		syringeAudioSource.Play();
	}

	#endregion
}
