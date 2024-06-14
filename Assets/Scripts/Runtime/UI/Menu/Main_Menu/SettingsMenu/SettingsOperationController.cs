using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 操作设置页面的控制脚本
/// </summary>
public class SettingsOperationController : MonoBehaviour
{
	#region 基本组件和变量

	[Header("左右两个按钮")]
	[SerializeField] private Button addButton;
	[SerializeField] private Button reduceButton;

	[Header("设置描述文字")]
	[SerializeField] private TextMeshProUGUI settingsDescribe;
	[Header("这个设置叫什么名字，用于 SettingsLoader 查找并应用于游戏")]
	[SerializeField] private string thisSettingsName;

	// 选项设置列表的含义：一个数值对应一种 UI 文本，仅用于表现
	[Header("设置选项列表")]
	[SerializeField] private List<SettingsMessage> thisSettingsList = new List<SettingsMessage>();
	private int index = 0; // 用于操作选项设置

	#endregion

	#region 基本生命周期函数

	private void Start()
	{
		// 拉取 SettingsLoader 的数据
		try
		{
			PullDataFromSettingsLoader();
		}
		catch
		{
			Debug.LogWarning("场景中没有设置加载器，可能是因为该场景是手动点击进入的");
			return;
		}

		if (thisSettingsList.Count > 0)
		{
			settingsDescribe.text = thisSettingsList[index]._settingMessage; // 读取描述文字
		}
		else
		{
			Debug.LogWarning("该选项没有设置好信息或存档数据有误");

			index = 0;
			settingsDescribe.text = thisSettingsList[index]._settingMessage; // 读取描述文字
		}

		addButton.onClick.AddListener(AddSettingsValue);
		reduceButton.onClick.AddListener(ReduceSettingsValue);
	}

	#endregion

	#region 设置操作功能（按钮点击事件）

	// 增加设置的值
	private void AddSettingsValue()
	{
		if (index < thisSettingsList.Count - 1 && index >= 0)
		{
			index++;
			settingsDescribe.text = thisSettingsList[index]._settingMessage; // 读取描述文字

			// DebugSettingsMessage();
		}

		MenuOperateSound.Instance.PlayCheckSound();
		SyncWithSettingsLoader();
	}

	// 减少设置的值
	private void ReduceSettingsValue()
	{
		if (index < thisSettingsList.Count && index > 0)
		{
			index--;
			settingsDescribe.text = thisSettingsList[index]._settingMessage; // 读取描述文字

			// DebugSettingsMessage();
		}

		MenuOperateSound.Instance.PlayCheckSound();
		SyncWithSettingsLoader();
	}

	#endregion

	#region 和 SettingsLoader 通信

	/// <summary>
	/// 从 SettingsLoader 拉取数据
	/// </summary>
	public void PullDataFromSettingsLoader()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i ++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == thisSettingsName)
			{
				index = SettingsLoader.Instance.gameSettingsList[i]._settingListIndex;
			}
		}
	}

	/// <summary>
	/// 将设置和 SettingsLoader 同步
	/// </summary>
	public void SyncWithSettingsLoader()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == thisSettingsName)
			{
				SettingsLoader.Instance.gameSettingsList[i]._settingListIndex = index;
			}
		}
	}

	#endregion

	#region DEBUG 用函数

	private void DebugSettingsMessage()
	{
		Debug.Log(index + " " + thisSettingsList[index]._settingIndex + " " +  thisSettingsList[index]._settingMessage);
	}

	#endregion

}
