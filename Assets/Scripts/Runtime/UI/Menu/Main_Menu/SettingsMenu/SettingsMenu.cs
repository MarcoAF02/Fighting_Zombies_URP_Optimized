using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 枚举：记录当前这个设置菜单在什么地方
/// </summary>
public enum SettingsMenuState
{
	None,
	MainMenu,
	InGamePlay
}

/// <summary>
/// 设置菜单根管理器
/// </summary>
public class SettingsMenu : MonoBehaviour
{
	#region 基本组件和变量

	public SettingsMenuState menuState = SettingsMenuState.None;

	[Header("设置父菜单的遮罩")]
	public GameObject settingsSortMaskImage;
	[Header("设置详细菜单的遮罩")]
	public GameObject settingsDetailsMaskImage;

	[Header("游戏设置子菜单")]
	[SerializeField] private GameObject gameSettingsMenu;
	[Header("图像设置子菜单")]
	[SerializeField] private GameObject graphicsSettingsMenu;
	[Header("音频设置子菜单")]
	[SerializeField] private GameObject audioSettingsMenu;

	[Header("所有设置细节菜单的引用")]
	[SerializeField] private List<GameObject> allDetailsMenuList = new List<GameObject>();

	#endregion

	#region 按钮点击事件：设置子菜单的显示

	// 显示游戏设置子菜单
	public void DisplayGameSettingDetailsMenu()
	{
		HideAllDetailsMenu();

		if (menuState == SettingsMenuState.None)
		{
			Debug.LogWarning("没有设置好菜单的状态，请检查游戏设置");
		}

		if (menuState == SettingsMenuState.MainMenu)
		{
			SetReturnCommand_MainMenuState();
		}

		if (menuState == SettingsMenuState.InGamePlay)
		{
			SetReturnCommand_InGame();
		}

		settingsSortMaskImage.SetActive(true);
		gameSettingsMenu.SetActive(true);
		settingsDetailsMaskImage.SetActive(false);

		try
		{
			MenuOperateSound.Instance.PlayCheckSound(); // 播放确认音效
		}
		catch
		{
			Debug.LogWarning("场景中没有音效播放器，可能是因为本场景是点击进入的，而不是从主菜单加载进入的");
		}
	}

	// 显示图像设置子菜单
	public void DisplayGraphicsSettingsDetailsMenu()
	{
		HideAllDetailsMenu();

		if (menuState == SettingsMenuState.None)
		{
			Debug.LogWarning("没有设置好菜单的状态，请检查游戏设置");
		}

		if (menuState == SettingsMenuState.MainMenu)
		{
			SetReturnCommand_MainMenuState();
		}

		if (menuState == SettingsMenuState.InGamePlay)
		{
			SetReturnCommand_InGame();
		}

		settingsSortMaskImage.SetActive(true);
		graphicsSettingsMenu.SetActive(true);
		settingsDetailsMaskImage.SetActive(false);

		try
		{
			MenuOperateSound.Instance.PlayCheckSound(); // 播放确认音效
		}
		catch
		{
			Debug.LogWarning("场景中没有音效播放器，可能是因为本场景是点击进入的，而不是从主菜单加载进入的");
		}
	}

	// 显示音频设置子菜单
	public void DisplayAudioSettingsDetailsMenu()
	{
		HideAllDetailsMenu();

		if (menuState == SettingsMenuState.None)
		{
			Debug.LogWarning("没有设置好菜单的状态，请检查游戏设置");
		}

		if (menuState == SettingsMenuState.MainMenu)
		{
			SetReturnCommand_MainMenuState();
		}

		if (menuState == SettingsMenuState.InGamePlay)
		{
			SetReturnCommand_InGame();
		}

		settingsSortMaskImage.SetActive(true);
		audioSettingsMenu.SetActive(true);
		settingsDetailsMaskImage.SetActive(false);

		try
		{
			MenuOperateSound.Instance.PlayCheckSound(); // 播放音效
		}
		catch
		{
			Debug.LogWarning("场景中没有音效播放器，可能是因为本场景是点击进入的，而不是从主菜单加载进入的");
		}
	}

	#endregion

	#region 隐藏设置细节 UI，储存返回指令

	// 隐藏所有菜单显示
	private void HideAllDetailsMenu()
	{
		for (int i = 0; i < allDetailsMenuList.Count; i ++)
		{
			allDetailsMenuList[i].gameObject.SetActive(false);
		}
	}

	// 保存菜单返回指令（游戏内）
	private void SetReturnCommand_InGame()
	{
		ReturnCommand newCommand = new ReturnCommand()
		{
			_hideObj = settingsSortMaskImage,
			_displayObj = settingsDetailsMaskImage,
			_saveSettingsData = true, // 保存设置信息
		};

		Transform playMenuCanvas = transform.parent; // 拿到玩家菜单 Canvas 根物体
		PauseMenu pauseMenu = playMenuCanvas.GetComponentInChildren<PauseMenu>();

		pauseMenu.returnLastUI_InGame.returnCommandStackInGame.Push(newCommand);
	}

	// 保存菜单返回指令（主菜单）
	private void SetReturnCommand_MainMenuState()
	{
		ReturnCommand newCommand = new ReturnCommand()
		{
			_hideObj = settingsSortMaskImage,
			_displayObj = settingsDetailsMaskImage,
			_saveSettingsData = true, // 保存设置信息
		};

		MainMenu.Instance.returnCommandStack.Push(newCommand);
	}

	#endregion

}
