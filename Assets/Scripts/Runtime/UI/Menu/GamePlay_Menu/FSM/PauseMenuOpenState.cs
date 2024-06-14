using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuOpenState : PauseMenuBaseState
{
	public override void EnterState(PauseMenu pauseMenu, bool firstEnterState)
	{
		pauseMenu.playerController.SwitchState(pauseMenu.playerController.gamePauseState);
		pauseMenu.pauseMenuRoot.SetActive(true);
		AudioListener.pause = true;
		Time.timeScale = 0f;

		pauseMenu.openPauseMenuTotalTime = pauseMenu.openPauseMenuCDTime;
		pauseMenu.CalculateOpenPauseMenuCDTime();

		// �ս�����ͣ�˵��������ӷ����߼�����Ȼ�������ָ����Զ����ʹ�ã�
		ReturnCommand newCommand = new ReturnCommand()
		{
			_hideObj = null,
			_displayObj = null,
			_saveSettingsData = false,
		};

		pauseMenu.returnLastUI_InGame.returnCommandStackInGame.Push(newCommand);

		try
		{
			MenuOperateSound.Instance.PlayMenuOpenSound_InGame(); // ���Ų˵�������Ч
		}
		catch
		{
			Debug.LogWarning("������û����Ч����������������Ϊ�������ǵ������ģ������Ǵ����˵����ؽ����");
		}

		Debug.Log("����ͣ�˵�");
	}

	public override void OnUpdate(PauseMenu pauseMenu)
	{
		// ����ִ�з��أ��͹رյģ��߼�
		pauseMenu.returnLastUI_InGame.ReturnLastMenuUI(); // ʵʱ���ִ�з�����һ�� UI �Ĺ���
	}

}
