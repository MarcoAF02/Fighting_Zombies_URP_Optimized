using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LayerAndTagCollection_Enemy
{
	#region TAG 集合

	public string concrete = "Concrete";
	public string concreteStaircase = "ConcreteStaircase";
	public string brick = "Brick";
	public string doorTag = "Door";

	#endregion

	#region Layer 编号集合（int 类型）

	public int doorInteractiveLayerIndex = 11; // 门的交互 Trigger
	public int ignoreNavMeshLayerIndex = 13; // 门模型的 LayerIndex
	public int playerLayerIndex = 7; // 玩家
	public int checkDoorDirLayerIndex = 8; // 检测门的方向
	public int airWallLayerIndex = 10; // 空气墙一类

	#endregion
}
