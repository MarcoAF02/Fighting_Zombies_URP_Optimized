using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 装备栏控制器，用于管理装备栏的 UI
/// </summary>
public class EquipSlotController : MonoBehaviour
{
	#region 基本组件和变量

	[Header("玩家角色控制器")]
	[SerializeField] private PlayerController playerController;
	[Header("切换物品时快捷装备栏显示的时长")]
	[SerializeField] private float equipItemUIDisplayTime;
	[Header("快捷装备栏根物体")]
	[SerializeField] private GameObject equipItemUI;
	[Header("快捷装备栏的格子列表")]
	[SerializeField] private List<EquipSlot> equipSlotList = new List<EquipSlot>();

	// 协程
	private Coroutine displayEquipItemUI_IECor;
	private Coroutine displayEquipItemUI_GetNewItem_IECor;

	#endregion

	#region 基本生命周期函数

	private void OnEnable()
	{
		playerController.eventHandler_Player.ChangeEquipWeaponEvent += DisplayEquipItemUI;
		playerController.eventHandler_Player.GetNewItemOnEquipUIEvent += DisplayEquipItemUI_GetNewItem;
	}

	private void OnDisable()
	{
		playerController.eventHandler_Player.ChangeEquipWeaponEvent -= DisplayEquipItemUI;
		playerController.eventHandler_Player.GetNewItemOnEquipUIEvent -= DisplayEquipItemUI_GetNewItem;
	}

	#endregion

	#region 显示和隐藏装备栏的功能

	/// <summary>
	/// 切换武器时，显示物品栏和武器对应的高亮
	/// </summary>
	/// <param name="_showEquipItem"></param>
	/// <param name="_hideWeaponIcon"></param>
	/// <param name="_weaponTypeInHand"></param>
	/// <param name="_currentCount"></param>
	/// <param name="_totalCount"></param>
	public void DisplayEquipItemUI(bool _showEquipItem, bool _hideWeaponIcon, WeaponTypeInHand _weaponTypeInHand, int _currentCount, int _totalCount)
	{
		if (_showEquipItem)
		{
			// 先停止装备栏的显示
			if (displayEquipItemUI_IECor != null) StopCoroutine(displayEquipItemUI_IECor);
			if (displayEquipItemUI_GetNewItem_IECor != null) StopCoroutine(displayEquipItemUI_GetNewItem_IECor);

			for (int i = 0; i < equipSlotList.Count; i++)
			{
				equipSlotList[i].glowHighlight.SetActive(false);
			}

			displayEquipItemUI_IECor = StartCoroutine(DisplayEquipItemUI_IE(_weaponTypeInHand, _hideWeaponIcon));
		}
	}

	private IEnumerator DisplayEquipItemUI_IE(WeaponTypeInHand _weaponTypeInHand, bool _hideWeaponIcon)
	{
		equipItemUI.SetActive(true);

		for (int i = 0; i < equipSlotList.Count; i ++)
		{
			if (equipSlotList[i].weaponTypeInHand == _weaponTypeInHand)
			{
				if (_hideWeaponIcon)
				{
					equipSlotList[i].itemIcon.SetActive(false);
					equipSlotList[i].glowHighlight.SetActive(false);
				}
				else
				{
					equipSlotList[i].itemIcon.SetActive(true);
					equipSlotList[i].glowHighlight.SetActive(true);
				}
			}
		}

		yield return new WaitForSeconds(equipItemUIDisplayTime);

		for (int i = 0; i < equipSlotList.Count; i ++)
		{
			equipSlotList[i].glowHighlight.SetActive(false);
		}

		equipItemUI.SetActive(false);
	}

	/// <summary>
	/// 如果一样物品是重新获得的（从无到有），重新显示一遍装备栏 UI，并且不改变高亮的位置
	/// 只显示装备栏，不显示弹药栏
	/// </summary>
	/// <param name="_weaponTypeInHand"></param>
	public void DisplayEquipItemUI_GetNewItem(WeaponTypeInHand _weaponTypeInHand)
	{
		// 先停止装备栏的显示
		if (displayEquipItemUI_IECor != null) StopCoroutine(displayEquipItemUI_IECor);
		if (displayEquipItemUI_GetNewItem_IECor != null) StopCoroutine(displayEquipItemUI_GetNewItem_IECor);

		for (int i = 0; i < equipSlotList.Count; i++)
		{
			equipSlotList[i].glowHighlight.SetActive(false);
		}

		// 用另一个协程专门显示装备栏
		displayEquipItemUI_GetNewItem_IECor = StartCoroutine(DisplayEquipItemUI_GetNewItem_IE(_weaponTypeInHand));
	}

	private IEnumerator DisplayEquipItemUI_GetNewItem_IE(WeaponTypeInHand _weaponTypeInHand)
	{
		equipItemUI.SetActive(true);

		for (int i = 0; i < equipSlotList.Count; i++)
		{
			if (equipSlotList[i].weaponTypeInHand == _weaponTypeInHand)
			{
				equipSlotList[i].itemIcon.SetActive(true);
			}
		}

		// 高亮是正常显示的，无论什么时候都要让玩家知道自己装备了什么
		for (int i = 0; i < equipSlotList.Count; i++)
		{
			if (equipSlotList[i].weaponTypeInHand == playerController.weaponManager.weaponTypeInHand)
			{
				equipSlotList[i].glowHighlight.SetActive(true);
			}
		}

		yield return new WaitForSeconds(equipItemUIDisplayTime);

		for (int i = 0; i < equipSlotList.Count; i++)
		{
			equipSlotList[i].glowHighlight.SetActive(false);
		}

		equipItemUI.SetActive(false);
	}

	#endregion
}
