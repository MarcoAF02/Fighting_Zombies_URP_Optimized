using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BT_CreateData
{
	[Header("��Ϊ�����в㣨���ſ���")]
	public List<BT_CreateDataNode> dataNodes = new List<BT_CreateDataNode>();
}

[Serializable]
public class BT_CreateDataNode
{
	[Header("����һ��ÿ���ڵ����Ϣ")]
	public List<NodeState> node = new List<NodeState>();
}

[Serializable]
public enum NodeState
{
	If,
	While,
	Selector,
	Sequence,
	Leaf,
}
