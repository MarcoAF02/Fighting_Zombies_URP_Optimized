using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 玩家游戏计时器，计算游戏开始至现在花费的时间
/// </summary>
public class PlayerTimeManager : MonoBehaviour
{
	#region 基本组件和变量

	private float playerUseTime; // 记录游戏开始到现在的时间

	#endregion

	#region 基本生命周期函数

	private void Start()
	{
		playerUseTime = 0f;
	}

	private void Update()
	{
		RecordGamePlayTime();
	}

	#endregion

	#region 游戏计时功能

	private void RecordGamePlayTime()
	{
		if (GameProgressManager.Instance.CurrentGameProgress == GameProgress.EarlyStage ||
			GameProgressManager.Instance.CurrentGameProgress == GameProgress.LaterStage)
		{
			playerUseTime += Time.deltaTime;
		}
	}

	/// <summary>
	/// 上传玩家游玩当前关卡花费的时间
	/// </summary>
	/// <returns></returns>
	public void GetGamePlayTotalTime() // out 引用必须赋值...
	{
		Scene curScene = SceneManager.GetActiveScene();
		string curSceneName = curScene.name;

		GameBestTimeManager.Instance.RecordGameUseTime(curSceneName, playerUseTime, out int _useMinute, out int _useSecond);
	}

	#endregion

}
