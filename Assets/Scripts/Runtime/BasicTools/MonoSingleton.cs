using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mono ������

// ������������Լ��� Awake() �� OnDestroy()���ǵ���ôд
// private new void Awake()
// {
//		base.Awake(); //���ע��ǧ������
//		//����д���Լ����߼�
// }
// OnDestroy() Ҳ��һ����

//��Ϸ����̳� Singleton ʱ��Ҫ�� ��Ϸ������ ���� < > ��

/// <summary>
/// һ�� Mono ������
/// </summary>
/// <typeparam name="T"></typeparam>
public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
	private static T instance;

	/// <summary>
	/// ���ص���
	/// </summary>
	public static T Instance
	{
		get
		{
			return instance;
		}
	}

	/// <summary>
	/// ��ʼ������
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
	/// ����ʱ���ٵ���
	/// </summary>
	protected virtual void OnDestroy() //����ʱ���ٵ���
	{
		if (instance == this)
		{
			instance = null;
		}
	}

}
