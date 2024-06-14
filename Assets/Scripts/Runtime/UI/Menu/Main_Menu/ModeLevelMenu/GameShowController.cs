using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������Ļ��ʾ������
/// </summary>
public class GameShowController : MonoBehaviour
{
	#region ��������ͱ���

	public Stack<GameObject> openedScreenStack = new Stack<GameObject>();

	#endregion

	#region �����Ļ��ʾ

	public void ClearShowImage()
	{
		for (int i = 0; i < openedScreenStack.Count; i ++)
		{
			GameObject lastOpenedScreen = openedScreenStack.Pop();
			lastOpenedScreen.SetActive(false);
		}

		openedScreenStack.Clear();
	}

	#endregion
}
