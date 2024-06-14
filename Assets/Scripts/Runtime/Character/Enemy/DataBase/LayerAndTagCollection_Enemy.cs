using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LayerAndTagCollection_Enemy
{
	#region TAG ����

	public string concrete = "Concrete";
	public string concreteStaircase = "ConcreteStaircase";
	public string brick = "Brick";
	public string doorTag = "Door";

	#endregion

	#region Layer ��ż��ϣ�int ���ͣ�

	public int doorInteractiveLayerIndex = 11; // �ŵĽ��� Trigger
	public int ignoreNavMeshLayerIndex = 13; // ��ģ�͵� LayerIndex
	public int playerLayerIndex = 7; // ���
	public int checkDoorDirLayerIndex = 8; // ����ŵķ���
	public int airWallLayerIndex = 10; // ����ǽһ��

	#endregion
}
