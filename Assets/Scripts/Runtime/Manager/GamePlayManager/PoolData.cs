using System.Collections.Generic;
using UnityEngine;

public interface IPoolData
{
	void clearObjData();
}

public class PoolData
{
	public GameObject fatherObj; // 目标物体用的父节点
	public List<GameObject> poolList; // 对象池列表
	public IPoolData poolData;

	/// <summary>
	/// 对象池类构造函数
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
	/// 向对象池中送入游戏物体
	/// </summary>
	/// <param name="obj"></param>
	public void PushObj(GameObject obj)
	{
		poolData.clearObjData(); // 清除游戏对象的数据

		obj.SetActive(false);
		poolList.Add(obj);
		obj.transform.parent = fatherObj.transform;
	}

	/// <summary>
	/// 从对象池中拿出游戏物体
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
