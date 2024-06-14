using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 主菜单控制器
/// </summary>
public class MainMenu : MonoSingleton<MainMenu>
{
	#region 基本组件和变量

	[Header("返回功能控制器")]
	public ReturnLastUI returnLastUI;

	[Header("菜单游戏物体")]
	[Header("主菜单")]
	public GameObject mainMenuRoot;
	[Header("模式关卡选择菜单")]
	public GameObject modeAndLevelRoot;
	[Header("设置菜单")]
	public GameObject settingsMenuRoot;

	[Header("模式关卡展示控制器")]
	public GameShowController gameShowController;
	[Header("模式关卡展示 UI 背景物体")]
	public GameObject gameShowBGObj;

	[Header("主菜单按钮")]
	[Header("游戏开始按钮")]
	[SerializeField] private Button gameStartButton;
	[Header("设置按钮")]
	[SerializeField] private Button settingsButton;
	[Header("退出游戏按钮")]
	[SerializeField] private Button exitButton;

	// 记录返回指令的栈
	public Stack<ReturnCommand> returnCommandStack = new Stack<ReturnCommand>();

	#endregion

	#region 菜单切换功能（按钮点击事件）

	/// <summary>
	/// 进入设置菜单
	/// </summary>
	public void EnterSettingsMenu()
	{
		SwitchDisplayUI(mainMenuRoot, settingsMenuRoot);

		ReturnCommand newCommand = new ReturnCommand()
		{
			_hideObj = settingsMenuRoot,
			_displayObj = mainMenuRoot,
			_saveSettingsData = false,
		};

		returnCommandStack.Push(newCommand);
	}

	/// <summary>
	/// 进入模式 - 关卡选择菜单
	/// </summary>
	public void EnterModeAndLevelMenu()
	{
		SwitchDisplayUI(mainMenuRoot, modeAndLevelRoot);
		gameShowBGObj.SetActive(true);

		ReturnCommand newCommand = new ReturnCommand()
		{
			_hideObj = modeAndLevelRoot,
			_displayObj = mainMenuRoot,
			_saveSettingsData = false,
		};

		returnCommandStack.Push(newCommand);
	}

	/// <summary>
	/// 切换当前显示的 UI
	/// </summary>
	/// <param name="_hideUI"></param>
	/// <param name="_displayUI"></param>
	public void SwitchDisplayUI(GameObject _hideUI, GameObject _displayUI)
	{
		MenuOperateSound.Instance.PlayCheckSound();
		_hideUI.SetActive(false);
		_displayUI.SetActive(true);
	}

	public void ExitGame()
	{
		Debug.Log("退出游戏");
		MenuOperateSound.Instance.PlayCheckSound();
		Application.Quit();
	}

	#endregion

}
