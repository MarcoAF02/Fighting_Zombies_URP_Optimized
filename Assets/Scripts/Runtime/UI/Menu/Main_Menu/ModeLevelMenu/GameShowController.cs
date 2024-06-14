using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 电脑屏幕显示控制器
/// </summary>
public class GameShowController : MonoBehaviour
{
	#region 基本组件和变量

	public Stack<GameObject> openedScreenStack = new Stack<GameObject>();

	#endregion

	#region 清空屏幕显示

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
