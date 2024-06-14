using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 僵尸和门的交互
/// </summary>
public class ZombieDoorInteractive : MonoBehaviour
{
	#region 基本组件和变量

	[Header("僵尸控制器")]
	[SerializeField] private ZombieController zombieController;

	[Header("门交互射线发射点")]
	[SerializeField] private Transform doorInteractiveTrans;
	[Header("门射线检测长度")]
	[SerializeField] private float doorCheckDistance;

	[Header("检查僵尸和门相对位置的参数")]
	[SerializeField] private Transform doorCheckPoint;
	[SerializeField] private float doorCheckRadius;

	private Dictionary<string, float> distanceDic = new Dictionary<string, float>();

	#endregion

	#region 僵尸和门的交互

	/// <summary>
	/// 僵尸和门的交互功能
	/// </summary>
	public void ZombieInteractiveWithDoor()
	{
		int checkLayerIndex = 1 << zombieController.layerAndTagCollection_Enemy.doorInteractiveLayerIndex; // 检测门的层级

		RaycastHit hitObj;

		if (Physics.Raycast(doorInteractiveTrans.position, doorInteractiveTrans.forward, out hitObj, doorCheckDistance, checkLayerIndex))
		{
			if (hitObj.collider.CompareTag(zombieController.layerAndTagCollection_Enemy.doorTag))
			{
				// Debug.Log("僵尸前方有门");

				MainDoorController mainDoorController = hitObj.collider.GetComponentInParent<MainDoorController>();

				if (mainDoorController != null)
				{
					if (mainDoorController.doorState != DoorState.Close) return;
					if (mainDoorController.locked) return;

					mainDoorController.SetCheckDirCollider(true);
					float checkZombiePos = CheckDoorDir();
					mainDoorController.SetCheckDirCollider(false);

					mainDoorController.InteractiveWithDoor(checkZombiePos);
				}
			}
		}
	}

	/// <summary>
	/// 检查僵尸在门的前方还是后方
	/// </summary>
	private float CheckDoorDir()
	{
		// 注意：门的方位检查点一定要打上 Door_In 或 Door_Out Tag，Layer 一定要设置为 CheckDoorDir（编号为 8）

		distanceDic.Clear();

		Collider[] doorCheckColliderArray = Physics.OverlapSphere(doorCheckPoint.position, doorCheckRadius, 1 << zombieController.layerAndTagCollection_Enemy.checkDoorDirLayerIndex);

		if (doorCheckColliderArray.Length != 2)
		{
			Debug.LogWarning("玩家没有正确检测到门方位点（或是近距离内有别的门）。请调整玩家和门的参数");
			return -2;
		}

		// 将检查点的 Tag 和 与玩家的距离 储存在字典数据结构中
		for (int i = 0; i < doorCheckColliderArray.Length; i++)
		{
			if (!distanceDic.ContainsKey(doorCheckColliderArray[i].tag))
			{
				distanceDic.Add(doorCheckColliderArray[i].tag, Vector3.Distance(doorCheckColliderArray[i].transform.position, transform.position));
			}
			else
			{
				Debug.LogWarning("门检查点的 Tag 未设置正确，请检查游戏设置");
				return -2;
			}
		}

		if (distanceDic["Door_In"] < distanceDic["Door_Out"])
		{
			return 2;
		}
		else
		{
			return -2;
		}
	}

	#endregion
}
