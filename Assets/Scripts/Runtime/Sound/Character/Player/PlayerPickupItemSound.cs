using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ʰȡ��Ʒʱ����ʾ��
/// </summary>
public class PlayerPickupItemSound : MonoBehaviour
{
	#region ��������ͱ���

	[Header("ʰȡ��Ʒ����Դ���")]
	[SerializeField] private AudioSource pickupAudioSource;
	[Header("ʰȡ��Կ�׵���Ч")]
	[SerializeField] private AudioClip pickupKeyAudioClip;
	[Header("ʰȡ����ǹ��ҩ����Ч")]
	[SerializeField] private AudioClip pickupPistolAmmoClip;
	[Header("ʰȡ��ҽ�ư�����Ч")]
	[SerializeField] private AudioClip pickupMedicineAmmoClip;

	#endregion

	#region ʰȡ��Ч����

	public void PlayPickupSound(ItemType _itemType, string _itemID)
	{
		if (_itemType == ItemType.None)
		{
			Debug.LogWarning("ʰȡ����Ʒ����û�ж��壬������Ϸ���");
			return;
		}

		if (_itemType == ItemType.Key)
		{
			pickupAudioSource.clip = pickupKeyAudioClip;
			pickupAudioSource.Play();
		}

		if (_itemType == ItemType.Supply)
		{
			// �жϲ���Ʒ��ʲô
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
