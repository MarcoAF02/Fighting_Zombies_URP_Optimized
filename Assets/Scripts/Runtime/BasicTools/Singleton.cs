using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例基类：非 MonoBehaviour（饱汉式）
/// </summary>
public class Singleton<T> where T : new()
{
	private static T instance;

	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new T();
			}

			return instance;
		}
	}

}
