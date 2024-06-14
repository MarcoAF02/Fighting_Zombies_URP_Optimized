using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ö�٣���¼��ǰ������ò˵���ʲô�ط�
/// </summary>
public enum SettingsMenuState
{
	None,
	MainMenu,
	InGamePlay
}

/// <summary>
/// ���ò˵���������
/// </summary>
public class SettingsMenu : MonoBehaviour
{
	#region ��������ͱ���

	public SettingsMenuState menuState = SettingsMenuState.None;

	[Header("���ø��˵�������")]
	public GameObject settingsSortMaskImage;
	[Header("������ϸ�˵�������")]
	public GameObject settingsDetailsMaskImage;

	[Header("��Ϸ�����Ӳ˵�")]
	[SerializeField] private GameObject gameSettingsMenu;
	[Header("ͼ�������Ӳ˵�")]
	[SerializeField] private GameObject graphicsSettingsMenu;
	[Header("��Ƶ�����Ӳ˵�")]
	[SerializeField] private GameObject audioSettingsMenu;

	[Header("��������ϸ�ڲ˵�������")]
	[SerializeField] private List<GameObject> allDetailsMenuList = new List<GameObject>();

	#endregion

	#region ��ť����¼��������Ӳ˵�����ʾ

	// ��ʾ��Ϸ�����Ӳ˵�
	public void DisplayGameSettingDetailsMenu()
	{
		HideAllDetailsMenu();

		if (menuState == SettingsMenuState.None)
		{
			Debug.LogWarning("û�����úò˵���״̬��������Ϸ����");
		}

		if (menuState == SettingsMenuState.MainMenu)
		{
			SetReturnCommand_MainMenuState();
		}

		if (menuState == SettingsMenuState.InGamePlay)
		{
			SetReturnCommand_InGame();
		}

		settingsSortMaskImage.SetActive(true);
		gameSettingsMenu.SetActive(true);
		settingsDetailsMaskImage.SetActive(false);

		try
		{
			MenuOperateSound.Instance.PlayCheckSound(); // ����ȷ����Ч
		}
		catch
		{
			Debug.LogWarning("������û����Ч����������������Ϊ�������ǵ������ģ������Ǵ����˵����ؽ����");
		}
	}

	// ��ʾͼ�������Ӳ˵�
	public void DisplayGraphicsSettingsDetailsMenu()
	{
		HideAllDetailsMenu();

		if (menuState == SettingsMenuState.None)
		{
			Debug.LogWarning("û�����úò˵���״̬��������Ϸ����");
		}

		if (menuState == SettingsMenuState.MainMenu)
		{
			SetReturnCommand_MainMenuState();
		}

		if (menuState == SettingsMenuState.InGamePlay)
		{
			SetReturnCommand_InGame();
		}

		settingsSortMaskImage.SetActive(true);
		graphicsSettingsMenu.SetActive(true);
		settingsDetailsMaskImage.SetActive(false);

		try
		{
			MenuOperateSound.Instance.PlayCheckSound(); // ����ȷ����Ч
		}
		catch
		{
			Debug.LogWarning("������û����Ч����������������Ϊ�������ǵ������ģ������Ǵ����˵����ؽ����");
		}
	}

	// ��ʾ��Ƶ�����Ӳ˵�
	public void DisplayAudioSettingsDetailsMenu()
	{
		HideAllDetailsMenu();

		if (menuState == SettingsMenuState.None)
		{
			Debug.LogWarning("û�����úò˵���״̬��������Ϸ����");
		}

		if (menuState == SettingsMenuState.MainMenu)
		{
			SetReturnCommand_MainMenuState();
		}

		if (menuState == SettingsMenuState.InGamePlay)
		{
			SetReturnCommand_InGame();
		}

		settingsSortMaskImage.SetActive(true);
		audioSettingsMenu.SetActive(true);
		settingsDetailsMaskImage.SetActive(false);

		try
		{
			MenuOperateSound.Instance.PlayCheckSound(); // ������Ч
		}
		catch
		{
			Debug.LogWarning("������û����Ч����������������Ϊ�������ǵ������ģ������Ǵ����˵����ؽ����");
		}
	}

	#endregion

	#region ��������ϸ�� UI�����淵��ָ��

	// �������в˵���ʾ
	private void HideAllDetailsMenu()
	{
		for (int i = 0; i < allDetailsMenuList.Count; i ++)
		{
			allDetailsMenuList[i].gameObject.SetActive(false);
		}
	}

	// ����˵�����ָ���Ϸ�ڣ�
	private void SetReturnCommand_InGame()
	{
		ReturnCommand newCommand = new ReturnCommand()
		{
			_hideObj = settingsSortMaskImage,
			_displayObj = settingsDetailsMaskImage,
			_saveSettingsData = true, // ����������Ϣ
		};

		Transform playMenuCanvas = transform.parent; // �õ���Ҳ˵� Canvas ������
		PauseMenu pauseMenu = playMenuCanvas.GetComponentInChildren<PauseMenu>();

		pauseMenu.returnLastUI_InGame.returnCommandStackInGame.Push(newCommand);
	}

	// ����˵�����ָ����˵���
	private void SetReturnCommand_MainMenuState()
	{
		ReturnCommand newCommand = new ReturnCommand()
		{
			_hideObj = settingsSortMaskImage,
			_displayObj = settingsDetailsMaskImage,
			_saveSettingsData = true, // ����������Ϣ
		};

		MainMenu.Instance.returnCommandStack.Push(newCommand);
	}

	#endregion

}
