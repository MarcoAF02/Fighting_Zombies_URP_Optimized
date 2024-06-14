using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 武器管理器音效属于游戏音效
// 不属于 3D 音效

// TODO: 未来的更新：更多的武器

/// <summary>
/// 玩家武器管理器的音效，负责玩家武器掏出和收回音效
/// </summary>
public class WeaponManagerSound : MonoBehaviour
{
	#region 基本组件和变量

	[Header("拿出装备的音源组件")]
	[SerializeField] private AudioSource getItemAudioSource;

	[Header("音效音量设置")]
	[Header("拿出手枪的音效音量")]
	[SerializeField] private float getPistolAudioVolume;
	[Header("拿出小刀的音效音量")]
	[SerializeField] private float getKinfeAudioVolume;
	[Header("拿出医疗注射器的音效音量")]
	[SerializeField] private float getSyringeAudioVolume;

	[Header("拿出手枪的音频")]
	[SerializeField] private AudioClip getPistolAudioClip;
	[Header("拿出小刀的音频")]
	[SerializeField] private AudioClip getKinfeAudioClip;
	[Header("拿出医疗注射器的音频")]
	[SerializeField] private AudioClip getSyringeAudioClip;

	#endregion

	#region 拿出武器音效播放

	/// <summary>
	/// 播放拿出小刀的音效
	/// </summary>
	public void PlayGetKinfeSound()
	{
		getItemAudioSource.volume = getKinfeAudioVolume;
		getItemAudioSource.clip = getKinfeAudioClip;
		getItemAudioSource.Play();
	}

	/// <summary>
	/// 播放拿出手枪的音效
	/// </summary>
	public void PlayGetPistolSound()
	{
		getItemAudioSource.volume = getPistolAudioVolume;
		getItemAudioSource.clip = getPistolAudioClip;
		getItemAudioSource.Play();
	}

	/// <summary>
	/// 播放拿出医疗注射器的音效
	/// </summary>
	public void PlayGetSyringeSound()
	{
		getItemAudioSource.volume = getSyringeAudioVolume;
		getItemAudioSource.clip = getSyringeAudioClip;
		getItemAudioSource.Play();
	}

	#endregion

}
