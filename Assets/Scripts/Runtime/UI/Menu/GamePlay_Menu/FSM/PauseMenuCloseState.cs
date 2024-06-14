using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuCloseState : PauseMenuBaseState
{
	public override void EnterState(PauseMenu pauseMenu, bool firstEnterState)
	{
		if (pauseMenu.playerController.playerLastState != null)
		{
			pauseMenu.playerController.SwitchState(pauseMenu.playerController.playerLastState); // �ص��򿪲˵�֮ǰ��״̬
		}

		pauseMenu.pauseMenuRoot.SetActive(false);
		AudioListener.pause = false;
		Time.timeScale = 1f;

		pauseMenu.closePauseMenuTotalTime = pauseMenu.closePauseMenuCDTime;

		try
		{
			if (!firstEnterState) MenuOperateSound.Instance.PlayMenuCloseSound_InGame(); // ���Ų˵��ر���Ч
		}
		catch
		{
			Debug.LogWarning("������û����Ч����������������Ϊ�������ǵ������ģ������Ǵ����˵����ؽ����");
		}

		Debug.Log("�ر���ͣ�˵�");
	}

	public override void OnUpdate(PauseMenu pauseMenu)
	{
		pauseMenu.closePauseMenuTotalTime -= Time.deltaTime;
		if (pauseMenu.closePauseMenuTotalTime <= 0f) pauseMenu.closePauseMenuTotalTime = 0f;

		pauseMenu.OpenPauseMenu(); // ��ʼ�˵��رպ󣬿��Լ����߼�
	}

}
