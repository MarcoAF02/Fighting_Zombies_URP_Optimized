using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mono 单例类

// 如果其他类有自己的 Awake() 和 OnDestroy()，记得这么写
// private new void Awake()
// {
//		base.Awake(); //这个注意千万不能少
//		//下面写类自己的逻辑
// }
// OnDestroy() 也是一样的

// 游戏对象继承 Singleton 时，要把 游戏对象类 放在 < > 中

/// <summary>
/// 一般 Mono 单例类
/// </summary>
/// <typeparam name="T"></typeparam>
public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
	private static T instance;

	/// <summary>
	/// 返回单例
	/// </summary>
	public static T Instance
	{
		get
		{
			return instance;
		}
	}

	/// <summary>
	/// 初始化单例
	/// </summary>
	protected virtual void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this as T;
		}
	}

	/// <summary>
	/// 结束时销毁单例
	/// </summary>
	protected virtual void OnDestroy() //结束时销毁单例
	{
		if (instance == this)
		{
			instance = null;
		}
	}

}
