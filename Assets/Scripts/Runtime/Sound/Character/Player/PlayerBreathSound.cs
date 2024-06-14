using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家喘息声音效控制
/// </summary>
public class PlayerBreathSound : MonoBehaviour
{
	#region 基本组件和变量

	[Header("玩家的嘴")]
	[SerializeField] private AudioSource playerMouth;

	[Header("走路时的喘气音量")]
	[SerializeField] private float walkBreathVolume;
	[Header("跑步时的喘气音量")]
	[SerializeField] private float runBreathVolume;
	[Header("瞄准时的喘气音量")]
	[SerializeField] private float aimingBreathVolume;
	[Header("攻击时吼叫的音量")]
	[SerializeField] private float attackRoarVolume;
	[Header("受伤时吼叫的音量")]
	[SerializeField] private float getHurtRoarVolume;
	[Header("死亡时吼叫的音量")]
	[SerializeField] private float deadRoarVolume;
	[Header("生命值恢复时放松喘气音量")]
	[SerializeField] private float healthRecoverVolume;

	[Header("走路喘气声间隔时间")]
	[SerializeField] private float walkBreathIntervalTime;
	private float walkBreathTotalTime;
	[Header("跑步喘气声间隔时间")]
	[SerializeField] private float runBreathIntervalTime;
	private float runBreathTotalTime;
	[Header("瞄准喘气声间隔时间")]
	[SerializeField] private float aimingBreathIntervalTime;
	private float aimingBreathTotalTime;

	[Header("玩家呼吸声的音频")]
	[SerializeField] private AudioClip breathAudioClip;
	[Header("攻击吼叫音频")]
	[SerializeField] private List<AudioClip> attackRoarClipList = new List<AudioClip>();

	[Header("受伤吼叫音频")]
	[SerializeField] private List<AudioClip> getHurtClipList = new List<AudioClip>();
	[Header("死亡吼叫音频")]
	[SerializeField] private List<AudioClip> deadClipList = new List<AudioClip>();
	[Header("生命值恢复时的放松音频")]
	[SerializeField] private List<AudioClip> healthRecoverClipList = new List<AudioClip>();

	#endregion

	#region 玩家移动呼吸声

	/// <summary>
	/// 走路时的玩家喘气声
	/// </summary>
	public void PlayWalkBreathSound()
	{
		walkBreathTotalTime += Time.deltaTime;
		if (walkBreathTotalTime > 32768f) walkBreathTotalTime = 32768f;

		if (walkBreathTotalTime > walkBreathIntervalTime)
		{
			walkBreathTotalTime = 0f;
			playerMouth.volume = walkBreathVolume;
			playerMouth.clip = breathAudioClip;
			playerMouth.Play();
		}
	}

	/// <summary>
	/// 跑步时的玩家喘气声
	/// </summary>
	public void PlayRunBreathSound()
	{
		runBreathTotalTime += Time.deltaTime;
		if (runBreathTotalTime > 32768f) runBreathTotalTime = 32768f;

		if (runBreathTotalTime > runBreathIntervalTime)
		{
			runBreathTotalTime = 0f;
			playerMouth.volume = runBreathVolume;
			playerMouth.clip = breathAudioClip;
			playerMouth.Play();
		}
	}

	/// <summary>
	/// 瞄准时的玩家喘气声
	/// </summary>
	public void PlayAimingBreathSound()
	{
		aimingBreathTotalTime += Time.deltaTime;
		if (aimingBreathTotalTime > 32768f) aimingBreathTotalTime = 32768f;

		if (aimingBreathTotalTime > aimingBreathIntervalTime)
		{
			aimingBreathTotalTime = 0f;
			playerMouth.volume = aimingBreathVolume;
			playerMouth.clip = breathAudioClip;
			playerMouth.Play();
		}
	}

	#endregion

	#region 玩家攻击吼叫声

	/// <summary>
	/// 玩家用近战武器攻击时的吼叫声
	/// </summary>
	public void PlayAttackRoarSound()
	{
		playerMouth.volume = attackRoarVolume;

		walkBreathTotalTime = 0f;
		runBreathTotalTime = 0f;

		int randomIndex = Random.Range(0, attackRoarClipList.Count);
		playerMouth.clip = attackRoarClipList[randomIndex];
		playerMouth.Play();
	}

	#endregion

	#region 玩家受伤和死亡吼叫声

	/// <summary>
	/// 播放玩家受伤吼叫音效
	/// </summary>
	public void PlayGetHurtRoarSound()
	{
		playerMouth.volume = getHurtRoarVolume;

		walkBreathTotalTime = 0f;
		runBreathTotalTime = 0f;

		int randomIndex = Random.Range(0, getHurtClipList.Count);
		playerMouth.clip = getHurtClipList[randomIndex];
		playerMouth.Play();
	}

	/// <summary>
	/// 播放玩家死亡吼叫音效
	/// </summary>
	public void PlayDeadRoarSound()
	{
		playerMouth.volume = deadRoarVolume;

		walkBreathTotalTime = 0f;
		runBreathTotalTime = 0f;

		int randomIndex = Random.Range(0, deadClipList.Count);
		playerMouth.clip = deadClipList[randomIndex];
		playerMouth.Play();
	}

	#endregion

	#region 生命值恢复的放松喘气声

	/// <summary>
	/// 生命值恢复的放松喘气声
	/// </summary>
	public void PlayRecoverBreathSound()
	{
		playerMouth.volume = healthRecoverVolume;

		walkBreathTotalTime = 0f;
		runBreathTotalTime = 0f;

		int randomIndex = Random.Range(0, healthRecoverClipList.Count);
		playerMouth.clip = healthRecoverClipList[randomIndex];
		playerMouth.Play();
	}

	#endregion

}
