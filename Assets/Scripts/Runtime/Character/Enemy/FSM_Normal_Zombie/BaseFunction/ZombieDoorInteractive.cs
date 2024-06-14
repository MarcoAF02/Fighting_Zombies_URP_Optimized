using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ʬ���ŵĽ���
/// </summary>
public class ZombieDoorInteractive : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��ʬ������")]
	[SerializeField] private ZombieController zombieController;

	[Header("�Ž������߷����")]
	[SerializeField] private Transform doorInteractiveTrans;
	[Header("�����߼�ⳤ��")]
	[SerializeField] private float doorCheckDistance;

	[Header("��齩ʬ�������λ�õĲ���")]
	[SerializeField] private Transform doorCheckPoint;
	[SerializeField] private float doorCheckRadius;

	private Dictionary<string, float> distanceDic = new Dictionary<string, float>();

	#endregion

	#region ��ʬ���ŵĽ���

	/// <summary>
	/// ��ʬ���ŵĽ�������
	/// </summary>
	public void ZombieInteractiveWithDoor()
	{
		int checkLayerIndex = 1 << zombieController.layerAndTagCollection_Enemy.doorInteractiveLayerIndex; // ����ŵĲ㼶

		RaycastHit hitObj;

		if (Physics.Raycast(doorInteractiveTrans.position, doorInteractiveTrans.forward, out hitObj, doorCheckDistance, checkLayerIndex))
		{
			if (hitObj.collider.CompareTag(zombieController.layerAndTagCollection_Enemy.doorTag))
			{
				// Debug.Log("��ʬǰ������");

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
	/// ��齩ʬ���ŵ�ǰ�����Ǻ�
	/// </summary>
	private float CheckDoorDir()
	{
		// ע�⣺�ŵķ�λ����һ��Ҫ���� Door_In �� Door_Out Tag��Layer һ��Ҫ����Ϊ CheckDoorDir�����Ϊ 8��

		distanceDic.Clear();

		Collider[] doorCheckColliderArray = Physics.OverlapSphere(doorCheckPoint.position, doorCheckRadius, 1 << zombieController.layerAndTagCollection_Enemy.checkDoorDirLayerIndex);

		if (doorCheckColliderArray.Length != 2)
		{
			Debug.LogWarning("���û����ȷ��⵽�ŷ�λ�㣨���ǽ��������б���ţ����������Һ��ŵĲ���");
			return -2;
		}

		// ������� Tag �� ����ҵľ��� �������ֵ����ݽṹ��
		for (int i = 0; i < doorCheckColliderArray.Length; i++)
		{
			if (!distanceDic.ContainsKey(doorCheckColliderArray[i].tag))
			{
				distanceDic.Add(doorCheckColliderArray[i].tag, Vector3.Distance(doorCheckColliderArray[i].transform.position, transform.position));
			}
			else
			{
				Debug.LogWarning("�ż���� Tag δ������ȷ��������Ϸ����");
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
