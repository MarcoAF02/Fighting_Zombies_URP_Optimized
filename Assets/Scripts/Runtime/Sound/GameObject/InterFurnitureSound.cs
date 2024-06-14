using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 家具音效属于游戏音效
// 是 3D 音效

/// <summary>
/// 交互家具的音效（主要是开关门和推拉抽屉）
/// </summary>
public class InterFurnitureSound : MonoBehaviour
{
	#region 基本组件和变量

	[Header("家具音源组件")]
	[SerializeField] private AudioSource furnitureAudioSource;

	[Header("旋转门音频片段")]
	[Header("开门")]
	[SerializeField] private AudioClip rotateDoorOpenClip;
	[Header("关门")]
	[SerializeField] private AudioClip rotateDoorCloseClip;

	[Header("推拉抽屉音频片段")]
	[Header("开启")]
	[SerializeField] private AudioClip moveFurnOpenClip;
	[Header("关闭")]
	[SerializeField] private AudioClip moveFurnCloseClip;

	[Header("旋转门音量设置")]
	[SerializeField] private float rotateDoorVolume;
	[Header("推拉抽屉音量设置")]
	[SerializeField] private float moveFurnVolume;

	#endregion

	#region 家具互动音效播放

	// Close 时放 Open 音效，Open 时放 Close 音效

	/// <summary>
	/// 根据家具类型和家具状态播放对应音效
	/// </summary>
	/// <param name="_type"></param>
	/// <param name="_state"></param>
	public void PlayFurnSound(FurnitureType _type, FurnitureState _state)
	{
		if (_type == FurnitureType.RotateDoor)
		{
			if (_state == FurnitureState.Close)
			{
				furnitureAudioSource.volume = rotateDoorVolume;
				furnitureAudioSource.clip = rotateDoorOpenClip;
				furnitureAudioSource.Play();
			}

			if (_state == FurnitureState.Open)
			{
				furnitureAudioSource.volume = rotateDoorVolume;
				furnitureAudioSource.clip = rotateDoorCloseClip;
				furnitureAudioSource.Play();
			}
		}

		if (_type == FurnitureType.PushPull)
		{
			if (_state == FurnitureState.Close)
			{
				furnitureAudioSource.volume = moveFurnVolume;
				furnitureAudioSource.clip = moveFurnOpenClip;
				furnitureAudioSource.Play();
			}

			if (_state == FurnitureState.Open)
			{
				furnitureAudioSource.volume = moveFurnVolume;
				furnitureAudioSource.clip = moveFurnCloseClip;
				furnitureAudioSource.Play();
			}
		}
	}

	#endregion
}
