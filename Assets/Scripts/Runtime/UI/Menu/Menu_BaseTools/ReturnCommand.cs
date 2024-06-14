using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 菜单工具类，记录需要显示和隐藏的 UI 物体
/// </summary>
[Serializable]
public class ReturnCommand
{
	public GameObject _hideObj;
	public GameObject _displayObj;
	public bool _saveSettingsData; // 菜单返回时，是否要保存设置数据
}
