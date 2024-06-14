using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsManager : MonoBehaviour
{
	#region 游戏设置具体功能

	// 注：游戏设置每次进入关卡时写入

	/// <summary>
	/// 读取鼠标灵敏度
	/// </summary>
	/// <returns></returns>
	public float LoadMouseSensitivity()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.mouseSensitivity)
			{
				// 这里注意数据类型
				return (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex + 1f) / 100f;
			}
		}

		Debug.LogWarning("设置的值没有对应上，可能超出了范围");
		return 0.01f;
	}

	// TODO: 未来的更新：准星有 点状，静态十字，动态十字

	/// <summary>
	/// 加载准星开关
	/// </summary>
	/// <returns></returns>
	public bool LoadFrontSight()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.frontSight)
			{
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 0)
				{
					return false;
				}

				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 1)
				{
					return true;
				}
			}
		}

		Debug.LogWarning("设置的值没有对应上，可能超出了范围");
		return true;
	}

	/// <summary>
	/// 读取摄像机 FOV
	/// </summary>
	/// <returns></returns>
	public float LoadCameraFOV()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.fieldOfView)
			{
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 0)
				{
					return 50f;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 1)
				{
					return 55f;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 2)
				{
					return 60f;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 3)
				{
					return 65f;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 4)
				{
					return 70f;
				}
			}
		}

		Debug.LogWarning("设置的值没有对应上，可能超出了范围");
		return 60f;
	}

	// TODO: 未来的更新：屏幕左下角加一个血条
	// 这个设置应该在 屏幕四周（普通），屏幕四周（减弱），生命值条 之间切换

	/// <summary>
	/// 加载屏幕受伤提示：普通还是减弱
	/// </summary>
	public int LoadScreenInjuryWarning()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.screenInjuryWarning)
			{
				return SettingsLoader.Instance.gameSettingsList[i]._settingListIndex;
			}
		}

		Debug.LogWarning("设置的值没有对应上，可能超出了范围");
		return 0;
	}

	/// <summary>
	/// 加载视角摇晃
	/// </summary>
	public bool LoadPerspectiveShake()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.shakingPerspective)
			{
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 0)
				{
					return false;
				}

				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 1)
				{
					return true;
				}
			}
		}

		Debug.LogWarning("设置的值没有对应上，可能超出了范围");
		return true;
	}

	/// <summary>
	/// 是否显示教程信息
	/// </summary>
	/// <returns></returns>
	public bool LoadDisplayTutorialInformation()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.displayTutorialInformation)
			{
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 0)
				{
					return false;
				}

				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 1)
				{
					return true;
				}
			}
		}

		Debug.LogWarning("设置的值没有对应上，可能超出了范围");
		return true;
	}

	#endregion
}
