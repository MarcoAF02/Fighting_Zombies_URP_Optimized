using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCountBGController : MonoBehaviour
{
	// 物品数量显示的规则：
	// 如果没有数量信息，关闭所有文字和背景
	// 如果没有后备弹药信息，只显示独立数量文字
	// 如果有后备弹药信息，显示当前弹药数和后备弹药数

	#region 基本组件和变量

	[Header("玩家角色控制器")]
	[SerializeField] private PlayerController playerController;

	[Header("背景图片")]
	[SerializeField] private GameObject imageBG;

	[Header("当前弹药数的文本（显示后备弹药数）")]
	[SerializeField] private TextMeshProUGUI currentCountText;
	[Header("后备弹药数的文本（显示后备弹药数）")]
	[SerializeField] private TextMeshProUGUI totalCountText;
	[Header("当前弹药数的文本（仅显示当前弹药数）")]
	[SerializeField] private TextMeshProUGUI onlyCurrentCountText;

	#endregion

	#region 基本生命周期函数

	private void OnEnable()
	{
		playerController.eventHandler_Player.ChangeEquipWeaponEvent += UpdateItemCountText;
	}

	private void OnDisable()
	{
		playerController.eventHandler_Player.ChangeEquipWeaponEvent -= UpdateItemCountText;
	}

	#endregion

	#region 显示剩余弹药数文本

	/// <summary>
	/// 根据切换到的武器类型显示剩余弹药数
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
			Debug.Log("当前手上没有武器");
			return;
		}

		if (_weaponTypeInHand == WeaponTypeInHand.PrimaryWeapon)
		{
			// TODO: 未来的更新：添加更多类型的武器
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
