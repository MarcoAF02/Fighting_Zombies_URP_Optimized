using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 挂载在装备栏格子上，用于管理装备栏格子（单个格子）
/// </summary>
public class EquipSlot : MonoBehaviour
{
	#region 基本组件和变量

	[Header("该格子所对应的武器类型")]
	public WeaponTypeInHand weaponTypeInHand = WeaponTypeInHand.None;

	[Header("该格子的武器图标")]
	public GameObject itemIcon;

	[Header("该格子所持有的高亮背景")]
	public GameObject glowHighlight;

	#endregion
}
