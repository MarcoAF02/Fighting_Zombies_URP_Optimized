using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ڼ�¼������Ļ�ֱ���
/// </summary>
[Serializable]
public class ScreenResolutionRecordList
{
	[Header("��Ļ�ֱ��ʼ�¼�б�")]
	public List<ScreenResolutionRecord> screenResolutionRecordList = new List<ScreenResolutionRecord>();
}

/// <summary>
/// ��Ļ�ֱ��ʼ�¼�����ݽṹ
/// </summary>
[Serializable]
public class ScreenResolutionRecord
{
	public int settingIndex; // ���ñ��
	public int width; // ��Ļ�ĳ�
	public int height; // ��Ļ�ĸ�
}
