using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ����Ϸ���̹������йص�ʱ���趨
/// </summary>
[Serializable]
public class ProgressTimer
{
	[Header("�ı���Ϸ״̬���ӳ�ʱ�䣨�룩")]
	[Tooltip("һ��ֻ����Ϸ�տ�ʼʱ����")]
	[Range(0.1f, 2f)]
	public float startChangeProgressTime;
}
