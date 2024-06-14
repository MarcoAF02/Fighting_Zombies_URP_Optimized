using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于记录所有屏幕分辨率
/// </summary>
[Serializable]
public class ScreenResolutionRecordList
{
	[Header("屏幕分辨率记录列表")]
	public List<ScreenResolutionRecord> screenResolutionRecordList = new List<ScreenResolutionRecord>();
}

/// <summary>
/// 屏幕分辨率记录用数据结构
/// </summary>
[Serializable]
public class ScreenResolutionRecord
{
	public int settingIndex; // 设置编号
	public int width; // 屏幕的长
	public int height; // 屏幕的高
}
