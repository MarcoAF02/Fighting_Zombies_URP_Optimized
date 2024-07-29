using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 对象池管理器
/// </summary>
public class PoolManager : MonoSingleton<PoolManager>
{
	#region 对象池主要功能

	/// <summary>
	/// 用键值对储存游戏物体名称和对象池数据
	/// </summary>
	public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();
	private GameObject poolObj;

	/// <summary>
	/// 往外拿东西，回调函数处理拿出对象时应该做什么
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public void GetObj(string name, UnityAction<GameObject> callBack)
	{
		//有抽屉 并且抽屉里有东西
		if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
		{
			callBack(poolDic[name].GetObj());
		}
	}

	/// <summary>
	/// 换暂时不用的东西给我
	/// </summary>
	public void PushObj(string name, GameObject obj)
	{
		if (poolObj == null) poolObj = new GameObject("Pool");

		if (poolDic.ContainsKey(name))
		{
			poolDic[name].PushObj(obj);
		}
		else
		{
			poolDic.Add(name, new PoolData(obj, poolObj));
		}
	}


	/// <summary>
	/// 清空缓存池的方法 
	/// 主要用在 场景切换时
	/// </summary>
	public void Clear()
	{
		poolDic.Clear();
		poolObj = null;
	}

	#endregion
}
