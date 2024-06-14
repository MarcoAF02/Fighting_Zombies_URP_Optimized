using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 经典模式关卡最佳完成时间管理
/// </summary>
public class GameBestTimeManager : MonoSingleton<GameBestTimeManager>, GamePlayISaveable
{
	#region 基本组件和变量

	[Header("计时单位的文本（可以切换语言）")]
	[SerializeField] private string _useMinute = "分";
	[SerializeField] private string _useSecond = "秒";

	[Header("所有关卡的完成记录")]
	public List<GamePlaySaveDataSingle> currentLevelPlayData = new List<GamePlaySaveDataSingle>();

	[Header("当前关卡中玩家花费的时间")]
	public float playerUseTime;

	// 存档系统
	public string GUID => GetComponent<DataGUID>().guid;

	#endregion

	#region 基本生命周期函数

	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		SaveLoadManager.Instance.RegisterGamePlaySaveable(this);
		SaveLoadManager.Instance.LoadGamePlayData(); // 游戏开始时读一次存档
	}

	#endregion

	#region 游戏时间记录功能

	/// <summary>
	/// 通过传入的时间，记录本关卡的完成用时
	/// </summary>
	/// <param name="_curSceneName"></param>
	/// <param name="_time"></param>
	/// <param name="_minute"></param>
	/// <param name="_second"></param>
	public void RecordGameUseTime(string _curSceneName, float _time, out int _minute, out int _second)
	{
		playerUseTime = _time;

		// 转换时间单位
		TimeSpan timeSpan = new TimeSpan(0, 0, (int)playerUseTime);
		_minute = timeSpan.Minutes;
		_second = timeSpan.Seconds;

		RecordCurrentLevelData(_curSceneName, _minute, _second);
	}

	/// <summary>
	/// 得到当前关卡完成时间的字符串（处理过单位的）
	/// </summary>
	/// <returns></returns>
	public string GetGameUseTimeString()
	{
		int _minute = 0;
		int _second = 0;

		TimeSpan timeSpan = new TimeSpan(0, 0, (int)playerUseTime);
		_minute = timeSpan.Minutes;
		_second = timeSpan.Seconds;

		string _timeText = _minute + " " + _useMinute + " " + _second + " " + _useSecond;

		return _timeText;
	}

	// 处理当前关卡的完成时间
	private void RecordCurrentLevelData(string _curSceneName, float _playerUseMinute, float _playerUseSecond)
	{
		GamePlaySaveDataSingle newLevelData = new GamePlaySaveDataSingle()
		{
			sceneName = _curSceneName,
			useMinute = _playerUseMinute,
			useSecond = _playerUseSecond,
		};

		if (currentLevelPlayData.Count == 0) // 对于完全没有记录的情况
		{
			currentLevelPlayData.Add(newLevelData); // 直接记录即可
		}
		else // 对于有记录，但是不确定是不是本关卡的情况
		{
			for (int i = 0; i < currentLevelPlayData.Count; i ++)
			{
				if (_curSceneName == currentLevelPlayData[i].sceneName) // 找到了本关卡的记录
				{
					// 对比哪个时间更小
					float lastTime = currentLevelPlayData[i].useMinute * 60f + currentLevelPlayData[i].useSecond;
					float currentTime = _playerUseMinute * 60f + _playerUseSecond;

					if (currentTime < lastTime) // 记录时间并修改
					{
						currentLevelPlayData[i].useMinute = _playerUseMinute;
						currentLevelPlayData[i].useSecond = _playerUseSecond;
					}

					SaveLoadManager.Instance.SaveGamePlayData(); // 存档
					return;
				}
			}

			currentLevelPlayData.Add(newLevelData); // 没有找到本关的记录，直接添加
			SaveLoadManager.Instance.SaveGamePlayData(); // 存档
		}
	}

	#endregion

	#region 存档系统接口实现

	public GamePlaySaveDataList SaveGamePlayInToData()
	{
		GamePlaySaveDataList saveList = new GamePlaySaveDataList();
		saveList.gamePlaySaveDataList = new List<GamePlaySaveDataSingle>();

		for (int i = 0; i < currentLevelPlayData.Count; i ++)
		{
			saveList.gamePlaySaveDataList.Add(currentLevelPlayData[i]);
		}

		return saveList;
	}

	public void LoadGamePlaySaveData(GamePlaySaveDataList saveData)
	{
		currentLevelPlayData.Clear();

		for (int i = 0; i < saveData.gamePlaySaveDataList.Count; i ++)
		{
			currentLevelPlayData.Add(saveData.gamePlaySaveDataList[i]);
		}
	}

	#endregion
}
