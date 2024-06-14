using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可拾取物品信息管理器
/// </summary>
public class PickableItemMessageManager : MonoSingleton<PickableItemMessageManager>
{
	#region 基本组件和变量

	[Header("物品信息列表（信息来源）")]
	[SerializeField] private PickableItemMessageList itemMessageList;

	[Header("物品信息来源（使用物品时读取）")]
	public List<PickableItemMessage> pickableItemMessageListToUse = new List<PickableItemMessage>();

	#endregion

	#region 基本生命周期函数

	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		CopyItemMessageToUse();
	}

	#endregion

	#region 信息复制功能

	private void CopyItemMessageToUse()
	{
		pickableItemMessageListToUse.Clear(); // 首先清空信息

		// 添加信息
		for (int i = 0; i < itemMessageList.pickableItemMessageList.Count; i ++)
		{
			pickableItemMessageListToUse.Add(itemMessageList.pickableItemMessageList[i]);
		}
	}

	#endregion
}
