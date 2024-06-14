using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 环境管理器
/// </summary>
public class EnvironmentManager : MonoBehaviour
{
	// 根据游戏进度的推进改变氛围灯等环境物件

	#region 基本组件和变量

	[Header("游戏进度事件发布器")]
	[SerializeField] private EventHandler_GameManager eventHandler_GameManager;

	[Header("环境灯光")]
	[SerializeField] private List<Light> envirLightList = new List<Light>();

	[Header("环境灯光在游戏前期的颜色")]
	[SerializeField] private Color earlyStageLightColor;

	[Header("环境灯光在游戏后期的颜色")]
	[SerializeField] private Color laterStageLightColor;

	#endregion

	#region 基本生命周期函数

	private void Awake()
	{
		eventHandler_GameManager = GameObject.FindGameObjectWithTag("EventHandler_GameManager").GetComponent<EventHandler_GameManager>();
	}

	private void OnEnable()
	{
		eventHandler_GameManager.ChangeGameProgressEvent += ChangeEnvirLightColor; // 订阅事件
	}

	private void OnDisable()
	{
		eventHandler_GameManager.ChangeGameProgressEvent -= ChangeEnvirLightColor; // 取消订阅事件
	}

	#endregion

	/// <summary>
	/// 根据游戏进程改变氛围灯的颜色
	/// </summary>
	/// <param name="_gameProgress"></param>
	private void ChangeEnvirLightColor(GameProgress _gameProgress)
	{
		if (_gameProgress == GameProgress.None)
		{
			Debug.LogWarning("游戏进度为 None，请检查游戏设计的正确性");
			return;
		}

		if (_gameProgress == GameProgress.EarlyStage)
		{
			for (int i = 0; i < envirLightList.Count; i ++)
			{
				try
				{
					envirLightList[i].color = earlyStageLightColor;
				}
				catch
				{
					Debug.Log("游戏已经结束，无需操作环境灯");
				}
			}
		}

		if (_gameProgress == GameProgress.LaterStage)
		{
			for (int i = 0; i < envirLightList.Count; i ++)
			{
				try
				{
					envirLightList[i].color = laterStageLightColor;
				}
				catch
				{
					Debug.Log("游戏已经结束，无需操作环境灯");
				}
			}
		}
	}

}
