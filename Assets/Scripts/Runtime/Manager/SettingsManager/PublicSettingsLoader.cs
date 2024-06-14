using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ȫ�ֹ������ü�������ÿ�ν��볡��ʱ�������һ�����ã�
/// </summary>
public class PublicSettingsLoader : MonoBehaviour
{
	#region �����������ں���

	private void Start()
	{
		try
		{
			SettingsLoader.Instance.ApplyAllGraphicsSettings(true);
			SettingsLoader.Instance.ApplyAllAudioSettings(true);
		}
		catch
		{
			Debug.LogWarning("������û�����ü���������������Ϊ�������ǵ������ģ������Ǵ����˵����ؽ����");
		}
	}

	#endregion
}
