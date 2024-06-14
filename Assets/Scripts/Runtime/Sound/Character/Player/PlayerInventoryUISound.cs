using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��� UI ��Ч����ϵͳ��Ч
// ������ 3D ��Ч

public class PlayerInventoryUISound : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��Ʒ��������Դ���")]
	[SerializeField] private AudioSource inventoryAudioSource;

	[Header("����Ʒ������ƵƬ��")]
	[SerializeField] private AudioClip openInventoryAudioClip;
	[Header("�ر���Ʒ������ƵƬ��")]
	[SerializeField] private AudioClip closeInventoryAudioClip;

	[Header("�򿪺͹ر���Ʒ��������")]
	[SerializeField] private float inventoryAudioVolume;

	#endregion

	#region ��ƵƬ�εĲ��ź͹ر�

	/// <summary>
	/// ���Ŵ���Ʒ������Ч
	/// </summary>
	public void PlayOpenInventorySound()
	{
		inventoryAudioSource.volume = inventoryAudioVolume;
		inventoryAudioSource.clip = openInventoryAudioClip;

		inventoryAudioSource.Play();
	}

	/// <summary>
	/// ���Źر���Ʒ������Ч
	/// </summary>
	public void PlayCloseInventorySound()
	{
		inventoryAudioSource.volume = inventoryAudioVolume;
		inventoryAudioSource.clip = closeInventoryAudioClip;

		inventoryAudioSource.Play();
	}

	#endregion
}
