using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 和游戏进程管理器有关的时间设定
/// </summary>
[Serializable]
public class ProgressTimer
{
	[Header("改变游戏状态的延迟时间（秒）")]
	[Tooltip("一般只有游戏刚开始时会用")]
	[Range(0.1f, 2f)]
	public float startChangeProgressTime;
}
