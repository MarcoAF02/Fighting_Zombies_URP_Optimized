using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

public class GraphicsSettingsManager : MonoBehaviour
{
	#region 基本组件和变量

	[Header("URP 渲染管线设置")]
	public UniversalRenderPipelineAsset urpAsset;
	public UniversalRendererData urpData;

	[Header("屏幕分辨率列表记录类")]
	[SerializeField] private ScreenResolutionRecordList screenResolutionRecordList;

	private FullScreenMode currentFullScreenMode = FullScreenMode.FullScreenWindow;

	#endregion

	#region 图形设置

	/// <summary>
	/// 改变显示模式
	/// </summary>
	public void ChangeDisplayMode()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.displayMode)
			{
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 0)
				{
					Screen.fullScreenMode = FullScreenMode.Windowed;
					currentFullScreenMode = FullScreenMode.Windowed;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 1)
				{
					Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
					currentFullScreenMode = FullScreenMode.ExclusiveFullScreen;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 2)
				{
					Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
					currentFullScreenMode = FullScreenMode.FullScreenWindow;
				}
			}
		}
	}

	/// <summary>
	/// 改变屏幕分辨率
	/// </summary>
	public void ChangeResolutionRatio()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.resolutionRatio)
			{
				int index = SettingsLoader.Instance.gameSettingsList[i]._settingListIndex;

				for (int j = 0; j < screenResolutionRecordList.screenResolutionRecordList.Count; j++)
				{
					Screen.SetResolution(screenResolutionRecordList.screenResolutionRecordList[index].width, screenResolutionRecordList.screenResolutionRecordList[index].height, currentFullScreenMode);
				}
			}
		}
	}

	/// <summary>
	/// 改变游戏内渲染倍数
	/// </summary>
	public void ChangeRenderScale()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.renderScale)
			{
				urpAsset.renderScale = (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex / 10f) + 0.5f;
			}
		}
	}

	/// <summary>
	/// 应用所有的后期处理抗锯齿（至摄像机）
	/// </summary>
	public void ApplyAllCameraAA()
	{
		for (int i = 0; i < SettingsLoader.Instance.cameraSettingsLoaderList.Count; i++)
		{
			SettingsLoader.Instance.cameraSettingsLoaderList[i].LoadCameraAntiAliasingSettings();
		}
	}

	/// <summary>
	/// 设置最大帧率
	/// </summary>
	public void SetMaxFrameRate()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.maximumFrameRate)
			{
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 0)
				{
					Application.targetFrameRate = 20;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 1)
				{
					Application.targetFrameRate = 30;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 2)
				{
					Application.targetFrameRate = 60;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 3)
				{
					Application.targetFrameRate = 90;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 4)
				{
					Application.targetFrameRate = 120;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 5)
				{
					Application.targetFrameRate = 144;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 6)
				{
					Application.targetFrameRate = 165;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 7)
				{
					Application.targetFrameRate = 240;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 8)
				{
					Application.targetFrameRate = 300;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 9)
				{
					Application.targetFrameRate = -1;
				}
			}
		}
	}

	/// <summary>
	/// 设置垂直同步（启用或关闭）
	/// </summary>
	public void SetVSync()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.vSync)
			{
				QualitySettings.vSyncCount = SettingsLoader.Instance.gameSettingsList[i]._settingListIndex;
			}
		}
	}

	/// <summary>
	/// 设置环境光遮蔽
	/// </summary>
	public void SetAmbientOcclusion()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.ambientOcclusion)
			{
				if (urpData.rendererFeatures.Count > 0)
				{
					if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 0)
					{
						if (urpData.rendererFeatures[0].name == "SSAO")
						{
							urpData.rendererFeatures[0].SetActive(false);
						}
						else
						{
							Debug.LogWarning("第一个渲染功能的名称不叫 SSAO，请检查设置");
						}
					}

					if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 1)
					{
						if (urpData.rendererFeatures[0].name == "SSAO")
						{
							urpData.rendererFeatures[0].SetActive(true);
						}
						else
						{
							Debug.LogWarning("第一个渲染功能的名称不叫 SSAO，请检查设置");
						}
					}
				}
			}
		}
	}

	// 阴影质量的大小仅会改变阴影渲染距离，对应的值为：
	// 关：0，低：20，中：40，高：100，极致：160

	/// <summary>
	/// 设置整体阴影质量
	/// </summary>
	public void SetShadowQuality()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i ++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.shadowQuality)
			{
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 0) // 关
				{
					urpAsset.shadowDistance = 0f;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 1) // 低
				{
					urpAsset.shadowDistance = 20f;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 2) // 中
				{
					urpAsset.shadowDistance = 40f;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 3) // 高
				{
					urpAsset.shadowDistance = 100f;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 4) // 极致
				{
					urpAsset.shadowDistance = 160f;
				}
			}
		}
	}

	// 注意：如果想实现单独修改阴影分辨率，需要进入 URP 的 UniversalRenderPipelineAsset 类修改相关属性为 对外可访问

	/// <summary>
	/// 设置平行光阴影质量
	/// </summary>
	[Obsolete("已停止支持对阴影分辨率的动态调整，阴影质量的调整功能现在在一个选项里")]
	public void SetDirectionalLightShadowQuality()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.directionalLightShadowQuality)
			{
				//if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 0) // 关
				//{
				// 	urpAsset.externalSupportsMainLightShadows = false;
				//	urpAsset.externalMainLightRenderingMode = LightRenderingMode.PerPixel;
				//	urpAsset.externalmainLightShadowmapResolution = 512;
				//}

				//if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 1) // 低
				//{
				//	urpAsset.externalSupportsMainLightShadows = true;
				//	urpAsset.externalMainLightRenderingMode = LightRenderingMode.PerPixel;
				//	urpAsset.externalmainLightShadowmapResolution = 512;
				//}

				//if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 2) // 中
				//{
				//	urpAsset.externalSupportsMainLightShadows = true;
				//	urpAsset.externalMainLightRenderingMode = LightRenderingMode.PerPixel;
				//	urpAsset.externalmainLightShadowmapResolution = 1024;
				//}

				//if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 3) // 高
				//{
				//	urpAsset.externalSupportsMainLightShadows = true;
				//	urpAsset.externalMainLightRenderingMode = LightRenderingMode.PerPixel;
				//	urpAsset.externalmainLightShadowmapResolution = 2048;
				//}

				//if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 4) // 极致
				//{
				//	urpAsset.externalSupportsMainLightShadows = true;
				//	urpAsset.externalMainLightRenderingMode = LightRenderingMode.PerPixel;
				//	urpAsset.externalmainLightShadowmapResolution = 4096;
				//}

				Debug.LogWarning("已停止支持对阴影分辨率的动态调整，阴影质量的调整功能现在在一个选项里");
			}
		}
	}

	/// <summary>
	/// 设置场景光源阴影质量
	/// </summary>
	[Obsolete("已停止支持对阴影分辨率的动态调整，阴影质量的调整功能现在在一个选项里")]
	public void SetSceneLightShadowQuality()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.sceneLightShadowQuality)
			{
				//if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 0) // 关
				//{
				//	urpAsset.externalSupportsAdditionalLightShadows = false;
				//	urpAsset.externalAdditionalLightsRenderingMode = LightRenderingMode.PerPixel;
				//	urpAsset.externalAdditionalLightsShadowmapResolution = 512;
				//}

				//if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 1) // 低
				//{
				//	urpAsset.externalSupportsAdditionalLightShadows = true;
				//	urpAsset.externalAdditionalLightsRenderingMode = LightRenderingMode.PerPixel;
				//	urpAsset.externalAdditionalLightsShadowmapResolution = 512;
				//}

				//if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 2) // 中
				//{
				//	urpAsset.externalSupportsAdditionalLightShadows = true;
				//	urpAsset.externalAdditionalLightsRenderingMode = LightRenderingMode.PerPixel;
				//	urpAsset.externalAdditionalLightsShadowmapResolution = 1024;
				//}

				//if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 3) // 高
				//{
				//	urpAsset.externalSupportsAdditionalLightShadows = true;
				//	urpAsset.externalAdditionalLightsRenderingMode = LightRenderingMode.PerPixel;
				//	urpAsset.externalAdditionalLightsShadowmapResolution = 2048;
				//}

				//if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 4) // 极致
				//{
				//	urpAsset.externalSupportsAdditionalLightShadows = true;
				//	urpAsset.externalAdditionalLightsRenderingMode = LightRenderingMode.PerPixel;
				//	urpAsset.externalAdditionalLightsShadowmapResolution = 4096;
				//}

				Debug.LogWarning("已停止支持对阴影分辨率的动态调整，阴影质量的调整功能现在在一个选项里");
			}
		}
	}

	/// <summary>
	/// 设置阴影的渲染距离
	/// </summary>
	[Obsolete("已停止支持对阴影分辨率的动态调整，阴影质量的调整功能现在在一个选项里")]
	public void SetShadowRenderDistance()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.shadowRenderDistance)
			{
				//if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 0)
				//{
				//	urpAsset.shadowDistance = 20;
				//}

				//if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 1)
				//{
				//	urpAsset.shadowDistance = 40;
				//}

				//if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 2)
				//{
				//	urpAsset.shadowDistance = 100;
				//}

				//if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 3)
				//{
				//	urpAsset.shadowDistance = 160;
				//}

				Debug.LogWarning("已停止支持对阴影分辨率的动态调整，阴影质量的调整功能现在在一个选项里");
			}
		}
	}

	/// <summary>
	/// 调整纹理质量
	/// </summary>
	public void SetTextureQuality()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.textureQuality)
			{
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 0) QualitySettings.globalTextureMipmapLimit = 3;
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 1) QualitySettings.globalTextureMipmapLimit = 2;
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 2) QualitySettings.globalTextureMipmapLimit = 1;
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 3) QualitySettings.globalTextureMipmapLimit = 0;
			}
		}
	}

	/// <summary>
	/// 设置纹理过滤（各向异性过滤）
	/// </summary>
	public void SetTextureFiltering()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.textureFiltering)
			{
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 0)
				{
					QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
				}

				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 1)
				{
					QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
				}
			}
		}
	}

	#endregion

}
