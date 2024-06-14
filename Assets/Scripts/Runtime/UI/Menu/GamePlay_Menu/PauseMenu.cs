using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 暂停菜单
/// </summary>
public class PauseMenu : MonoBehaviour
{
	#region 基本组件和变量

	private FPS_Play_Action fpsPlayAction;

	[Header("玩家角色控制器")]
	public PlayerController playerController;
	[Header("暂停菜单根物体")]
	public GameObject pauseMenuRoot;
	[Header("设置菜单根跟物体")]
	public GameObject settingsMenu_Root_UI;
	[Header("暂停选择菜单根物体")]
	public GameObject pauseMenuChooseRoot;
	[Header("确定是否要重新开始的 UI 菜单根物体")]
	public GameObject checkRestartMenuRoot;
	[Header("确定是否要返回主菜单的 UI 菜单根物体")]
	public GameObject checkReturnMainMenuRoot;
	[Header("UI 返回功能控制器")]
	public ReturnLastUI_InGame returnLastUI_InGame;

	[Header("暂停菜单打开的 CD 时间")]
	public float openPauseMenuCDTime;
	public float openPauseMenuTotalTime;

	[Header("暂停菜单关闭的 CD 时间")]
	public float closePauseMenuCDTime;
	public float closePauseMenuTotalTime;

	// 暂停菜单状态机
	public PauseMenuBaseState pauseMenuCurrentState; // 当前所处状态
	public PauseMenuOpenState pauseMenuOpenState = new PauseMenuOpenState();
	public PauseMenuCloseState pauseMenuCloseState = new PauseMenuCloseState();

	// 协程
	private Coroutine calculateOpenPauseMenuCDTime_IECor;

	#endregion

	#region 状态机切换函数

	public void SwitchState(PauseMenuBaseState _state, bool firstEnterState)
	{
		pauseMenuCurrentState = _state;
		pauseMenuCurrentState.EnterState(this, firstEnterState);
	}

	#endregion

	#region 基本生命周期函数

	private void Awake()
	{
		fpsPlayAction = new FPS_Play_Action();
		fpsPlayAction.Enable();
	}

	private void Start()
	{
		SwitchState(pauseMenuCloseState, true);
	}

	private void Update()
	{
		pauseMenuCurrentState.OnUpdate(this);
	}

	private void OnDestroy()
	{
		fpsPlayAction?.Disable();
	}

	#endregion

	#region 暂停菜单的打开与关闭

	/// <summary>
	/// 打开暂停菜单
	/// </summary>
	public void OpenPauseMenu()
	{
		if (fpsPlayAction.UI_Keyboard_And_Mouse.Player_OpenPauseMenu.WasPressedThisFrame())
		{
			// 玩家只有在控制状态才能打开暂停菜单，否则不行
			if (playerController.playerCurrentState != playerController.playerControlState) return;
			if (closePauseMenuTotalTime > 0f) return; // CD 时间未到
			SwitchState(pauseMenuOpenState, false);
		}
	}

	// 返回逻辑：如果栈上只有一个指令，表示就要退出暂停菜单了，需要特殊判断一下

	/// <summary>
	/// 直接关闭暂停菜单
	/// </summary>
	public void ClosePauseMenu()
	{
		if (openPauseMenuTotalTime > 0f) return;
		SwitchState(pauseMenuCloseState, false);
	}

	/// <summary>
	/// 计算暂停菜单打开的 CD 时间
	/// </summary>
	public void CalculateOpenPauseMenuCDTime()
	{
		calculateOpenPauseMenuCDTime_IECor = StartCoroutine(CalculateOpenPauseMenuCDTime_IE());
	}

	private IEnumerator CalculateOpenPauseMenuCDTime_IE()
	{
		yield return new WaitForSecondsRealtime(openPauseMenuCDTime); // 这个不受 timeScale 影响
		openPauseMenuTotalTime = 0f;
	}

	#endregion

	#region 按钮点击事件

	/// <summary>
	/// 不保存并退出当前游戏（显示确认菜单）
	/// </summary>
	public void DisplayCheckReturnMainMenu()
	{
		checkReturnMainMenuRoot.SetActive(true);
		pauseMenuChooseRoot.SetActive(false);

		ReturnCommand newCommand = new ReturnCommand()
		{
			_hideObj = checkReturnMainMenuRoot,
			_displayObj = pauseMenuChooseRoot,
		};

		returnLastUI_InGame.returnCommandStackInGame.Push(newCommand);

		try
		{
			MenuOperateSound.Instance.PlayCheckSound(); // 播放确认音效
		}
		catch
		{
			Debug.LogWarning("场景中没有音效播放器，可能是因为本场景是点击进入的，而不是从主菜单加载进入的");
		}
	}

	/// <summary>
	/// 重新开始当前场景的游戏（显示确认菜单）
	/// </summary>
	public void DisplayCheckRestartCurrentScene()
	{
		checkRestartMenuRoot.SetActive(true);
		pauseMenuChooseRoot.SetActive(false);

		ReturnCommand newCommand = new ReturnCommand()
		{
			_hideObj = checkRestartMenuRoot,
			_displayObj = pauseMenuChooseRoot,
		};

		returnLastUI_InGame.returnCommandStackInGame.Push(newCommand);

		try
		{
			MenuOperateSound.Instance.PlayCheckSound(); // 播放确认音效
		}
		catch
		{
			Debug.LogWarning("场景中没有音效播放器，可能是因为本场景是点击进入的，而不是从主菜单加载进入的");
		}
	}

	/// <summary>
	/// 按钮打开设置菜单
	/// </summary>
	public void OpenSettingsMenu()
	{
		settingsMenu_Root_UI.SetActive(true);
		pauseMenuRoot.SetActive(false);

		ReturnCommand newCommand = new ReturnCommand()
		{
			_displayObj = pauseMenuRoot,
			_hideObj = settingsMenu_Root_UI,
		};

		returnLastUI_InGame.returnCommandStackInGame.Push(newCommand);

		try
		{
			MenuOperateSound.Instance.PlayCheckSound(); // 播放确认音效
		}
		catch
		{
			Debug.LogWarning("场景中没有音效播放器，可能是因为本场景是点击进入的，而不是从主菜单加载进入的");
		}
	}

	/// <summary>
	/// 按钮点击返回上一个 UI
	/// </summary>
	public void ActiveReturnLastUI()
	{
		if (returnLastUI_InGame.returnCommandStackInGame.Count <= 1) // 最新指令出栈
		{
			// 菜单（根物体）打开和关闭的音效在 FSM 状态机（类）里
			returnLastUI_InGame.pauseMenu.ClosePauseMenu(); // 直接关闭菜单
			return;
		}

		ReturnCommand returnCommand = returnLastUI_InGame.returnCommandStackInGame.Pop();
		returnLastUI_InGame.PerformReturnLastUI(returnCommand);

		try
		{
			MenuOperateSound.Instance.PlayCancelSound(); // 播放返回音效
		}
		catch
		{
			Debug.LogWarning("场景中没有音效播放器，可能是因为本场景是点击进入的，而不是从主菜单加载进入的");
		}
	}

	#endregion

}
