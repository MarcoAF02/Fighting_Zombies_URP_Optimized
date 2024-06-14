using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCountBGController : MonoBehaviour
{
	// ��Ʒ������ʾ�Ĺ���
	// ���û��������Ϣ���ر��������ֺͱ���
	// ���û�к󱸵�ҩ��Ϣ��ֻ��ʾ������������
	// ����к󱸵�ҩ��Ϣ����ʾ��ǰ��ҩ���ͺ󱸵�ҩ��

	#region ��������ͱ���

	[Header("��ҽ�ɫ������")]
	[SerializeField] private PlayerController playerController;

	[Header("����ͼƬ")]
	[SerializeField] private GameObject imageBG;

	[Header("��ǰ��ҩ�����ı�����ʾ�󱸵�ҩ����")]
	[SerializeField] private TextMeshProUGUI currentCountText;
	[Header("�󱸵�ҩ�����ı�����ʾ�󱸵�ҩ����")]
	[SerializeField] private TextMeshProUGUI totalCountText;
	[Header("��ǰ��ҩ�����ı�������ʾ��ǰ��ҩ����")]
	[SerializeField] private TextMeshProUGUI onlyCurrentCountText;

	#endregion

	#region �����������ں���

	private void OnEnable()
	{
		playerController.eventHandler_Player.ChangeEquipWeaponEvent += UpdateItemCountText;
	}

	private void OnDisable()
	{
		playerController.eventHandler_Player.ChangeEquipWeaponEvent -= UpdateItemCountText;
	}

	#endregion

	#region ��ʾʣ�൯ҩ���ı�

	/// <summary>
	/// �����л���������������ʾʣ�൯ҩ��
	/// </summary>
	/// <param name="_showEquipItem"></param>
	/// <param name="_hideWeaponIcon"></param>
	/// <param name="_weaponTypeInHand"></param>
	/// <param name="_currentCount"></param>
	/// <param name="_totalCount"></param>
	public void UpdateItemCountText(
	bool _showEquipItem,
	bool _hideWeaponIcon,
	WeaponTypeInHand _weaponTypeInHand,
	int _currentCount,
	int _totalCount)
	{
		if (_weaponTypeInHand == WeaponTypeInHand.None)
		{
			Debug.Log("��ǰ����û������");
			return;
		}

		if (_weaponTypeInHand == WeaponTypeInHand.PrimaryWeapon)
		{
			// TODO: δ���ĸ��£���Ӹ������͵�����
		}

		if (_weaponTypeInHand == WeaponTypeInHand.SecondaryWeapon)
		{
			imageBG.SetActive(true);

			currentCountText.text = _currentCount.ToString() + " /";
			totalCountText.text = _totalCount.ToString();

			onlyCurrentCountText.text = string.Empty;
		}

		if (_weaponTypeInHand == WeaponTypeInHand.MeleeWeapon)
		{
			imageBG.SetActive(false);

			currentCountText.text = string.Empty;
			totalCountText.text = string.Empty;

			onlyCurrentCountText.text = string.Empty;
		}

		if (_weaponTypeInHand == WeaponTypeInHand.Medicine)
		{
			imageBG.SetActive(true);

			currentCountText.text = string.Empty;
			totalCountText.text = string.Empty;

			onlyCurrentCountText.text = _currentCount.ToString();
		}
	}

	#endregion
}
