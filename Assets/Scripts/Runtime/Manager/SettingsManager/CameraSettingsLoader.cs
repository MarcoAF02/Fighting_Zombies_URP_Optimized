using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// ��������ö�ȡ
/// </summary>
public class CameraSettingsLoader : MonoBehaviour
{
	#region ��������ͱ���

	[SerializeField] private UniversalAdditionalCameraData _cameraData;

	private Coroutine loadCameraAntiAliasingSettings_IECor;

	#endregion

	#region �����������ں���

	private void Start()
	{
		loadCameraAntiAliasingSettings_IECor = StartCoroutine(LoadCameraAntiAliasingSettings_IE());
	}

	#endregion

	#region ���ö�ȡ����

	// TODO: ����ӳ�ʱ����ʱ��д����
	// ��Ϸ��ʼʱע���Լ���Ӧ�����õĺ���д�� SettingsLoader ����
	private IEnumerator LoadCameraAntiAliasingSettings_IE()
	{
		yield return new WaitForSecondsRealtime(0.1f);

		try
		{
			SettingsLoader.Instance.cameraSettingsLoaderList.Add(this);
		}
		catch
		{
			Debug.LogWarning("������û�����ü���������������Ϊ�������ǵ������ģ������Ǵ����˵����ؽ����");
		}
	}

	/// <summary>
	/// Ϊ�����Ӧ������
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
