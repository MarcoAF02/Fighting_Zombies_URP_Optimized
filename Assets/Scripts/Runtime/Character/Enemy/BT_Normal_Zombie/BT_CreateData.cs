using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BT_CreateData
{
	[Header("行为树所有层（横着看）")]
	public List<BT_CreateDataNode> dataNodes = new List<BT_CreateDataNode>();
}

[Serializable]
public class BT_CreateDataNode
{
	[Header("树这一层每个节点的信息")]
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
