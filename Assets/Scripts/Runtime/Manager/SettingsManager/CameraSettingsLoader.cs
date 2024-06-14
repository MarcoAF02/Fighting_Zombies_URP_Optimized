using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 摄像机设置读取
/// </summary>
public class CameraSettingsLoader : MonoBehaviour
{
	#region 基本组件和变量

	[SerializeField] private UniversalAdditionalCameraData _cameraData;

	private Coroutine loadCameraAntiAliasingSettings_IECor;

	#endregion

	#region 基本生命周期函数

	private void Start()
	{
		loadCameraAntiAliasingSettings_IECor = StartCoroutine(LoadCameraAntiAliasingSettings_IE());
	}

	#endregion

	#region 设置读取功能

	// TODO: 这个延迟时间暂时是写死的
	// 游戏开始时注册自己，应用设置的函数写在 SettingsLoader 类中
	private IEnumerator LoadCameraAntiAliasingSettings_IE()
	{
		yield return new WaitForSecondsRealtime(0.1f);

		try
		{
			SettingsLoader.Instance.cameraSettingsLoaderList.Add(this);
		}
		catch
		{
			Debug.LogWarning("场景中没有设置加载器，可能是因为本场景是点击进入的，而不是从主菜单加载进入的");
		}
	}

	/// <summary>
	/// 为摄像机应用设置
	/// </summary>
	/// <param name=""></param>
	public void LoadCameraAntiAliasingSettings()
	{
		int _index = 0;

		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.postProcessingAntiAliasing)
			{
				_index = SettingsLoader.Instance.gameSettingsList[i]._settingListIndex;
			}
		}

		if (_index == 0)
		{
			_cameraData.antialiasing = AntialiasingMode.None;
		}

		if (_index == 1)
		{
			_cameraData.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
		}

		if (_index == 2)
		{
			_cameraData.antialiasing = AntialiasingMode.FastApproximateAntialiasing;
		}

		if (_index == 3)
		{
			_cameraData.antialiasing = AntialiasingMode.TemporalAntiAliasing;
		}
	}

	#endregion

}
