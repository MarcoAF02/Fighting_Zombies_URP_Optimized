using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

public class GraphicsSettingsManager : MonoBehaviour
{
	#region ��������ͱ���

	[Header("URP ��Ⱦ��������")]
	public UniversalRenderPipelineAsset urpAsset;
	public UniversalRendererData urpData;

	[Header("��Ļ�ֱ����б��¼��")]
	[SerializeField] private ScreenResolutionRecordList screenResolutionRecordList;

	private FullScreenMode currentFullScreenMode = FullScreenMode.FullScreenWindow;

	#endregion

	#region ͼ������

	/// <summary>
	/// �ı���ʾģʽ
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
	/// �ı���Ļ�ֱ���
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
	/// �ı���Ϸ����Ⱦ����
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
	/// Ӧ�����еĺ��ڴ�����ݣ����������
	/// </summary>
	public void ApplyAllCameraAA()
	{
		for (int i = 0; i < SettingsLoader.Instance.cameraSettingsLoaderList.Count; i++)
		{
			SettingsLoader.Instance.cameraSettingsLoaderList[i].LoadCameraAntiAliasingSettings();
		}
	}

	/// <summary>
	/// �������֡��
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
	/// ���ô�ֱͬ�������û�رգ�
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
	/// ���û������ڱ�
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
							Debug.LogWarning("��һ����Ⱦ���ܵ����Ʋ��� SSAO����������");
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
							Debug.LogWarning("��һ����Ⱦ���ܵ����Ʋ��� SSAO����������");
						}
					}
				}
			}
		}
	}

	// ��Ӱ�����Ĵ�С����ı���Ӱ��Ⱦ���룬��Ӧ��ֵΪ��
	// �أ�0���ͣ�20���У�40���ߣ�100�����£�160

	/// <summary>
	/// ����������Ӱ����
	/// </summary>
	public void SetShadowQuality()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i ++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.shadowQuality)
			{
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 0) // ��
				{
					urpAsset.shadowDistance = 0f;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 1) // ��
				{
					urpAsset.shadowDistance = 20f;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 2) // ��
				{
					urpAsset.shadowDistance = 40f;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 3) // ��
				{
					urpAsset.shadowDistance = 100f;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 4) // ����
				{
					urpAsset.shadowDistance = 160f;
				}
			}
		}
	}

	// ע�⣺�����ʵ�ֵ����޸���Ӱ�ֱ��ʣ���Ҫ���� URP �� UniversalRenderPipelineAsset ���޸��������Ϊ ����ɷ���

	/// <summary>
	/// ����ƽ�й���Ӱ����
	/// </summary>
	[Obsolete("��ֹ֧ͣ�ֶ���Ӱ�ֱ��ʵĶ�̬��������Ӱ�����ĵ�������������һ��ѡ����")]
	public void SetDirectionalLightShadowQuality()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.directionalLightShadowQuality)
			{
				//if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 0) // ��
				//{
				// 	urpAsset.externalSupportsMainLightShadows = false;
				//	urpAsset.externalMainLightRenderingMode = LightRenderingMode.PerPixel;
				//	urpAsset.externalmainLightShadowmapResolution = 512;
				//}

				//if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 1) // ��
				//{
				//	urpAsset.externalSupportsMainLightShadows = true;
				//	urpAsset.externalMainLightRenderingMode = LightRenderingMode.PerPixel;
				//	urpAsset.externalmainLightShadowmapResolution = 512;
				//}

				//if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 2) // ��
				//{
				//	urpAsset.externalSupportsMainLightShadows = true;
				//	urpAsset.externalMainLightRenderingMode = LightRenderingMode.PerPixel;
				//	urpAsset.externalmainLightShadowmapResolution = 1024;
				//}

				//if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 3) // ��
				//{
				//	urpAsset.externalSupportsMainLightShadows = true;
				//	urpAsset.externalMainLightRenderingMode = LightRenderingMode.PerPixel;
				//	urpAsset.externalmainLightShadowmapResolution = 2048;
				//}

				//if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 4) // ����
				//{
				//	urpAsset.externalSupportsMainLightShadows = true;
				//	urpAsset.externalMainLightRenderingMode = LightRenderingMode.PerPixel;
				//	urpAsset.externalmainLightShadowmapResolution = 4096;
				//}

				Debug.LogWarning("��ֹ֧ͣ�ֶ���Ӱ�ֱ��ʵĶ�̬��������Ӱ�����ĵ�������������һ��ѡ����");
			}
		}
	}

	/// <summary>
	/// ���ó�����Դ��Ӱ����
	/// </summary>
	[Obsolete("��ֹ֧ͣ�ֶ���Ӱ�ֱ��ʵĶ�̬��������Ӱ�����ĵ�������������һ��ѡ����")]
	public void SetSceneLightShadowQuality()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.sceneLightShadowQuality)
			{
				//if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 0) // ��
				//{
				//	urpAsset.externalSupportsAdditionalLightShadows = false;
				//	urpAsset.externalAdditionalLightsRenderingMode = LightRenderingMode.PerPixel;
				//	urpAsset.externalAdditionalLightsShadowmapResolution = 512;
				//}

				//if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 1) // ��
				//{
				//	urpAsset.externalSupportsAdditionalLightShadows = true;
				//	urpAsset.externalAdditionalLightsRenderingMode = LightRenderingMode.PerPixel;
				//	urpAsset.externalAdditionalLightsShadowmapResolution = 512;
				//}

				//if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 2) // ��
				//{
				//	urpAsset.externalSupportsAdditionalLightShadows = true;
				//	urpAsset.externalAdditionalLightsRenderingMode = LightRenderingMode.PerPixel;
				//	urpAsset.externalAdditionalLightsShadowmapResolution = 1024;
				//}

				//if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 3) // ��
				//{
				//	urpAsset.externalSupportsAdditionalLightShadows = true;
				//	urpAsset.externalAdditionalLightsRenderingMode = LightRenderingMode.PerPixel;
				//	urpAsset.externalAdditionalLightsShadowmapResolution = 2048;
				//}

				//if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 4) // ����
				//{
				//	urpAsset.externalSupportsAdditionalLightShadows = true;
				//	urpAsset.externalAdditionalLightsRenderingMode = LightRenderingMode.PerPixel;
				//	urpAsset.externalAdditionalLightsShadowmapResolution = 4096;
				//}

				Debug.LogWarning("��ֹ֧ͣ�ֶ���Ӱ�ֱ��ʵĶ�̬��������Ӱ�����ĵ�������������һ��ѡ����");
			}
		}
	}

	/// <summary>
	/// ������Ӱ����Ⱦ����
	/// </summary>
	[Obsolete("��ֹ֧ͣ�ֶ���Ӱ�ֱ��ʵĶ�̬��������Ӱ�����ĵ�������������һ��ѡ����")]
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

				Debug.LogWarning("��ֹ֧ͣ�ֶ���Ӱ�ֱ��ʵĶ�̬��������Ӱ�����ĵ�������������һ��ѡ����");
			}
		}
	}

	/// <summary>
	/// ������������
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
	/// ����������ˣ��������Թ��ˣ�
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
