using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ��¼��Ʒ����Ʒ��Ϣ���б�
/// </summary>
[Serializable]
public class PickableItemMessageList
{
	public List<PickableItemMessage> pickableItemMessageList;
}

/// <summary>
/// ����ʰȡ��Ʒ��Ϣ
/// </summary>
[Serializable]
public class PickableItemMessage
{
	public string _itemID; // ��Ʒ ID
	public string _itemDescribe; // ��Ʒ����
	public Sprite _itemIcon; // ��Ʒͼ��
}
