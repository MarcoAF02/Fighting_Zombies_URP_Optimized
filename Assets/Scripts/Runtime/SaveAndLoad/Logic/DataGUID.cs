using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// GUID �����壺��֤����浵��Ψһ�ԣ����ֵ�����Ϊ���Ĵ��ڣ�ȡ���浵���ݵ�ʱ���ܶ�Ӧ�ϳ����е���Ʒ��

[ExecuteAlways]
public class DataGUID : MonoBehaviour
{
	public string guid;

	private void Awake()
	{
		if (guid == string.Empty)
		{
			guid = Guid.NewGuid().ToString(); // ����һ���� GUID
		}
	}

}
