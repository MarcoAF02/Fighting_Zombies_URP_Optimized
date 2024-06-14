using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �����Ϸ��ʱ����������Ϸ��ʼ�����ڻ��ѵ�ʱ��
/// </summary>
public class PlayerTimeManager : MonoBehaviour
{
	#region ��������ͱ���

	private float playerUseTime; // ��¼��Ϸ��ʼ�����ڵ�ʱ��

	#endregion

	#region �����������ں���

	private void Start()
	{
		playerUseTime = 0f;
	}

	private void Update()
	{
		RecordGamePlayTime();
	}

	#endregion

	#region ��Ϸ��ʱ����

	private void RecordGamePlayTime()
	{
		if (GameProgressManager.Instance.CurrentGameProgress == GameProgress.EarlyStage ||
			GameProgressManager.Instance.CurrentGameProgress == GameProgress.LaterStage)
		{
			playerUseTime += Time.deltaTime;
		}
	}

	/// <summary>
	/// �ϴ�������浱ǰ�ؿ����ѵ�ʱ��
	/// </summary>
	/// <returns></returns>
	public void GetGamePlayTotalTime() // out ���ñ��븳ֵ...
	{
		Scene curScene = SceneManager.GetActiveScene();
		string curSceneName = curScene.name;

		GameBestTimeManager.Instance.RecordGameUseTime(curSceneName, playerUseTime, out int _useMinute, out int _useSecond);
	}

	#endregion

}
