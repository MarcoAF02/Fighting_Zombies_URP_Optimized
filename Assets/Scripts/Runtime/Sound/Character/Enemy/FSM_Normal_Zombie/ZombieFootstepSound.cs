using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敌人脚步声属于游戏音效
// 属于 3D 音效

// 追击玩家，丢失玩家有一个独立的脚步声音量和脚步声间隔
// 巡逻状态又有一个独立的脚步声音量和脚步声间隔

public enum ZombieGroundMaterial
{
	Concrete, // 混凝土地板
	ConcreteStaircase, // 混凝土台阶
	Brick, // 石砖
}

public class ZombieFootstepSound : MonoBehaviour
{
	#region 基本组件和变量

	[Header("僵尸敌人主控制器")]
	[SerializeField] private ZombieController zombieController;

	[Header("脚步声音源组件")]
	[SerializeField] private AudioSource footstepAudioSource;
	[Header("僵尸倒地音源组件")]
	[SerializeField] private AudioSource fallDownAudioSource;

	[Header("角色地面检测功能")]
	[SerializeField] private Transform groundCheckPoint;
	[SerializeField] private float groundCheckRadius;

	[Header("不同状态脚步声的间隔时间")]
	[Header("巡逻时脚步声的间隔时间")]
	[SerializeField] private float patrolFootstepIntervalTime;
	private float patrolFootstepTotaltime;

	[Header("追击玩家时脚步声的间隔时间")]
	[SerializeField] private float pursuitFootstepIntervalTime;
	private float pursuitFootstepTotaltime;

	[Header("攻击类型 1 发出的脚步声次数")]
	[SerializeField] private int attack_1_footstepRate = 1;
	[Header("攻击类型 2 发出的脚步声次数")]
	[SerializeField] private int attack_2_footstepRate = 3;
	[Header("后退时发出的脚步声次数")]
	[SerializeField] private int moveback_footstepRate = 3;
	[Header("死亡时发出的脚步声次数")]
	[SerializeField] private int dead_footstepRate = 3;
	[Header("攻击类型 1 的脚步声间隔")]
	[SerializeField] private float attack_1_footstepRateTime = 1;
	[Header("对于后续攻击，攻击间隔时间的放大倍率（如果存在放大倍率的话）")]
	[SerializeField] private float attack_1_footstepRateTimeMag = 1;
	[Header("攻击类型 2 的脚步声间隔")]
	[SerializeField] private float attack_2_footstepRateTime = 1;
	[Header("对于后续攻击，攻击间隔时间的放大倍率（如果存在放大倍率的话）")]
	[SerializeField] private float attack_2_footstepRateTimeMag = 1;
	[Header("后退时的脚步声间隔")]
	[SerializeField] private float moveback_footstepRateTime = 1;
	[Header("后退时脚步声间隔的放大倍率（如果存在放大倍率的话）")]
	[SerializeField] private float moveback_footstepRateTimeMag = 0.8f;
	[Header("死亡时的脚步声间隔")]
	[SerializeField] private float dead_footstepRateTime;
	[Header("死亡时脚步声间隔的放大倍率（如果存在放大倍率的话）")]
	[SerializeField] private float dead_footstepRateTimeMag;

	// 脚步声音量：仅有追击玩家和巡逻的脚步声音量区别
	[Header("音量设置")]
	[Header("巡逻时的脚步声音量")]
	[SerializeField] private float patrolFootstepVolume;
	[Header("追击玩家时的脚步声音量")]
	[SerializeField] private float pursuitFootstepVolume;
	[Header("发动攻击时的脚步声音量")]
	[SerializeField] private float attackFootstepVolume;
	[Header("受伤硬直和死亡时的脚步声音量")]
	[SerializeField] private float deadAndHSFootstepVolume;
	[Header("僵尸倒地后死亡的音量大小")]
	[SerializeField] private float fallDownVolume;

	[Header("僵尸死亡后倒地的声音列表")]
	[SerializeField] private List<AudioClip> fallDownAudioClipList = new List<AudioClip>();
	[Header("混凝土地板音频列表")]
	[SerializeField] private List<AudioClip> footstepAudioList_Concrete = new List<AudioClip>();
	[Header("混凝土台阶音频列表")]
	[SerializeField] private List<AudioClip> footstepAudioList_ConcreteStaircase = new List<AudioClip>();

