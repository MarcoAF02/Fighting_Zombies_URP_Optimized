using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 门交互的音效
/// </summary>
public class DoorSound : MonoBehaviour
{
	#region 基本组件和变量

	[Header("门音源组件")]
	[Tooltip("不同材质的门音效应该略有不同，如果音效素材不够请忽略")]
	[SerializeField] private AudioSource doorAudioSource;
	[Header("门正常开启的音频")]
	[SerializeField] private AudioClip doorOpenClip;
	[Header("门正常关闭的音频")]
	[SerializeField] private AudioClip doorCloseClip;
	[Header("门锁上试图拉动的音频")]
	[SerializeField] private AudioClip lockedClip;
	[Header("用钥匙解锁门的音频")]
	[SerializeField] private AudioClip unlockedClip;

	#endregion

	#region 门音频的播放

	/// <summary>
	/// 门正常开启的音效
	/// </summary>
	public void PlayDoorOpenSound()
	{
		doorAudioSource.clip = doorOpenClip;
		doorAudioSource.Play();
	}

	/// <summary>
	/// 门正常关闭的音效
	/// </summary>
	public void PlayDoorCloseSound()
	{
		doorAudioSource.clip = doorCloseClip;
		doorAudioSource.Play();
	}

	/// <summary>
	/// 门被锁上了，播放上锁音效
	/// </summary>
	public void PlayDoorLockedSound()
	{
		doorAudioSource.clip = lockedClip;
		doorAudioSource.Play();
	}

	/// <summary>
	/// 钥匙插进门将门解锁
	/// </summary>
	public void PlayDoorUnlockedSound()
	{
		doorAudioSource.clip = unlockedClip;
		doorAudioSource.Play();
	}

	#endregion
}
