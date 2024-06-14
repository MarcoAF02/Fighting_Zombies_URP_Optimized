using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 手枪音效是游戏音效
// 不属于 3D 音效

/// <summary>
/// 玩家手枪音效管理
/// </summary>
public class PlayerPistolSound : MonoBehaviour
{
	#region 基本组件和变量

	[Header("开枪音源组件")]
	[SerializeField] private AudioSource pistolShootAudioSource;
	[Header("换弹音源组件")]
	[SerializeField] private AudioSource pistolReloadAudioSource;

	[Header("音源组件音量设置")]
	[SerializeField] private float shootVolumeLevel;
	[SerializeField] private float reloadVolumeLevel;
	[SerializeField] private float triggerVolumeLevel;

	[Header("手枪换弹音效")]
	[Header("弹匣退出")]
	[SerializeField] private AudioClip magOutAudio;
	[Header("弹匣装回去")]
	[SerializeField] private AudioClip magInAudio;
	[Header("恢复空仓挂机")]
	[SerializeField] private AudioClip magResetAudio;

	[Header("弹匣退出的延迟时间")]
	[SerializeField] private float magOutDelayTime;
	[Header("弹匣退出到装进去持续多长时间")]
	[SerializeField] private float magOutSustainTime;
	[Header("弹匣装进去多久恢复空仓挂机")]
	[SerializeField] private float magInResetTime;

	[Header("弹匣打空时的撞针音效")]
	[SerializeField] private AudioClip emptyTriggerAudio;

	[Header("手枪使用的开枪音效列表")]
	[Tooltip("开枪时随机取一个音效")]
	[SerializeField] private List<AudioClip> shootAudioList = new List<AudioClip>();

	// 协程
	private Coroutine PlayPistolReloadSound_IECor;

	#endregion

	#region 音效播放功能

	/// <summary>
	/// 开枪音效
	/// </summary>
	public void PlayPistolShootSound()
	{
		pistolShootAudioSource.volume = shootVolumeLevel;

		int randomIndex = UnityEngine.Random.Range(0, shootAudioList.Count);
		pistolShootAudioSource.clip = shootAudioList[randomIndex];

		pistolShootAudioSource.Play();
	}

	/// <summary>
	/// 弹匣打空时播放撞针音效
	/// </summary>
	public void PlayEmptyTriggerSound()
	{
		pistolShootAudioSource.volume = triggerVolumeLevel;
		pistolShootAudioSource.clip = emptyTriggerAudio;
		pistolShootAudioSource.Play();
	}

	/// <summary>
	/// 手枪换弹时播放换弹音效
	/// </summary>
	public void PlayPistolReloadSound()
	{
		PlayPistolReloadSound_IECor = StartCoroutine(PlayPistolReloadSound_IE());
	}

	private IEnumerator PlayPistolReloadSound_IE()
	{
		pistolReloadAudioSource.volume = reloadVolumeLevel;

		yield return new WaitForSeconds(magOutDelayTime);

		pistolReloadAudioSource.clip = magOutAudio;
		pistolReloadAudioSource.Play();

		yield return new WaitForSeconds(magOutSustainTime);

		pistolReloadAudioSource.clip = magInAudio;
		pistolReloadAudioSource.Play();

		yield return new WaitForSeconds(magInResetTime);

		pistolReloadAudioSource.clip = magResetAudio;
		pistolReloadAudioSource.Play();
	}

	#endregion
}
