using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 负责管理场景加载数据
/// </summary>
[Serializable]
public class SceneLoadDataList
{
	[Header("过渡场景的名称")]
	public string _sceneLoaderName;
	[Header("经典模式下可游玩场景的名称")]
	public List<SceneLoadData> _sceneLoadDataList = new List<SceneLoadData>();
}

[Serializable]
public class SceneLoadData
{
	[Header("场景名称")]
	public string sceneName;
	[Header("场景描述文本")]
	public string sceneDescribeText;
	[Header("场景加载时的背景图片")]
	public Sprite sceneBGSprite;
}
