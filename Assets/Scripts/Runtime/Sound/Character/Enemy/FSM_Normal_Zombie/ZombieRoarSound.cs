using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敌人吼叫声属于游戏音效
// 属于 3D 音效

public class ZombieRoarSound : MonoBehaviour
{
	#region 基本组件和变量

	[Header("音效播放音源组件")]
	[SerializeField] private AudioSource roarAudioSource;

	[Header("空闲站桩时的吼叫声大小")]
	[SerializeField] private float idleRoarAudioVolume;
	[Header("空闲巡逻时的吼叫声大小")]
	[SerializeField] private float patrolRoarAudioVolume;
	[Header("发现玩家瞬间的吼叫声大小")]
	[SerializeField] private float shockRoarAudioVolume;
	[Header("追击玩家时的吼叫声大小")]
	[SerializeField] private float pursuitRoarAudioVolume;
	[Header("发动攻击时的吼叫声大小")]
	[SerializeField] private float attackRoarAudioVolume;
	[Header("受伤进入硬直的吼叫声大小")]
	[SerializeField] private float hardStraightRoarAudioVolume;
	[Header("死亡发出的吼叫声大小")]
	[SerializeField] private float deadRoarAudioVolume;

	[Header("站桩吼叫音效的间隔时间")]
	[SerializeField] private float idleSoundIntervalTime;
	private float idleSoundTotalTime;
	[Header("巡逻吼叫音效的间隔时间")]
	[SerializeField] private float patrolSoundIntervalTime;
	private float patrolSoundTotalTime;
	[Header("追击玩家时的吼叫声时间间隔")]
	[SerializeField] private float pursuitSoundIntervalTime;
	private float pursuitSoundTotalTime;

	[Header("发现玩家的瞬间吼叫声要持续多久")]
	[SerializeField] private float shockAudioSustainTime = 1f;
	[HideInInspector] public bool isShocked = false; // 是否吼叫过了

	[Header("空闲时音效列表")]
	[SerializeField] private List<AudioClip> idleAudioList = new List<AudioClip>();
	[Header("空闲巡逻时音效列表")]
	[SerializeField] private List<AudioClip> patrolAudioList = new List<AudioClip>();
	[Header("发现玩家瞬间的吼叫声列表")]
	[SerializeField] private List<AudioClip> shockAudioList = new List<AudioClip>();
	[Header("追击玩家时的吼叫声列表")]
	[SerializeField] private List<AudioClip> pursuitAudioList = new List<AudioClip>();
	[Header("发动攻击时的吼叫声列表")]
	[SerializeField] private List<AudioClip> attackAudioList = new List<AudioClip>();
	[Header("受伤进入硬直的吼叫声音效列表")]
	[SerializeField] private List<AudioClip> hardStraightAudioList = new List<AudioClip>();
	[Header("死亡时的吼叫声音效列表")]
	[SerializeField] private List<AudioClip> deadAudioList = new List<AudioClip>();

	// 协程
	private Coroutine playShockRoarSound_IECor;

	#endregion

	#region 基本生命周期函数

	private void Start()
	{
		idleSoundTotalTime = idleSoundIntervalTime;
		patrolSoundTotalTime = patrolSoundIntervalTime;
	}

	#endregion

	#region 音效播放功能

	/// <summary>
	/// 僵尸站桩低吼音效播放
	/// </summary>
	public void PlayIdleRoarSound()
	{
		idleSoundTotalTime += Time.deltaTime;
		if (idleSoundTotalTime > 32768f) idleSoundTotalTime = 32768f;

		roarAudioSource.volume = idleRoarAudioVolume;

		if (idleSoundTotalTime > idleSoundIntervalTime)
		{
			idleSoundTotalTime = 0f;
			int randomIndex = UnityEngine.Random.Range(0, idleAudioList.Count);
			roarAudioSource.clip = idleAudioList[randomIndex];
			roarAudioSource.Play();
		}
	}

	/// <summary>
	/// 僵尸巡逻低吼音效播放
	/// </summary>
	public void PlayPatrolRoarSound()
	{
		patrolSoundTotalTime += Time.deltaTime;
		if (patrolSoundTotalTime > 32768f) patrolSoundTotalTime = 32768f;

		roarAudioSource.volume = patrolRoarAudioVolume;

		if (patrolSoundTotalTime > patrolSoundIntervalTime)
		{
			patrolSoundTotalTime = 0f;
			int randomIndex = UnityEngine.Random.Range(0, patrolAudioList.Count);
			roarAudioSource.clip = patrolAudioList[randomIndex];
			roarAudioSource.Play();
		}
	}

	/// <summary>
	/// 僵尸发现玩家瞬间的吼叫声
	/// </summary>
	public void PlayShockRoarSound()
	{
		if (isShocked) return;
		if (!gameObject.activeInHierarchy) return;

		playShockRoarSound_IECor = StartCoroutine(PlayShockRoarSound_IE());
	}

	private IEnumerator PlayShockRoarSound_IE()
	{
		roarAudioSource.volume = shockRoarAudioVolume;

		int randomIndex = UnityEngine.Random.Range(0, shockAudioList.Count);
		roarAudioSource.clip = shockAudioList[randomIndex];
		roarAudioSource.Play();

		yield return new WaitForSeconds(shockAudioSustainTime);

		isShocked = true;
	}

	/// <summary>
	/// 僵尸追击玩家时的吼叫声
	/// </summary>
	public void PlayPursuitRoarSound()
	{
		pursuitSoundTotalTime += Time.deltaTime;
		if (pursuitSoundTotalTime > pursuitSoundIntervalTime) pursuitSoundTotalTime = 32768f;
		roarAudioSource.volume = pursuitRoarAudioVolume;

		if (pursuitSoundTotalTime > pursuitSoundIntervalTime)
		{
			pursuitSoundTotalTime = 0f;

			int randomIndex = Random.Range(0, pursuitAudioList.Count);
			roarAudioSource.clip = pursuitAudioList[randomIndex];
			roarAudioSource.Play();
		}
	}

	/// <summary>
	/// 僵尸发动攻击时的吼叫声
	/// </summary>
	public void PlayAttackRoarSound()
	{
		roarAudioSource.clip = null; // 先清空别的音效
		roarAudioSource.volume = attackRoarAudioVolume;

		int randomIndex = UnityEngine.Random.Range(0, attackAudioList.Count);
		roarAudioSource.clip = attackAudioList[randomIndex];
		roarAudioSource.Play();
	}

	/// <summary>
	/// 僵尸受伤进入硬直状态发出吼叫声
	/// </summary>
	public void PlayHardStraightRoarSound()
	{
		roarAudioSource.clip = null; // 先清空别的音效
		roarAudioSource.volume = hardStraightRoarAudioVolume;

		int randomIndex = UnityEngine.Random.Range(0, hardStraightAudioList.Count);
		roarAudioSource.clip = hardStraightAudioList[randomIndex];
		roarAudioSource.Play();
	}

	/// <summary>
	/// 僵尸死亡时发出吼叫声
	/// </summary>
	public void PlayDeadRoarSound()
	{
		roarAudioSource.clip = null; // 先清空别的音效
		roarAudioSource.volume = deadRoarAudioVolume;

		int randomIndex = UnityEngine.Random.Range(0, deadAudioList.Count);
		roarAudioSource.clip = deadAudioList[randomIndex];
		roarAudioSource.Play();
	}

	#endregion

}
