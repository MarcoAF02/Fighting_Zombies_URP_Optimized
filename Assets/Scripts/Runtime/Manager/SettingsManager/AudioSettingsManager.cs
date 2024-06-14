using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; // AudioMixer ����Ҫ���

public class AudioSettingsManager : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��Ƶ����")]
	[SerializeField] private AudioMixer globalAudioMixer;

	[Header("���Ե������������ֵ��ӳ�����䣩")]
	[SerializeField] private float maxAudioVolumeInterval;

	#region �������� Trigger ������

	[Header("ȫ������")]
	[SerializeField] private string mainVolumeTrigger = "MainVolume";
	[Header("��������")]
	[SerializeField] private string bgmVolumeTrigger = "BGMVolume";
	[Header("��Ч����")]
	[SerializeField] private string sfxVolumeTrigger = "SFXVolume";
	[Header("ϵͳ��Ч����")]
	[SerializeField] private string systemSFXVolumeTrigger = "SystemSFXVolume";

	#endregion

	#endregion

	#region ��Ƶ����

	/// <summary>
	/// ����ȫ������
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
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex > 0) // �������� 0 �����
				{
					volume = -(maxAudioVolumeInterval * ((10 - SettingsLoader.Instance.gameSettingsList[i]._settingListIndex) / 10f));
				}
			}
		}

		globalAudioMixer.SetFloat(mainVolumeTrigger, volume);
	}

	/// <summary>
	/// ���ñ�����������
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
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex > 0) // �������� 0 �����
				{
					volume = -(maxAudioVolumeInterval * ((10 - SettingsLoader.Instance.gameSettingsList[i]._settingListIndex) / 10f));
				}
			}
		}

		globalAudioMixer.SetFloat(bgmVolumeTrigger, volume);
	}

	/// <summary>
	/// ������Ч����
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
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex > 0) // �������� 0 �����
				{
					volume = -(maxAudioVolumeInterval * ((10 - SettingsLoader.Instance.gameSettingsList[i]._settingListIndex) / 10f));
				}
			}
		}

		globalAudioMixer.SetFloat(sfxVolumeTrigger, volume);
	}

	/// <summary>
	/// ����ϵͳ��Ч����
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
				if (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex > 0) // �������� 0 �����
				{
					volume = -(maxAudioVolumeInterval * ((10 - SettingsLoader.Instance.gameSettingsList[i]._settingListIndex) / 10f));
				}
			}
		}

		globalAudioMixer.SetFloat(systemSFXVolumeTrigger, volume);
	}

	#endregion
}
