using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// �����������������
/// </summary>
[Serializable]
public class SceneLoadDataList
{
	[Header("���ɳ���������")]
	public string _sceneLoaderName;
	[Header("����ģʽ�¿����泡��������")]
	public List<SceneLoadData> _sceneLoadDataList = new List<SceneLoadData>();
}

[Serializable]
public class SceneLoadData
{
	[Header("��������")]
	public string sceneName;
	[Header("���������ı�")]
	public string sceneDescribeText;
	[Header("��������ʱ�ı���ͼƬ")]
	public Sprite sceneBGSprite;
}
