using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType // 记录物品的种类，玩家会根据拾取物品种类的不同做出不同响应
{
	None,
	Key, // 解谜关键物品
	Supply, // 物资补给
}

// 注意：游戏中的游戏物体不应该有任何实例化和删除 GameObject 的操作（性能优化）

/// <summary>
/// 可拾取物品控制器（应该挂载在可拾取物品的根物体上）
/// </summary>
public class PickableItem : MonoBehaviour
{
	#region 基本组件和变量

	[Header("指定该物品的类型")]
	public ItemType itemType = ItemType.None;

	[Header("物品 ID")]
	[Tooltip("这个会根据上面物品的类型放入对应的物品栏中")]
	public string itemID;

	[Header("拾取该物品时要不要改变游戏阶段")]
	public bool canChangeGameState;

	[Header("目标游戏阶段")]
	[Tooltip("如果这个物品拾取后会改变游戏阶段，指定目标游戏阶段")]
	public GameProgress targetGameProgress;

	[Header("是否要提示游戏目标信息")]
	[Tooltip("决定这个物品拾取后应不应该提示玩家目标改变")]
	public bool canNoticePlayer;
	[Header("提示文本隶属于什么提示阶段")]
	public GameTargetTipState gameTargetTipState;

	[Header("拾取物品时会不会显示说明信息")]
	public bool showTipMessage;

	[Header("如果显示提示信息，内容为")]
	public string tipMessage;

	[Header("如果这个物品是物资补给，补给的数量为")]
	public int supplies;

	#endregion

	#region 基本生命周期函数

	private void OnDisable()
	{
		if (canChangeGameState)
		{
			if (GameProgressManager.Instance != null)
			{
				GameProgressManager.Instance.PerformChangeGameProgress(targetGameProgress);
			}
		}
	}

	#endregion
}
