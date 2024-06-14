using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseEventOnButton : MonoBehaviour, IPointerEnterHandler
{
	#region ��������ͱ���

	[Header("��Ҫ��ʾ�ĵ�����Ļ����")]
	[SerializeField] private GameObject screenBG; // ��Ļ����
	[Header("�Ƿ���Ҫ��ʾ�ùؿ������ʱ��")]
	[SerializeField] private bool showBestTime;

	#endregion

	#region ����¼�

	public void OnPointerEnter(PointerEventData eventData)
	{
		// �ر���һ����Ļ��ʾ������һ��
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

		MenuOperateSound.Instance.PlayMouseUpSound(); // ������Ч
	}

	#endregion
}
