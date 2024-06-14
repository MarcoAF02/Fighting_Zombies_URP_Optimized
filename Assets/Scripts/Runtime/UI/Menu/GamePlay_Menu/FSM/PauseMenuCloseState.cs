using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuCloseState : PauseMenuBaseState
{
	public override void EnterState(PauseMenu pauseMenu, bool firstEnterState)
	{
		if (pauseMenu.playerController.playerLastState != null)
		{
			pauseMenu.playerController.SwitchState(pauseMenu.playerController.playerLastState); // 回到打开菜单之前的状态
		}

		pauseMenu.pauseMenuRoot.SetActive(false);
		AudioListener.pause = false;
		Time.timeScale = 1f;

		pauseMenu.closePauseMenuTotalTime = pauseMenu.closePauseMenuCDTime;

		try
		{
			if (!firstEnterState) MenuOperateSound.Instance.PlayMenuCloseSound_InGame(); // 播放菜单关闭音效
		}
		catch
		{
			Debug.LogWarning("场景中没有音效播放器，可能是因为本场景是点击进入的，而不是从主菜单加载进入的");
		}

		Debug.Log("关闭暂停菜单");
	}

	public override void OnUpdate(PauseMenu pauseMenu)
	{
		pauseMenu.closePauseMenuTotalTime -= Time.deltaTime;
		if (pauseMenu.closePauseMenuTotalTime <= 0f) pauseMenu.closePauseMenuTotalTime = 0f;

		pauseMenu.OpenPauseMenu(); // 开始菜单关闭后，可以检测打开逻辑
	}

}
