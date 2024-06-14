using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using StartFramework.GamePlay.BehaviourTree;

/// <summary>
/// 行为树执行者：将行为树代入 Unity 生命周期
/// </summary>
public class BT_Process : MonoBehaviour
{
	#region 基本组件和变量

	// 行为树的构建逻辑：用一个二位数组表示树，每个父数组表示树的每一层，每个子数组表示行为树的每个节点。
	// 创建行为树时，读取二维数组后广度优先搜索创建行为树。

	[Header("行为树构建数据")]
	public BT_CreateData bt_CreateData;

	// 行为树的构建目标
	public List<Node> nodes = new List<Node>();

	#endregion
}
