using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsMessage
{
	public int _settingIndex; // 数字用于储存程序通信用设置信息
	public string _settingMessage; // 文字用于 UI 表现，为了让玩家看见
}
