using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ��������ҳ��Ŀ��ƽű�
/// </summary>
public class SettingsOperationController : MonoBehaviour
{
	#region ��������ͱ���

	[Header("����������ť")]
	[SerializeField] private Button addButton;
	[SerializeField] private Button reduceButton;

	[Header("������������")]
	[SerializeField] private TextMeshProUGUI settingsDescribe;
	[Header("������ý�ʲô���֣����� SettingsLoader ���Ҳ�Ӧ������Ϸ")]
	[SerializeField] private string thisSettingsName;

	// ѡ�������б�ĺ��壺һ����ֵ��Ӧһ�� UI �ı��������ڱ���
	[Header("����ѡ���б�")]
	[SerializeField] private List<SettingsMessage> thisSettingsList = new List<SettingsMessage>();
	private int index = 0; // ���ڲ���ѡ������

	#endregion

	#region �����������ں���

	private void Start()
	{
		// ��ȡ SettingsLoader ������
		try
		{
			PullDataFromSettingsLoader();
		}
		catch
		{
			Debug.LogWarning("������û�����ü���������������Ϊ�ó������ֶ���������");
			return;
		}

		if (thisSettingsList.Count > 0)
		{
			settingsDescribe.text = thisSettingsList[index]._settingMessage; // ��ȡ��������
		}
		else
		{
			Debug.LogWarning("��ѡ��û�����ú���Ϣ��浵��������");

			index = 0;
			settingsDescribe.text = thisSettingsList[index]._settingMessage; // ��ȡ��������
		}

		addButton.onClick.AddListener(AddSettingsValue);
		reduceButton.onClick.AddListener(ReduceSettingsValue);
	}

	#endregion

	#region ���ò������ܣ���ť����¼���

	// �������õ�ֵ
	private void AddSettingsValue()
	{
		if (index < thisSettingsList.Count - 1 && index >= 0)
		{
			index++;
			settingsDescribe.text = thisSettingsList[index]._settingMessage; // ��ȡ��������

			// DebugSettingsMessage();
		}

		MenuOperateSound.Instance.PlayCheckSound();
		SyncWithSettingsLoader();
	}

	// �������õ�ֵ
	private void ReduceSettingsValue()
	{
		if (index < thisSettingsList.Count && index > 0)
		{
			index--;
			settingsDescribe.text = thisSettingsList[index]._settingMessage; // ��ȡ��������

			// DebugSettingsMessage();
		}

		MenuOperateSound.Instance.PlayCheckSound();
		SyncWithSettingsLoader();
	}

	#endregion

	#region �� SettingsLoader ͨ��

	/// <summary>
	/// �� SettingsLoader ��ȡ����
	/// </summary>
	public void PullDataFromSettingsLoader()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i ++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == thisSettingsName)
			{
				index = SettingsLoader.Instance.gameSettingsList[i]._settingListIndex;
			}
		}
	}

	/// <summary>
	/// �����ú� SettingsLoader ͬ��
	/// </summary>
	public void SyncWithSettingsLoader()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == thisSettingsName)
			{
				SettingsLoader.Instance.gameSettingsList[i]._settingListIndex = index;
			}
		}
	}

	#endregion

	#region DEBUG �ú���

	private void DebugSettingsMessage()
	{
		Debug.Log(index + " " + thisSettingsList[index]._settingIndex + " " +  thisSettingsList[index]._settingMessage);
	}

	#endregion

}