	private ZombieGroundMaterial zombieGroundMaterial = ZombieGroundMaterial.Concrete;

	// 使用中的次数和时间
	private int curAttackFootstepRate_1;
	private int curAttackFootstepRate_2;
	private int curMovebackFootstepRate;
	private int curDeadFootstepRate;
	private float curAttackFootstepRateTime_1;
	private float curAttackFootstepRateTime_2;
	private float curMovebackFootstepRateTime;
	private float curDeadFootstepRateTime;

	// 协程
	private Coroutine playAttack_1_Footstep_IECor;
	private Coroutine playAttack_2_Footstep_IECor;
	private Coroutine playHardStraightFootstepSound_IECor;
	private Coroutine playDeadFootstepSound_IECor;

	#endregion

	#region 地面类型检测

	/// <summary>
	/// 执行地面检测：脚步声播放功能
	/// </summary>
	public void GroundMaterialCheck()
	{
		RaycastHit hitObj;

		if (Physics.Raycast(groundCheckPoint.position, -groundCheckPoint.up, out hitObj, groundCheckRadius))
		{
			if (hitObj.collider.CompareTag(zombieController.layerAndTagCollection_Enemy.concrete))
			{
				zombieGroundMaterial = ZombieGroundMaterial.Concrete;
				return;
			}
			if (hitObj.collider.CompareTag(zombieController.layerAndTagCollection_Enemy.concreteStaircase))
			{
				zombieGroundMaterial = ZombieGroundMaterial.ConcreteStaircase;
				return;
			}
			if (hitObj.collider.CompareTag(zombieController.layerAndTagCollection_Enemy.brick))
			{
				zombieGroundMaterial = ZombieGroundMaterial.Brick;
				return;
			}
		}

		zombieGroundMaterial = ZombieGroundMaterial.Concrete;
	}

	#endregion

	#region 脚步声音效播放功能

	/// <summary>
	/// 巡逻时的脚步声
	/// </summary>
	public void PlayPatrolFootstepSound()
	{
		patrolFootstepTotaltime += Time.deltaTime;
		if (patrolFootstepTotaltime > 32768f) patrolFootstepTotaltime = 32768f;

		if (patrolFootstepTotaltime >  patrolFootstepIntervalTime)
		{
			patrolFootstepTotaltime = 0f;
			footstepAudioSource.volume = patrolFootstepVolume;
			PlaySingleFootstepSound();
		}
	}

	/// <summary>
	/// 追击玩家时的脚步声
	/// </summary>
	public void PlayPursuitFootstepSound()
	{
		pursuitFootstepTotaltime += Time.deltaTime;
		if (pursuitFootstepTotaltime > 32768f) pursuitFootstepTotaltime = 32768f;
		if (zombieController.zombieBattle.isAttacking) return;
		if (!zombieController.zombieBattle.continuePursuit) return;

		if (pursuitFootstepTotaltime > pursuitFootstepIntervalTime)
		{
			pursuitFootstepTotaltime = 0f;
			footstepAudioSource.volume = pursuitFootstepVolume;
			PlaySingleFootstepSound();
		}
	}

	/// <summary>
	/// 播放受伤硬直的脚步声（进入硬直状态的瞬间播放）
	/// </summary>
	public void PlayHardStraightFootstepSound()
	{
		footstepAudioSource.volume = deadAndHSFootstepVolume;
		playHardStraightFootstepSound_IECor = StartCoroutine(PlayHardStraightFootstepSound_IE());
	}

	private IEnumerator PlayHardStraightFootstepSound_IE()
	{
		curMovebackFootstepRate = moveback_footstepRate;
		curMovebackFootstepRateTime = moveback_footstepRateTime;
		
		while (true)
		{
			yield return new WaitForSeconds(curMovebackFootstepRateTime);

			PlaySingleFootstepSound();

			curMovebackFootstepRateTime *= moveback_footstepRateTimeMag;
			curMovebackFootstepRate--;

			if (curMovebackFootstepRate <= 0) yield break;
		}
	}

