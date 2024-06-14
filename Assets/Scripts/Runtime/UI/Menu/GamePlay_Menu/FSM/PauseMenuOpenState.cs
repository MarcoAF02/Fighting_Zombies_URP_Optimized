using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuOpenState : PauseMenuBaseState
{
	public override void EnterState(PauseMenu pauseMenu, bool firstEnterState)
	{
		pauseMenu.playerController.SwitchState(pauseMenu.playerController.gamePauseState);
		pauseMenu.pauseMenuRoot.SetActive(true);
		AudioListener.pause = true;
		Time.timeScale = 0f;

		pauseMenu.openPauseMenuTotalTime = pauseMenu.openPauseMenuCDTime;
		pauseMenu.CalculateOpenPauseMenuCDTime();

		// 刚进入暂停菜单，先增加返回逻辑（虽然这个返回指令永远不会使用）
		ReturnCommand newCommand = new ReturnCommand()
		{
			_hideObj = null,
			_displayObj = null,
			_saveSettingsData = false,
		};

		pauseMenu.returnLastUI_InGame.returnCommandStackInGame.Push(newCommand);

		try
		{
			MenuOperateSound.Instance.PlayMenuOpenSound_InGame(); // 播放菜单启动音效
		}
		catch
		{
			Debug.LogWarning("场景中没有音效播放器，可能是因为本场景是点击进入的，而不是从主菜单加载进入的");
		}

		Debug.Log("打开暂停菜单");
	}

	public override void OnUpdate(PauseMenu pauseMenu)
	{
		// 这里执行返回（和关闭的）逻辑
		pauseMenu.returnLastUI_InGame.ReturnLastMenuUI(); // 实时检测执行返回上一个 UI 的功能
	}

}
