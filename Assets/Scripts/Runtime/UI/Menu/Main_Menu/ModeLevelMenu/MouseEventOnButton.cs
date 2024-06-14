using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseEventOnButton : MonoBehaviour, IPointerEnterHandler
{
	#region 基本组件和变量

	[Header("需要显示的电脑屏幕背景")]
	[SerializeField] private GameObject screenBG; // 屏幕背景
	[Header("是否需要显示该关卡的完成时间")]
	[SerializeField] private bool showBestTime;

	#endregion

	#region 鼠标事件

	public void OnPointerEnter(PointerEventData eventData)
	{
		// 关闭上一个屏幕显示并打开下一个
		if (MainMenu.Instance.gameShowController.openedScreenStack.Count > 0)
		{
			GameObject lastScreen = MainMenu.Instance.gameShowController.openedScreenStack.Pop();
			lastScreen.SetActive(false);
		}

		screenBG.SetActive(true);
		MainMenu.Instance.gameShowController.openedScreenStack.Push(screenBG);

		if (showBestTime)
		{
			GetComponent<ShowLevelBestTime>().ShowLevelBestTimeOnUI();
		}

		MenuOperateSound.Instance.PlayMouseUpSound(); // 播放音效
	}

	#endregion
}
