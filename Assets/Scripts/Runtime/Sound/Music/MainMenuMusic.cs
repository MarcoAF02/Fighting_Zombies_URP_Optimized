using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 主菜单音乐控制器
/// </summary>
public class MainMenuMusic : MonoBehaviour
{
	#region 基本组件和变量

	[Header("音乐音源组件")]
	[SerializeField] private AudioSource mainMenuBGMSource;

	#endregion

	#region 基本生命周期函数

	private void Start()
	{
		mainMenuBGMSource.ignoreListenerPause = true;
		mainMenuBGMSource.Play();
	}

	#endregion
}
