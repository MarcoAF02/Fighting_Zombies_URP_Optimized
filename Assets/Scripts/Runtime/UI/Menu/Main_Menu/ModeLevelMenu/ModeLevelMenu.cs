using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏模式和关卡选择菜单
/// </summary>
public class ModeLevelMenu : MonoBehaviour
{
	#region 基本组件和变量

	[Header("模式选择菜单")]
	[SerializeField] private GameObject modeSelectMenu;
	[Header("关卡选择菜单")]
	[SerializeField] private GameObject levelSelectMenu;

	#endregion

	#region 按钮点击事件

	// 进入游戏关卡选择菜单
	public void EnterGameLevelSelectMenu()
	{
		modeSelectMenu.SetActive(false);
		levelSelectMenu.SetActive(true);

		ReturnCommand newCommand = new ReturnCommand()
		{
			_hideObj = levelSelectMenu,
			_displayObj = modeSelectMenu,
			_saveSettingsData = false,
		};

		MainMenu.Instance.returnCommandStack.Push(newCommand);
		MenuOperateSound.Instance.PlayCheckSound();
	}

	#endregion
}
