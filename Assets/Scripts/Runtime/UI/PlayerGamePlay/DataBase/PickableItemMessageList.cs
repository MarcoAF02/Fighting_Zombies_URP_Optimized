using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 记录物品栏物品信息的列表
/// </summary>
[Serializable]
public class PickableItemMessageList
{
	public List<PickableItemMessage> pickableItemMessageList;
}

/// <summary>
/// 可以拾取物品信息
/// </summary>
[Serializable]
public class PickableItemMessage
{
	public string _itemID; // 物品 ID
	public string _itemDescribe; // 物品描述
	public Sprite _itemIcon; // 物品图标
}
