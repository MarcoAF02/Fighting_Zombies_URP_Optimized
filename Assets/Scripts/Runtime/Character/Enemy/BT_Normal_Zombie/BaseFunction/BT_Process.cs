using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using StartFramework.GamePlay.BehaviourTree;

/// <summary>
/// ��Ϊ��ִ���ߣ�����Ϊ������ Unity ��������
/// </summary>
public class BT_Process : MonoBehaviour
{
	#region ��������ͱ���

	// ��Ϊ���Ĺ����߼�����һ����λ�����ʾ����ÿ���������ʾ����ÿһ�㣬ÿ���������ʾ��Ϊ����ÿ���ڵ㡣
	// ������Ϊ��ʱ����ȡ��ά���������������������Ϊ����

	[Header("��Ϊ����������")]
	public BT_CreateData bt_CreateData;

	// ��Ϊ���Ĺ���Ŀ��
	public List<Node> nodes = new List<Node>();

	#endregion
}
