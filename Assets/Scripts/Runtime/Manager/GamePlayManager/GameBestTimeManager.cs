using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ģʽ�ؿ�������ʱ�����
/// </summary>
public class GameBestTimeManager : MonoSingleton<GameBestTimeManager>, GamePlayISaveable
{
	#region ��������ͱ���

	[Header("��ʱ��λ���ı��������л����ԣ�")]
	[SerializeField] private string _useMinute = "��";
	[SerializeField] private string _useSecond = "��";

	[Header("���йؿ�����ɼ�¼")]
	public List<GamePlaySaveDataSingle> currentLevelPlayData = new List<GamePlaySaveDataSingle>();

	[Header("��ǰ�ؿ�����һ��ѵ�ʱ��")]
	public float playerUseTime;

	// �浵ϵͳ
	public string GUID => GetComponent<DataGUID>().guid;

	#endregion

	#region �����������ں���

	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		SaveLoadManager.Instance.RegisterGamePlaySaveable(this);
		SaveLoadManager.Instance.LoadGamePlayData(); // ��Ϸ��ʼʱ��һ�δ浵
	}

	#endregion

	#region ��Ϸʱ���¼����

	/// <summary>
	/// ͨ�������ʱ�䣬��¼���ؿ��������ʱ
	/// </summary>
	/// <param name="_curSceneName"></param>
	/// <param name="_time"></param>
	/// <param name="_minute"></param>
	/// <param name="_second"></param>
	public void RecordGameUseTime(string _curSceneName, float _time, out int _minute, out int _second)
	{
		playerUseTime = _time;

		// ת��ʱ�䵥λ
		TimeSpan timeSpan = new TimeSpan(0, 0, (int)playerUseTime);
		_minute = timeSpan.Minutes;
		_second = timeSpan.Seconds;

		RecordCurrentLevelData(_curSceneName, _minute, _second);
	}

	/// <summary>
	/// �õ���ǰ�ؿ����ʱ����ַ������������λ�ģ�
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

	// ����ǰ�ؿ������ʱ��
	private void RecordCurrentLevelData(string _curSceneName, float _playerUseMinute, float _playerUseSecond)
	{
		GamePlaySaveDataSingle newLevelData = new GamePlaySaveDataSingle()
		{
			sceneName = _curSceneName,
			useMinute = _playerUseMinute,
			useSecond = _playerUseSecond,
		};

		if (currentLevelPlayData.Count == 0) // ������ȫû�м�¼�����
		{
			currentLevelPlayData.Add(newLevelData); // ֱ�Ӽ�¼����
		}
		else // �����м�¼�����ǲ�ȷ���ǲ��Ǳ��ؿ������
		{
			for (int i = 0; i < currentLevelPlayData.Count; i ++)
			{
				if (_curSceneName == currentLevelPlayData[i].sceneName) // �ҵ��˱��ؿ��ļ�¼
				{
					// �Ա��ĸ�ʱ���С
					float lastTime = currentLevelPlayData[i].useMinute * 60f + currentLevelPlayData[i].useSecond;
					float currentTime = _playerUseMinute * 60f + _playerUseSecond;

					if (currentTime < lastTime) // ��¼ʱ�䲢�޸�
					{
						currentLevelPlayData[i].useMinute = _playerUseMinute;
						currentLevelPlayData[i].useSecond = _playerUseSecond;
					}

					SaveLoadManager.Instance.SaveGamePlayData(); // �浵
					return;
				}
			}

			currentLevelPlayData.Add(newLevelData); // û���ҵ����صļ�¼��ֱ�����
			SaveLoadManager.Instance.SaveGamePlayData(); // �浵
		}
	}

	#endregion

	#region �浵ϵͳ�ӿ�ʵ��

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
