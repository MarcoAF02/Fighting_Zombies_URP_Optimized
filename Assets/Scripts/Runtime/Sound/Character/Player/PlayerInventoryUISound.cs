using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 玩家 UI 音效属于系统音效
// 不属于 3D 音效

public class PlayerInventoryUISound : MonoBehaviour
{
	#region 基本组件和变量

	[Header("物品栏交互音源组件")]
	[SerializeField] private AudioSource inventoryAudioSource;

	[Header("打开物品栏的音频片段")]
	[SerializeField] private AudioClip openInventoryAudioClip;
	[Header("关闭物品栏的音频片段")]
	[SerializeField] private AudioClip closeInventoryAudioClip;

	[Header("打开和关闭物品栏的音量")]
	[SerializeField] private float inventoryAudioVolume;

	#endregion

	#region 音频片段的播放和关闭

	/// <summary>
	/// 播放打开物品栏的音效
	/// </summary>
	public void PlayOpenInventorySound()
	{
		inventoryAudioSource.volume = inventoryAudioVolume;
		inventoryAudioSource.clip = openInventoryAudioClip;

		inventoryAudioSource.Play();
	}

	/// <summary>
	/// 播放关闭物品栏的音效
	/// </summary>
	public void PlayCloseInventorySound()
	{
		inventoryAudioSource.volume = inventoryAudioVolume;
		inventoryAudioSource.clip = closeInventoryAudioClip;

		inventoryAudioSource.Play();
	}

	#endregion
}
