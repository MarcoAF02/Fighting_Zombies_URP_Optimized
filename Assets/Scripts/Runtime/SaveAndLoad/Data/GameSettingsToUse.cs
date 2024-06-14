using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSettingsToUse
{
	#region 设置使用记录

	public string _settingName; // 储存本设置的名称（好让用户自行修改）
	public int _settingListIndex; // 这个数字用于寻找设置列表中对应的选项

	#endregion
}

[System.Serializable]
public class GameSettingsToUseList
{
	#region 设置储存记录

	// 储存所有设置信息的列表
	public List<GameSettingsToUse> gameSettingsSaveList;

	#endregion
}
