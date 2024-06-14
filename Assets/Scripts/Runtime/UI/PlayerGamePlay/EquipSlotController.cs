using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// װ���������������ڹ���װ������ UI
/// </summary>
public class EquipSlotController : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��ҽ�ɫ������")]
	[SerializeField] private PlayerController playerController;
	[Header("�л���Ʒʱ���װ������ʾ��ʱ��")]
	[SerializeField] private float equipItemUIDisplayTime;
	[Header("���װ����������")]
	[SerializeField] private GameObject equipItemUI;
	[Header("���װ�����ĸ����б�")]
	[SerializeField] private List<EquipSlot> equipSlotList = new List<EquipSlot>();

	// Э��
	private Coroutine displayEquipItemUI_IECor;
	private Coroutine displayEquipItemUI_GetNewItem_IECor;

	#endregion

	#region �����������ں���

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

	#region ��ʾ������װ�����Ĺ���

	/// <summary>
	/// �л�����ʱ����ʾ��Ʒ����������Ӧ�ĸ���
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
			// ��ֹͣװ��������ʾ
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
	/// ���һ����Ʒ�����»�õģ����޵��У���������ʾһ��װ���� UI�����Ҳ��ı������λ��
	/// ֻ��ʾװ����������ʾ��ҩ��
	/// </summary>
	/// <param name="_weaponTypeInHand"></param>
	public void DisplayEquipItemUI_GetNewItem(WeaponTypeInHand _weaponTypeInHand)
	{
		// ��ֹͣװ��������ʾ
		if (displayEquipItemUI_IECor != null) StopCoroutine(displayEquipItemUI_IECor);
		if (displayEquipItemUI_GetNewItem_IECor != null) StopCoroutine(displayEquipItemUI_GetNewItem_IECor);

		for (int i = 0; i < equipSlotList.Count; i++)
		{
			equipSlotList[i].glowHighlight.SetActive(false);
		}

		// ����һ��Э��ר����ʾװ����
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

		// ������������ʾ�ģ�����ʲôʱ��Ҫ�����֪���Լ�װ����ʲô
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
