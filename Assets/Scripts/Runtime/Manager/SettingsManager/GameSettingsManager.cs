using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsManager : MonoBehaviour
{
	#region ��Ϸ���þ��幦��

	// ע����Ϸ����ÿ�ν���ؿ�ʱд��

	/// <summary>
	/// ��ȡ���������
	/// </summary>
	/// <returns></returns>
	public float LoadMouseSensitivity()
	{
		for (int i = 0; i < SettingsLoader.Instance.gameSettingsList.Count; i++)
		{
			if (SettingsLoader.Instance.gameSettingsList[i]._settingName == SettingsLoader.Instance.allSettingsName.mouseSensitivity)
			{
				// ����ע����������
				return (SettingsLoader.Instance.gameSettingsList[i]._settingListIndex + 1f) / 100f;
			}
		}

		Debug.LogWarning("���õ�ֵû�ж�Ӧ�ϣ����ܳ����˷�Χ");
		return 0.01f;
	}

	// TODO: δ���ĸ��£�׼���� ��״����̬ʮ�֣���̬ʮ��

	/// <summary>
	/// ����׼�ǿ���
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

		Debug.LogWarning("���õ�ֵû�ж�Ӧ�ϣ����ܳ����˷�Χ");
		return true;
	}

	/// <summary>
	/// ��ȡ����� FOV
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

		Debug.LogWarning("���õ�ֵû�ж�Ӧ�ϣ����ܳ����˷�Χ");
		return 60f;
	}

	// TODO: δ���ĸ��£���Ļ���½Ǽ�һ��Ѫ��
	// �������Ӧ���� ��Ļ���ܣ���ͨ������Ļ���ܣ�������������ֵ�� ֮���л�

	/// <summary>
	/// ������Ļ������ʾ����ͨ���Ǽ���
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

		Debug.LogWarning("���õ�ֵû�ж�Ӧ�ϣ����ܳ����˷�Χ");
		return 0;
	}

	/// <summary>
	/// �����ӽ�ҡ��
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

		Debug.LogWarning("���õ�ֵû�ж�Ӧ�ϣ����ܳ����˷�Χ");
		return true;
	}

	/// <summary>
	/// �Ƿ���ʾ�̳���Ϣ
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

		Debug.LogWarning("���õ�ֵû�ж�Ӧ�ϣ����ܳ����˷�Χ");
		return true;
	}

	#endregion
}
