using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏内的暂停菜单返回控制器，用于控制游戏内的菜单 UI
/// </summary>
public class ReturnLastUI_InGame : MonoBehaviour
{
	#region 基本组件和变量

	private FPS_Play_Action fpsPlayAction; // 输入系统

	[Header("玩家设置加载器")]
	[SerializeField] private PlayerSettingsLoader playerSettingsLoader;
	[Header("玩家暂停菜单控制器")]
	public PauseMenu pauseMenu;
	[Header("UI 返回指令栈")]
	public Stack<ReturnCommand> returnCommandStackInGame = new Stack<ReturnCommand>();

	#endregion

	#region 基本生命周期函数

	private void Awake()
	{
		fpsPlayAction = new FPS_Play_Action();
		fpsPlayAction.Enable();
	}

	private void OnDestroy()
	{
		fpsPlayAction?.Disable();
	}

	#endregion

	#region 菜单返回功能实现

	/// <summary>
	/// 菜单返回功能（根据栈中指令设置 UI 的显示与隐藏）
	/// </summary>
	public void ReturnLastMenuUI()
	{
		if (fpsPlayAction.UI_Keyboard_And_Mouse.Player_OpenPauseMenu.WasPressedThisFrame() ||
		fpsPlayAction.UI_Keyboard_And_Mouse.Mouse_Cancel.WasPressedThisFrame())
		{
			if (returnCommandStackInGame.Count <= 1) // 最新指令出栈
			{
				// 菜单（根物体）打开和关闭的音效在 FSM 状态机（类）里
				pauseMenu.ClosePauseMenu(); // 直接关闭菜单
				return;
			}

			ReturnCommand returnCommand = returnCommandStackInGame.Pop();
			PerformReturnLastUI(returnCommand);

			try
			{
				if (returnCommandStackInGame.Count > 1) MenuOperateSound.Instance.PlayCancelSound(); // 播放返回的音效
			}
			catch
			{
				Debug.LogWarning("场景中没有音效播放器，可能是因为本场景是点击进入的，而不是从主菜单加载进入的");
			}
		}
	}
	
	// 返回上级 UI 实际执行
	public void PerformReturnLastUI(ReturnCommand _returnCommand)
	{
		if (_returnCommand._displayObj != null) _returnCommand._displayObj.SetActive(true);
		if (_returnCommand._hideObj != null) _returnCommand._hideObj.SetActive(false);

		if (_returnCommand._saveSettingsData) // 应用并保存设置
		{
			try
			{
				SettingsLoader.Instance.ApplyAllGraphicsSettings(false);
				SettingsLoader.Instance.ApplyAllAudioSettings(false); // 应用所有一次性设置
				SaveLoadManager.Instance.SaveGameSettingsData();

				playerSettingsLoader.LoadGameSettings();
			}
			catch
			{
				Debug.LogWarning("没有找到设置加载器或存档管理器");
			}
		}
	}

	#endregion
}
