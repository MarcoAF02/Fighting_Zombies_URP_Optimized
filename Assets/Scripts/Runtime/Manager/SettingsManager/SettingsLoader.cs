using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ϸ���ü�����
/// </summary>
public class SettingsLoader : MonoSingleton<SettingsLoader>, GameSettingsISaveable
{
	#region ��������ͱ���

	[Header("���볡��ʱ��Ӧ��ͼ�����õ��ӳ�ʱ��")]
	[SerializeField] private float applyGraphicsSettingsDelayTime = 0.2f;
	[Header("���볡��ʱ��Ӧ����Ƶ���õ��ӳ�ʱ��")]
	[SerializeField] private float applyAudioSettingsDealyTime = 0.2f;

	[Header("�������õ����ƹ�����")]
	public AllSettingsName allSettingsName;

	[Header("��Ϸ���ù�����")]
	public GameSettingsManager gameSettingsManager;
	[Header("ͼ�����ù�����")]
	public GraphicsSettingsManager graphicsSettingsManager;
	[Header("��Ƶ���ù�����")]
	public AudioSettingsManager audioSettingsManager;

	[Header("����ע���������������ýű�")]
	public List<CameraSettingsLoader> cameraSettingsLoaderList = new List<CameraSettingsLoader>();

	[Header("��Ҫд�����Ϸ����ֵ����Ҫ�ڱ༭��������ֶ�ָ��")]
	public List<GameSettingsToUse> gameSettingsList = new List<GameSettingsToUse>();

	public string GUID => GetComponent<DataGUID>().guid;

	// Э��
	private Coroutine applyAllSettings_IECor;
	private Coroutine applyAudioSettings_IECor;

	#endregion

	#region �����������ں���

	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		SaveLoadManager.Instance.RegisterGameSettingsSaveable(this);
		SaveLoadManager.Instance.LoadGameSettingsData(); // ��Ϸ��ʼʱ��ȡ�浵
	}

	#endregion

	#region Ӧ��һ���Ը��ĵ�����

	/// <summary>
	/// Ӧ������ͼ�����ã������������Ƿ�Ҫ�ӳ�
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
	/// ���볡��ʱ�ӳ�Ӧ��������Ƶ����
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

	// �� SaveLoadManager ͨ�ţ�����ͼ�����Ϸ����

	#region ���ô浵�ӿ�ʵ��

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
