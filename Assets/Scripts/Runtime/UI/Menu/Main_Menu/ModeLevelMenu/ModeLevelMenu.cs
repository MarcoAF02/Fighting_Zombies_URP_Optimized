using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ϸģʽ�͹ؿ�ѡ��˵�
/// </summary>
public class ModeLevelMenu : MonoBehaviour
{
	#region ��������ͱ���

	[Header("ģʽѡ��˵�")]
	[SerializeField] private GameObject modeSelectMenu;
	[Header("�ؿ�ѡ��˵�")]
	[SerializeField] private GameObject levelSelectMenu;

	#endregion

	#region ��ť����¼�

	// ������Ϸ�ؿ�ѡ��˵�
	public void EnterGameLevelSelectMenu()
	{
		modeSelectMenu.SetActive(false);
		levelSelectMenu.SetActive(true);

		ReturnCommand newCommand = new ReturnCommand()
		{
			_hideObj = levelSelectMenu,
			_displayObj = modeSelectMenu,
			_saveSettingsData = false,
		};

		MainMenu.Instance.returnCommandStack.Push(newCommand);
		MenuOperateSound.Instance.PlayCheckSound();
	}

	#endregion
}
