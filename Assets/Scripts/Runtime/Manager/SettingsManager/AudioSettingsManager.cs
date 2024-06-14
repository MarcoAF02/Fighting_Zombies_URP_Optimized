using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; // AudioMixer 必须要这个

public class AudioSettingsManager : MonoBehaviour
{
	#region 基本组件和变量

	[Header("音频设置")]
	[SerializeField] private AudioMixer globalAudioMixer;

	[Header("可以调整的音量最大值（映射区间）")]
	[SerializeField] private float maxAudioVolumeInterval;

	#region 音量设置 Trigger 的名称

	[Header("全局音量")]
	[SerializeField] private string mainVolumeTrigger = "MainVolume";
	[Header("音乐音量")]
	[SerializeField] private string bgmVolumeTrigger = "BGMVolume";
	[Header("音效音量")]
	[SerializeField] private string sfxVolumeTrigger = "SFXVolume";
	[Header("系统音效音量")]
	[SerializeField] private string systemSFXVolumeTrigger = "SystemSFXVolume";

	#endregion

	#endregion

	#region 音频设置

	/// <summary>
	/// 设置全局音量
	/// </summary>
	public void SetGlobalVolume()
	{
		float volume = 0f;

		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.globalVolume)
			{
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 0)
				{
					volume = -80f;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex > 0) // 声音不是 0 的情况
				{
					volume = -(maxAudioVolumeInterval * ((10 - SettingsLoader.Instance.gameSettingsList[i]._settingListIndex) / 10f));
				}
			}
		}

		globalAudioMixer.SetFloat(mainVolumeTrigger, volume);
	}

	/// <summary>
	/// 设置背景音乐音量
	/// </summary>
	public void SetBGMVolume()
	{
		float volume = 0f;
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.bgmVolume)
			{
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 0)
				{
					volume = -80f;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex > 0) // 声音不是 0 的情况
				{
					volume = -(maxAudioVolumeInterval * ((10 - SettingsLoader.Instance.gameSettingsList[i]._settingListIndex) / 10f));
				}
			}
		}

		globalAudioMixer.SetFloat(bgmVolumeTrigger, volume);
	}

	/// <summary>
	/// 设置音效音量
	/// </summary>
	public void SetSFXVolume()
	{
		float volume = 0f;
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.sfxVolume)
			{
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 0)
				{
					volume = -80f;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex > 0) // 声音不是 0 的情况
				{
					volume = -(maxAudioVolumeInterval * ((10 - SettingsLoader.Instance.gameSettingsList[i]._settingListIndex) / 10f));
				}
			}
		}

		globalAudioMixer.SetFloat(sfxVolumeTrigger, volume);
	}

	/// <summary>
	/// 设置系统音效音量
	/// </summary>
	public void SetSystemSFXVolume()
	{
		float volume = 0f;
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.systemSFXVolume)
			{
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex == 0)
				{
					volume = -80f;
				}
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex > 0) // 声音不是 0 的情况
				{
					volume = -(maxAudioVolumeInterval * ((10 - SettingsLoader.Instance.gameSettingsList[i]._settingListIndex) / 10f));
				}
			}
		}

		globalAudioMixer.SetFloat(systemSFXVolumeTrigger, volume);
	}

	#endregion
}
