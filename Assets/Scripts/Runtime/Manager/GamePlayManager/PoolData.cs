using System.Collections.Generic;
using UnityEngine;

public interface IPoolData
{
	void clearObjData();
}

public class PoolData
{
	public GameObject fatherObj; // Ŀ�������õĸ��ڵ�
	public List<GameObject> poolList; // ������б�
	public IPoolData poolData;

	/// <summary>
	/// ������๹�캯��
	/// </summary>
	/// <param name="obj"></param>
	/// <param name="poolObj"></param>
	public PoolData(GameObject obj, GameObject poolObj)
	{
		fatherObj = new GameObject(obj.name);
		fatherObj.transform.parent = poolObj.transform;
		poolList = new List<GameObject>() { };
		PushObj(obj);
	}

	/// <summary>
	/// ��������������Ϸ����
	/// </summary>
	/// <param name="obj"></param>
	public void PushObj(GameObject obj)
	{
		poolData.clearObjData(); // �����Ϸ���������

		obj.SetActive(false);
		poolList.Add(obj);
		obj.transform.parent = fatherObj.transform;
	}

	/// <summary>
	/// �Ӷ�������ó���Ϸ����
	/// </summary>
	/// <returns></returns>
	public GameObject GetObj()
	{
		GameObject obj = null;

		obj = poolList[0];
		poolList.RemoveAt(0);
		obj.SetActive(true);
		obj.transform.parent = null;

		return obj;
	}

}
