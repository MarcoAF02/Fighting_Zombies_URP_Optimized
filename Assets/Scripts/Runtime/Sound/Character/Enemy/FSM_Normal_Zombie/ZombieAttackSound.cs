using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 僵尸攻击的声音管理器，会结合吼叫声，脚步声一起使用
// 属于游戏音效，属于 3D 音效

public class ZombieAttackSound : MonoBehaviour
{
	#region 基本组件和变量

	// 攻击挥砍音频是同一系列音频资源，区分的办法是音调高低

	[Header("僵尸控制器")]
	[SerializeField] private ZombieController zombieController;

	[Header("僵尸攻击音源组件")]
	[SerializeField] private AudioSource attackAudioSource;

	[Header("不同武器攻击音频的音量")]
	[Header("斧头挥砍")]
	[Range(0f, 1f)]
	[SerializeField] private float axeAttackVolume;
	[Header("钩子挥砍")]
	[Range(0f, 1f)]
	[SerializeField] private float hangerAttackVolume;
	[Header("小刀挥砍")]
	[Range(0f, 1f)]
	[SerializeField] private float kinfeAttackVolume;

	[Header("不同攻击音频的音调")]
	[Header("斧头挥砍")]
	[Range(0f, 3f)]
	[SerializeField] private float axeAttackPitch;
	[Header("钩子挥砍")]
	[Range(0f, 3f)]
	[SerializeField] private float hangerAttackPitch;
	[Header("小刀挥砍")]
	[Range(0f, 3f)]
	[SerializeField] private float kinfeAttackPitch;

	[Header("攻击挥砍音频列表")]
	[SerializeField] private List<AudioClip> attackAudioClipList = new List<AudioClip>();

	#endregion

	#region 攻击音效播放功能

	/// <summary>
	/// 播放攻击音效
	/// </summary>
	public void PlayAttackSound()
	{
		if (zombieController.zombieBattle.zombieWeaponType == ZombieWeaponType.None)
		{
			Debug.LogWarning("目前还没有设计徒手攻击的僵尸，徒手攻击音效不播放");
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
