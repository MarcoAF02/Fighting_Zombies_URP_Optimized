using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏设置加载器
/// </summary>
public class SettingsLoader : MonoSingleton<SettingsLoader>, GameSettingsISaveable
{
	#region 基本组件和变量

	[Header("进入场景时，应用图形设置的延迟时长")]
	[SerializeField] private float applyGraphicsSettingsDelayTime = 0.2f;
	[Header("进入场景时，应用音频设置的延迟时长")]
	[SerializeField] private float applyAudioSettingsDealyTime = 0.2f;

	[Header("所有设置的名称管理类")]
	public AllSettingsName allSettingsName;

	[Header("游戏设置管理类")]
	public GameSettingsManager gameSettingsManager;
	[Header("图形设置管理类")]
	public GraphicsSettingsManager graphicsSettingsManager;
	[Header("音频设置管理类")]
	public AudioSettingsManager audioSettingsManager;

	[Header("所有注册进来的摄像机设置脚本")]
	public List<CameraSettingsLoader> cameraSettingsLoaderList = new List<CameraSettingsLoader>();

	[Header("需要写入的游戏设置值，需要在编辑器面板中手动指定")]
	public List<GameSettingsToUse> gameSettingsList = new List<GameSettingsToUse>();

	public string GUID => GetComponent<DataGUID>().guid;

	// 协程
	private Coroutine applyAllSettings_IECor;
	private Coroutine applyAudioSettings_IECor;

	#endregion

	#region 基本生命周期函数

	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		SaveLoadManager.Instance.RegisterGameSettingsSaveable(this);
		SaveLoadManager.Instance.LoadGameSettingsData(); // 游戏开始时读取存档
	}

	#endregion

	#region 应用一次性更改的设置

	/// <summary>
	/// 应用所有图形设置，参数决定了是否要延迟
	/// </summary>
	/// <param name="wantDelay"></param>
	public void ApplyAllGraphicsSettings(bool wantDelay)
	{
		if (wantDelay)
		{
			applyAllSettings_IECor = StartCoroutine(ApplyAllGraphicsSettings_IE());
		}
		else
		{
			graphicsSettingsManager.ChangeResolutionRatio();
			graphicsSettingsManager.ChangeDisplayMode();
			graphicsSettingsManager.ChangeRenderScale();
			graphicsSettingsManager.ApplyAllCameraAA();
			graphicsSettingsManager.SetMaxFrameRate();
			graphicsSettingsManager.SetVSync();
			graphicsSettingsManager.SetAmbientOcclusion();
			graphicsSettingsManager.SetShadowQuality();
			graphicsSettingsManager.SetTextureQuality();
			graphicsSettingsManager.SetTextureFiltering();

			// graphicsSettingsManager.SetDirectionalLightShadowQuality();
			// graphicsSettingsManager.SetSceneLightShadowQuality();
			// graphicsSettingsManager.SetShadowRenderDistance();
		}
	}

	private IEnumerator ApplyAllGraphicsSettings_IE()
	{
		yield return new WaitForSecondsRealtime(applyGraphicsSettingsDelayTime);

		graphicsSettingsManager.ChangeResolutionRatio();
		graphicsSettingsManager.ChangeDisplayMode();
		graphicsSettingsManager.ChangeRenderScale();
		graphicsSettingsManager.ApplyAllCameraAA();
		graphicsSettingsManager.SetMaxFrameRate();
		graphicsSettingsManager.SetVSync();
		graphicsSettingsManager.SetAmbientOcclusion();
		graphicsSettingsManager.SetShadowQuality();
		graphicsSettingsManager.SetTextureQuality();
		graphicsSettingsManager.SetTextureFiltering();

		// graphicsSettingsManager.SetDirectionalLightShadowQuality();
		// graphicsSettingsManager.SetSceneLightShadowQuality();
		// graphicsSettingsManager.SetShadowRenderDistance();
	}

	/// <summary>
	/// 进入场景时延迟应用所有音频设置
	/// </summary>
	public void ApplyAllAudioSettings(bool wantDelay)
	{
		if (wantDelay)
		{
			applyAudioSettings_IECor = StartCoroutine(ApplyAllAudioSettings_IE());
		}
		else
		{
			audioSettingsManager.SetGlobalVolume();
			audioSettingsManager.SetBGMVolume();
			audioSettingsManager.SetSFXVolume();
			audioSettingsManager.SetSystemSFXVolume();
		}
	}

	private IEnumerator ApplyAllAudioSettings_IE()
	{
		yield return new WaitForSecondsRealtime(applyAudioSettingsDealyTime);

		audioSettingsManager.SetGlobalVolume();
		audioSettingsManager.SetBGMVolume();
		audioSettingsManager.SetSFXVolume();
		audioSettingsManager.SetSystemSFXVolume();
	}

	#endregion

	// 和 SaveLoadManager 通信，储存和加载游戏设置

	#region 设置存档接口实现

	public GameSettingsToUseList SaveGameSettingsInToData()
	{
		GameSettingsToUseList saveList = new GameSettingsToUseList();
		saveList.gameSettingsSaveList = new List<GameSettingsToUse>();

		for (int i = 0; i < gameSettingsList.Count; i++)
		{
			saveList.gameSettingsSaveList.Add(gameSettingsList[i]);
		}

		return saveList;
	}

	public void LoadGameSettingsSaveData(GameSettingsToUseList gameSettingsSaveData)
	{
		gameSettingsList.Clear();

		for (int i = 0; i < gameSettingsSaveData.gameSettingsSaveList.Count; i++)
		{
			gameSettingsList.Add(gameSettingsSaveData.gameSettingsSaveList[i]);
		}
	}

	#endregion

}
