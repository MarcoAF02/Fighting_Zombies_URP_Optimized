using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���˵�������
/// </summary>
public class MainMenu : MonoSingleton<MainMenu>
{
	#region ��������ͱ���

	[Header("���ع��ܿ�����")]
	public ReturnLastUI returnLastUI;

	[Header("�˵���Ϸ����")]
	[Header("���˵�")]
	public GameObject mainMenuRoot;
	[Header("ģʽ�ؿ�ѡ��˵�")]
	public GameObject modeAndLevelRoot;
	[Header("���ò˵�")]
	public GameObject settingsMenuRoot;

	[Header("ģʽ�ؿ�չʾ������")]
	public GameShowController gameShowController;
	[Header("ģʽ�ؿ�չʾ UI ��������")]
	public GameObject gameShowBGObj;

	[Header("���˵���ť")]
	[Header("��Ϸ��ʼ��ť")]
	[SerializeField] private Button gameStartButton;
	[Header("���ð�ť")]
	[SerializeField] private Button settingsButton;
	[Header("�˳���Ϸ��ť")]
	[SerializeField] private Button exitButton;

	// ��¼����ָ���ջ
	public Stack<ReturnCommand> returnCommandStack = new Stack<ReturnCommand>();

	#endregion

	#region �˵��л����ܣ���ť����¼���

	/// <summary>
	/// �������ò˵�
	/// </summary>
	public void EnterSettingsMenu()
	{
		SwitchDisplayUI(mainMenuRoot, settingsMenuRoot);

		ReturnCommand newCommand = new ReturnCommand()
		{
			_hideObj = settingsMenuRoot,
			_displayObj = mainMenuRoot,
			_saveSettingsData = false,
		};

		returnCommandStack.Push(newCommand);
	}

	/// <summary>
	/// ����ģʽ - �ؿ�ѡ��˵�
	/// </summary>
	public void EnterModeAndLevelMenu()
	{
		SwitchDisplayUI(mainMenuRoot, modeAndLevelRoot);
		gameShowBGObj.SetActive(true);

		ReturnCommand newCommand = new ReturnCommand()
		{
			_hideObj = modeAndLevelRoot,
			_displayObj = mainMenuRoot,
			_saveSettingsData = false,
		};

		returnCommandStack.Push(newCommand);
	}

	/// <summary>
	/// �л���ǰ��ʾ�� UI
	/// </summary>
	/// <param name="_hideUI"></param>
	/// <param name="_displayUI"></param>
	public void SwitchDisplayUI(GameObject _hideUI, GameObject _displayUI)
	{
		MenuOperateSound.Instance.PlayCheckSound();
		_hideUI.SetActive(false);
		_displayUI.SetActive(true);
	}

	public void ExitGame()
	{
		Debug.Log("�˳���Ϸ");
		MenuOperateSound.Instance.PlayCheckSound();
		Application.Quit();
	}

	#endregion

}