	/// <summary>
	/// 播放死亡时的脚步声
	/// </summary>
	public void PlayDeadFootstepSound()
	{
		footstepAudioSource.volume = deadAndHSFootstepVolume;
		playDeadFootstepSound_IECor = StartCoroutine(PlayDeadFootstepSound_IE());
	}

	private IEnumerator PlayDeadFootstepSound_IE()
	{
		curDeadFootstepRate = dead_footstepRate;
		curDeadFootstepRateTime = dead_footstepRateTime;

		while (true)
		{
			yield return new WaitForSeconds(curDeadFootstepRateTime);

			PlaySingleFootstepSound();

			curDeadFootstepRateTime *= dead_footstepRateTimeMag;
			curDeadFootstepRate--;

			if (curDeadFootstepRate <= 0)
			{
				PlayFallDownSound();
				yield break;
			}
		}
	}

	/// <summary>
	/// 播放发动攻击时的脚步声
	/// </summary>
	public void PlayAttackFootstep(int attackIndex)
	{
		footstepAudioSource.volume = attackFootstepVolume;

		if (attackIndex == 1)
		{
			if (playAttack_2_Footstep_IECor != null)
			{
				StopCoroutine(playAttack_2_Footstep_IECor);
			}

			playAttack_1_Footstep_IECor = StartCoroutine(PlayAttack_1_Footstep_IE());
		}

		if (attackIndex == 2)
		{
			if (playAttack_1_Footstep_IECor != null)
			{
				StopCoroutine(playAttack_1_Footstep_IECor);
			}

			playAttack_2_Footstep_IECor = StartCoroutine(PlayAttack_2_Footstep_IE());
		}
	}

	// 播放攻击行为 1 的脚步声
	private IEnumerator PlayAttack_1_Footstep_IE()
	{
		curAttackFootstepRate_1 = attack_1_footstepRate;
		curAttackFootstepRateTime_1 = attack_1_footstepRateTime;

		while (true)
		{
			yield return new WaitForSeconds(curAttackFootstepRateTime_1);

			PlaySingleFootstepSound();

			curAttackFootstepRateTime_1 *= attack_1_footstepRateTimeMag;
			curAttackFootstepRate_1--;

			if (curAttackFootstepRate_1 <= 0) yield break;
		}
	}
	
	// 播放攻击行为 2 的脚步声
	private IEnumerator PlayAttack_2_Footstep_IE()
	{
		curAttackFootstepRate_2 = attack_2_footstepRate;
		curAttackFootstepRateTime_2 = attack_2_footstepRateTime;

		while (true)
		{
			yield return new WaitForSeconds(curAttackFootstepRateTime_2);

			PlaySingleFootstepSound();

			curAttackFootstepRateTime_2 *= attack_2_footstepRateTimeMag;
			curAttackFootstepRate_2--;

			if (curAttackFootstepRate_2 <= 0) yield break;
		}
	}

	/// <summary>
	/// 单独播放一次脚步声
	/// </summary>
	public void PlaySingleFootstepSound()
	{
		// TODO: 添加更多材质的脚步声

		if (zombieGroundMaterial == ZombieGroundMaterial.Concrete)
		{
			int randomIndex = UnityEngine.Random.Range(0, footstepAudioList_Concrete.Count);
			footstepAudioSource.clip = footstepAudioList_Concrete[randomIndex];
		}
		if (zombieGroundMaterial == ZombieGroundMaterial.ConcreteStaircase)
		{
			int randomIndex = UnityEngine.Random.Range(0, footstepAudioList_ConcreteStaircase.Count);
			footstepAudioSource.clip = footstepAudioList_ConcreteStaircase[randomIndex];
		}

		footstepAudioSource.Play();
	}

	/// <summary>
	/// 僵尸死亡播放倒地音效
	/// </summary>
	private void PlayFallDownSound()
	{
		fallDownAudioSource.volume = fallDownVolume;

		int randomIndex = UnityEngine.Random.Range(0, fallDownAudioClipList.Count);
		fallDownAudioSource.clip = fallDownAudioClipList[randomIndex];

		fallDownAudioSource.Play();
	}

	#endregion

}
