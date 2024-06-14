using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全局公共设置加载器（每次进入场景时都会加载一遍设置）
/// </summary>
public class PublicSettingsLoader : MonoBehaviour
{
	#region 基本生命周期函数

	private void Start()
	{
		try
		{
			SettingsLoader.Instance.ApplyAllGraphicsSettings(true);
			SettingsLoader.Instance.ApplyAllAudioSettings(true);
		}
		catch
		{
			Debug.LogWarning("场景中没有设置加载器，可能是因为本场景是点击进入的，而不是从主菜单加载进入的");
		}
	}

	#endregion
}
