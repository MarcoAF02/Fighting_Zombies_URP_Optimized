using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ����ع�����
/// </summary>
public class PoolManager : MonoSingleton<PoolManager>
{
	#region �������Ҫ����

	/// <summary>
	/// �ü�ֵ�Դ�����Ϸ�������ƺͶ��������
	/// </summary>
	public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();
	private GameObject poolObj;

	/// <summary>
	/// �����ö������ص����������ó�����ʱӦ����ʲô
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public void GetObj(string name, UnityAction<GameObject> callBack)
	{
		//�г��� ���ҳ������ж���
		if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
		{
			callBack(poolDic[name].GetObj());
		}
	}

	/// <summary>
	/// ����ʱ���õĶ�������
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
	/// ��ջ���صķ��� 
	/// ��Ҫ���� �����л�ʱ
	/// </summary>
	public void Clear()
	{
		poolDic.Clear();
		poolObj = null;
	}

	#endregion
}
