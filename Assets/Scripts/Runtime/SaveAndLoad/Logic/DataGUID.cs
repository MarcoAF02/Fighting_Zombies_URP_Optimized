using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// GUID 的意义：保证具体存档的唯一性，在字典里作为键的存在（取出存档数据的时候能对应上场景中的物品）

[ExecuteAlways]
public class DataGUID : MonoBehaviour
{
	public string guid;

	private void Awake()
	{
		if (guid == string.Empty)
		{
			guid = Guid.NewGuid().ToString(); // 生成一个新 GUID
		}
	}

}
