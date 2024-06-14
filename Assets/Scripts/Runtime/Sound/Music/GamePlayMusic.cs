using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 背景音乐属于游戏音乐
// 背景音乐全局播放，不属于 3D 音效

/// <summary>
/// 背景音乐管理器
/// </summary>
public class GamePlayMusic : MonoBehaviour
{
	#region 基本组件和变量

	[Header("背景音乐音源组件")]
	[SerializeField] private AudioSource musicAudioSource;

	[Header("前期音乐音量")]
	[SerializeField] private float earlyBGMVolume;
	[Header("后期音乐音量")]
	[SerializeField] private float laterBGMVolume;
	[Header("游戏通关时的音乐音量")]
	[SerializeField] private float gameCompleteBGMVolume;
	[Header("游戏失败时的音乐音量")]
	[SerializeField] private float gameFailBGMVolume;

	[Header("前期背景音乐片段")]
	[SerializeField] private AudioClip earlyStageBGM;
	[Header("后期背景音乐片段")]
	[SerializeField] private AudioClip laterStageBGM;
	[Header("游戏通关时的音乐片段")]
	[SerializeField] private AudioClip gameCompleteBGM;
	[Header("游戏失败时的音乐片段")]
	[SerializeField] private AudioClip gameFailBGM;

	#endregion

	#region 基本生命周期函数

	private void Start()
	{
		GameProgressManager.Instance.eventHandler_GameManager.ChangeGameProgressEvent += ChangeAndPlayBGM;
	}

	private void OnDisable()
	{
		GameProgressManager.Instance.eventHandler_GameManager.ChangeGameProgressEvent -= ChangeAndPlayBGM;
	}

	#endregion

	#region 背景音乐的播放和切换功能

	// 随着游戏进度改变背景音乐
	private void ChangeAndPlayBGM(GameProgress gameProgress)
	{
		if (gameProgress == GameProgress.None)
		{
			Debug.LogWarning("游戏进度未定义，请检查游戏流程逻辑");
			return;
		}

		if (gameProgress == GameProgress.EarlyStage)
		{
			musicAudioSource.loop = true;
			musicAudioSource.volume = earlyBGMVolume;
			musicAudioSource.clip = earlyStageBGM;
			musicAudioSource.Play();
		}

		if (gameProgress == GameProgress.LaterStage)
		{
			musicAudioSource.loop = true;
			musicAudioSource.volume = laterBGMVolume;
			musicAudioSource.clip = laterStageBGM;
			musicAudioSource.Play();
		}

		if (gameProgress == GameProgress.GameComplete)
		{
			musicAudioSource.loop = true;
			musicAudioSource.volume = gameCompleteBGMVolume;
			musicAudioSource.clip = gameCompleteBGM;
			musicAudioSource.Play();
		}

		if (gameProgress == GameProgress.GameFail)
		{
			musicAudioSource.loop = false;
			musicAudioSource.volume = gameFailBGMVolume;
			musicAudioSource.clip = gameFailBGM;
			musicAudioSource.Play();
		}

	}

	#endregion
}
