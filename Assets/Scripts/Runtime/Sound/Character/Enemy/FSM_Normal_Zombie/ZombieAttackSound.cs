using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ʬ���������������������Ϻ�������Ų���һ��ʹ��
// ������Ϸ��Ч������ 3D ��Ч

public class ZombieAttackSound : MonoBehaviour
{
	#region ��������ͱ���

	// �����ӿ���Ƶ��ͬһϵ����Ƶ��Դ�����ֵİ취�������ߵ�

	[Header("��ʬ������")]
	[SerializeField] private ZombieController zombieController;

	[Header("��ʬ������Դ���")]
	[SerializeField] private AudioSource attackAudioSource;

	[Header("��ͬ����������Ƶ������")]
	[Header("��ͷ�ӿ�")]
	[Range(0f, 1f)]
	[SerializeField] private float axeAttackVolume;
	[Header("���ӻӿ�")]
	[Range(0f, 1f)]
	[SerializeField] private float hangerAttackVolume;
	[Header("С���ӿ�")]
	[Range(0f, 1f)]
	[SerializeField] private float kinfeAttackVolume;

	[Header("��ͬ������Ƶ������")]
	[Header("��ͷ�ӿ�")]
	[Range(0f, 3f)]
	[SerializeField] private float axeAttackPitch;
	[Header("���ӻӿ�")]
	[Range(0f, 3f)]
	[SerializeField] private float hangerAttackPitch;
	[Header("С���ӿ�")]
	[Range(0f, 3f)]
	[SerializeField] private float kinfeAttackPitch;

	[Header("�����ӿ���Ƶ�б�")]
	[SerializeField] private List<AudioClip> attackAudioClipList = new List<AudioClip>();

	#endregion

	#region ������Ч���Ź���

	/// <summary>
	/// ���Ź�����Ч
	/// </summary>
	public void PlayAttackSound()
	{
		if (zombieController.zombieBattle.zombieWeaponType == ZombieWeaponType.None)
		{
			Debug.LogWarning("Ŀǰ��û�����ͽ�ֹ����Ľ�ʬ��ͽ�ֹ�����Ч������");
			return;
		}

		if (zombieController.zombieBattle.zombieWeaponType == ZombieWeaponType.Axe)
		{
			attackAudioSource.pitch = axeAttackPitch;
			attackAudioSource.volume = axeAttackVolume;
		}

		if (zombieController.zombieBattle.zombieWeaponType == ZombieWeaponType.Kinfe)
		{
			attackAudioSource.pitch = kinfeAttackPitch;
			attackAudioSource.volume = kinfeAttackVolume;
		}

		if (zombieController.zombieBattle.zombieWeaponType == ZombieWeaponType.Hanger)
		{
			attackAudioSource.pitch = hangerAttackPitch;
			attackAudioSource.volume = hangerAttackVolume;
		}

		int randomIndex = UnityEngine.Random.Range(0, attackAudioClipList.Count);
		attackAudioSource.clip = attackAudioClipList[randomIndex];

		attackAudioSource.Play();
	}

	#endregion

}
