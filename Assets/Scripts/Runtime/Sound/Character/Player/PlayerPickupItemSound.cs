using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 拾取物品时的提示声
/// </summary>
public class PlayerPickupItemSound : MonoBehaviour
{
	#region 基本组件和变量

	[Header("拾取物品的音源组件")]
	[SerializeField] private AudioSource pickupAudioSource;
	[Header("拾取到钥匙的音效")]
	[SerializeField] private AudioClip pickupKeyAudioClip;
	[Header("拾取到手枪弹药的音效")]
	[SerializeField] private AudioClip pickupPistolAmmoClip;
	[Header("拾取到医疗包的音效")]
	[SerializeField] private AudioClip pickupMedicineAmmoClip;

	#endregion

	#region 拾取音效播放

	public void PlayPickupSound(ItemType _itemType, string _itemID)
	{
		if (_itemType == ItemType.None)
		{
			Debug.LogWarning("拾取的物品类型没有定义，请检查游戏设计");
			return;
		}

		if (_itemType == ItemType.Key)
		{
			pickupAudioSource.clip = pickupKeyAudioClip;
			pickupAudioSource.Play();
		}

		if (_itemType == ItemType.Supply)
		{
			// 判断补给品是什么
			if (_itemID == "Pistol_Ammo")
			{
				pickupAudioSource.clip = pickupPistolAmmoClip;
				pickupAudioSource.Play();
			}

			if (_itemID == "Medicine")
			{
				pickupAudioSource.clip = pickupMedicineAmmoClip;
				pickupAudioSource.Play();
			}
		}
	}

	#endregion
}
