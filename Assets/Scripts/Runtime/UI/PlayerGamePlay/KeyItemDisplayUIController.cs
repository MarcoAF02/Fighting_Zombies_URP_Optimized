using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 关键道具 UI 卡片控制器
/// </summary>
public class KeyItemDisplayUIController : MonoBehaviour
{
	#region 基本组件和变量

	[Header("物品 ID")]
	public string _itemID; // 这个不用显示在屏幕上
	[Header("物品描述文本")]
	public TextMeshProUGUI _itemDescribe;
	[Header("物品图标")]
	public Image _itemIcon;

	#endregion
}
