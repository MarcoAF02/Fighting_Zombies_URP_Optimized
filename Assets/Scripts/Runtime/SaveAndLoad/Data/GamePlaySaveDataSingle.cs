using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GamePlaySaveDataList
{
	#region 关卡完成记录列表

	public List<GamePlaySaveDataSingle> gamePlaySaveDataList = new List<GamePlaySaveDataSingle>();

	#endregion
}

[Serializable]
public class GamePlaySaveDataSingle
{
	#region 关卡完成记录（单个记录）

	// 关卡名称
	public string sceneName;

	// 关卡完成时间
	public float useMinute;
	public float useSecond;

	#endregion
}
