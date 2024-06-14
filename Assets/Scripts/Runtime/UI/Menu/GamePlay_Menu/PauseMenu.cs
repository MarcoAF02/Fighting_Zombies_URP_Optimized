using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ͣ�˵�
/// </summary>
public class PauseMenu : MonoBehaviour
{
	#region ��������ͱ���

	private FPS_Play_Action fpsPlayAction;

	[Header("��ҽ�ɫ������")]
	public PlayerController playerController;
	[Header("��ͣ�˵�������")]
	public GameObject pauseMenuRoot;
	[Header("���ò˵���������")]
	public GameObject settingsMenu_Root_UI;
	[Header("��ͣѡ��˵�������")]
	public GameObject pauseMenuChooseRoot;
	[Header("ȷ���Ƿ�Ҫ���¿�ʼ�� UI �˵�������")]
	public GameObject checkRestartMenuRoot;
	[Header("ȷ���Ƿ�Ҫ�������˵��� UI �˵�������")]
	public GameObject checkReturnMainMenuRoot;
	[Header("UI ���ع��ܿ�����")]
	public ReturnLastUI_InGame returnLastUI_InGame;

	[Header("��ͣ�˵��򿪵� CD ʱ��")]
	public float openPauseMenuCDTime;
	public float openPauseMenuTotalTime;

	[Header("��ͣ�˵��رյ� CD ʱ��")]
	public float closePauseMenuCDTime;
	public float closePauseMenuTotalTime;

	// ��ͣ�˵�״̬��
	public PauseMenuBaseState pauseMenuCurrentState; // ��ǰ����״̬
	public PauseMenuOpenState pauseMenuOpenState = new PauseMenuOpenState();
	public PauseMenuCloseState pauseMenuCloseState = new PauseMenuCloseState();

	// Э��
	private Coroutine calculateOpenPauseMenuCDTime_IECor;

	#endregion

	#region ״̬���л�����

	public void SwitchState(PauseMenuBaseState _state, bool firstEnterState)
	{
		pauseMenuCurrentState = _state;
		pauseMenuCurrentState.EnterState(this, firstEnterState);
	}

	#endregion

	#region �����������ں���

	private void Awake()
	{
		fpsPlayAction = new FPS_Play_Action();
		fpsPlayAction.Enable();
	}

	private void Start()
	{
		SwitchState(pauseMenuCloseState, true);
	}

	private void Update()
	{
		pauseMenuCurrentState.OnUpdate(this);
	}

	private void OnDestroy()
	{
		fpsPlayAction?.Disable();
	}

	#endregion

	#region ��ͣ�˵��Ĵ���ر�

	/// <summary>
	/// ����ͣ�˵�
	/// </summary>
	public void OpenPauseMenu()
	{
		if (fpsPlayAction.UI_Keyboard_And_Mouse.Player_OpenPauseMenu.WasPressedThisFrame())
		{
			// ���ֻ���ڿ���״̬���ܴ���ͣ�˵���������
			if (playerController.playerCurrentState != playerController.playerControlState) return;
			if (closePauseMenuTotalTime > 0f) return; // CD ʱ��δ��
			SwitchState(pauseMenuOpenState, false);
		}
	}

	// �����߼������ջ��ֻ��һ��ָ���ʾ��Ҫ�˳���ͣ�˵��ˣ���Ҫ�����ж�һ��

	/// <summary>
	/// ֱ�ӹر���ͣ�˵�
	/// </summary>
	public void ClosePauseMenu()
	{
		if (openPauseMenuTotalTime > 0f) return;
		SwitchState(pauseMenuCloseState, false);
	}

	/// <summary>
	/// ������ͣ�˵��򿪵� CD ʱ��
	/// </summary>
	public void CalculateOpenPauseMenuCDTime()
	{
		calculateOpenPauseMenuCDTime_IECor = StartCoroutine(CalculateOpenPauseMenuCDTime_IE());
	}

	private IEnumerator CalculateOpenPauseMenuCDTime_IE()
	{
		yield return new WaitForSecondsRealtime(openPauseMenuCDTime); // ������� timeScale Ӱ��
		openPauseMenuTotalTime = 0f;
	}

	#endregion

	#region ��ť����¼�

	/// <summary>
	/// �����沢�˳���ǰ��Ϸ����ʾȷ�ϲ˵���
	/// </summary>
	public void DisplayCheckReturnMainMenu()
	{
		checkReturnMainMenuRoot.SetActive(true);
		pauseMenuChooseRoot.SetActive(false);

		ReturnCommand newCommand = new ReturnCommand()
		{
			_hideObj = checkReturnMainMenuRoot,
			_displayObj = pauseMenuChooseRoot,
		};

		returnLastUI_InGame.returnCommandStackInGame.Push(newCommand);

		try
		{
			MenuOperateSound.Instance.PlayCheckSound(); // ����ȷ����Ч
		}
		catch
		{
			Debug.LogWarning("������û����Ч����������������Ϊ�������ǵ������ģ������Ǵ����˵����ؽ����");
		}
	}

	/// <summary>
	/// ���¿�ʼ��ǰ��������Ϸ����ʾȷ�ϲ˵���
	/// </summary>
	public void DisplayCheckRestartCurrentScene()
	{
		checkRestartMenuRoot.SetActive(true);
		pauseMenuChooseRoot.SetActive(false);

		ReturnCommand newCommand = new ReturnCommand()
		{
			_hideObj = checkRestartMenuRoot,
			_displayObj = pauseMenuChooseRoot,
		};

		returnLastUI_InGame.returnCommandStackInGame.Push(newCommand);

		try
		{
			MenuOperateSound.Instance.PlayCheckSound(); // ����ȷ����Ч
		}
		catch
		{
			Debug.LogWarning("������û����Ч����������������Ϊ�������ǵ������ģ������Ǵ����˵����ؽ����");
		}
	}

	/// <summary>
	/// ��ť�����ò˵�
	/// </summary>
	public void OpenSettingsMenu()
	{
		settingsMenu_Root_UI.SetActive(true);
		pauseMenuRoot.SetActive(false);

		ReturnCommand newCommand = new ReturnCommand()
		{
			_displayObj = pauseMenuRoot,
			_hideObj = settingsMenu_Root_UI,
		};

		returnLastUI_InGame.returnCommandStackInGame.Push(newCommand);

		try
		{
			MenuOperateSound.Instance.PlayCheckSound(); // ����ȷ����Ч
		}
		catch
		{
			Debug.LogWarning("������û����Ч����������������Ϊ�������ǵ������ģ������Ǵ����˵����ؽ����");
		}
	}

	/// <summary>
	/// ��ť���������һ�� UI
	/// </summary>
	public void ActiveReturnLastUI()
	{
		if (returnLastUI_InGame.returnCommandStackInGame.Count <= 1) // ����ָ���ջ
		{
			// �˵��������壩�򿪺͹رյ���Ч�� FSM ״̬�����ࣩ��
			returnLastUI_InGame.pauseMenu.ClosePauseMenu(); // ֱ�ӹرղ˵�
			return;
		}

		ReturnCommand returnCommand = returnLastUI_InGame.returnCommandStackInGame.Pop();
		returnLastUI_InGame.PerformReturnLastUI(returnCommand);

		try
		{
			MenuOperateSound.Instance.PlayCancelSound(); // ���ŷ�����Ч
		}
		catch
		{
			Debug.LogWarning("������û����Ч����������������Ϊ�������ǵ������ģ������Ǵ����˵����ؽ����");
		}
	}

	#endregion

}
