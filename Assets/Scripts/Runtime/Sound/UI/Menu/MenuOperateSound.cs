using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 主菜单音效属于系统音效
// 系统音效不属于 3D 音效

/// <summary>
/// 主菜单音效管理
/// </summary>
public class MenuOperateSound : MonoSingleton<MenuOperateSound>
{
	#region 基本组件和变量

	// 因为这些音频是固定的，所以不做任何随机取样

	[Header("确认音源组件")]
	[SerializeField] private AudioSource checkAudioSource;
	[Header("取消音源组件")]
	[SerializeField] private AudioSource cancelAudioSource;
	[Header("鼠标滑动选择音源组件")]
	[SerializeField] private AudioSource mouseUpAudioSource;

	[Header("游戏内菜单打开音效")]
	[SerializeField] private AudioSource openMenuAudioSourceInGame;
	[Header("游戏内菜单关闭音效")]
	[SerializeField] private AudioSource closeMenuAudioSourceInGame;

	#endregion

	#region 基本生命周期函数

	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject); // 这个音源组件在游戏菜单内也会使用
	}

	private void Start()
	{
		// 使音频忽略 TimeScale
		checkAudioSource.ignoreListenerPause = true;
		cancelAudioSource.ignoreListenerPause = true;
		mouseUpAudioSource.ignoreListenerPause = true;
		openMenuAudioSourceInGame.ignoreListenerPause = true;
		closeMenuAudioSourceInGame.ignoreListenerPause = true;
	}

	#endregion

	#region UI 选择音效

	/// <summary>
	/// 播放按钮确认的音效
	/// </summary>
	public void PlayCheckSound()
	{
		checkAudioSource.Play();
	}

	/// <summary>
	/// 播放按钮取消的音效
	/// </summary>
	public void PlayCancelSound()
	{
		cancelAudioSource.Play();
	}
	
	/// <summary>
	/// 播放鼠标悬浮在按钮上的音效
	/// </summary>
	public void PlayMouseUpSound()
	{
		mouseUpAudioSource.Play();
	}

	#endregion

	#region 游戏内暂停菜单打开和关闭音效

	/// <summary>
	/// 播放游戏内菜单打开时的音效
	/// </summary>
	public void PlayMenuOpenSound_InGame()
	{
		openMenuAudioSourceInGame.Play();
	}

	/// <summary>
	/// 播放游戏内菜单关闭时的音效
	/// </summary>
	public void PlayMenuCloseSound_InGame()
	{
		closeMenuAudioSourceInGame.Play();
	}

	#endregion

}
