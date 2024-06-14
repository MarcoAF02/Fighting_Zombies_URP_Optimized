using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 返回上一个 UI 的控制器
/// </summary>
public class ReturnLastUI : MonoBehaviour
{
	#region 基本组件和变量

	[Header("模式关卡展示背景")]
	[SerializeField] private GameObject gameShowBGObj;

	private FPS_Play_Action fpsPlayAction;

	#endregion

	#region 基本生命周期函数

	private void Awake()
	{
		fpsPlayAction = new FPS_Play_Action();
		fpsPlayAction.Enable();
	}

	private void Update()
	{
		if (fpsPlayAction.UI_Keyboard_And_Mouse.Mouse_Cancel.WasPressedThisFrame())
		{
			if (MainMenu.Instance.returnCommandStack.Count > 0)
			{
				if (MainMenu.Instance.returnCommandStack.Count == 1) // 这里表示再下一步就要返回主菜单，隐藏关卡展示背景（无论有没有关卡展示背景）
				{
					gameShowBGObj.SetActive(false);
				}

				MainMenu.Instance.gameShowController.ClearShowImage(); // 一旦点击返回键，就会清空描述图片背景
				ReturnCommand returnCommand = MainMenu.Instance.returnCommandStack.Pop();
				PerformReturnLastUI(returnCommand);
			}
			else
			{
				Debug.Log("现在在主菜单，右键不能返回");
			}
		}
	}

	private void OnDestroy()
	{
		fpsPlayAction.Disable();
	}

	#endregion

	#region 返回功能实现

	/// <summary>
	/// 返回上级菜单
	/// </summary>
	private void PerformReturnLastUI(ReturnCommand command)
	{
		MenuOperateSound.Instance.PlayCancelSound();

		if (command._hideObj != null) command._hideObj.SetActive(false);
		if (command._displayObj != null) command._displayObj.SetActive(true);

		if (command._saveSettingsData) // 应用并保存设置
		{
			try
			{
				SettingsLoader.Instance.ApplyAllGraphicsSettings(false);
				SettingsLoader.Instance.ApplyAllAudioSettings(false); // 应用所有一次性设置
				SaveLoadManager.Instance.SaveGameSettingsData(); // 保存设置
			}
			catch
			{
				Debug.LogWarning("没有找到设置加载器或存档管理器");
			}
		}
	}

	#endregion
}
