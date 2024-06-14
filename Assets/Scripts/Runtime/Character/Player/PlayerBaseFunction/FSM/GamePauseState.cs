using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauseState : PlayerBaseState
{
	public override void EnterState(PlayerController playerController)
	{
		Cursor.lockState = CursorLockMode.None; // �������

		// ����ͣ�˵�����ʱ���зǳ����ݵ�ʱ�䲻����׼
		playerController.fixAimActionCurTime = playerController.fixAimActionMaxTime;
	}

	public override void OnUpdate(PlayerController playerController)
	{
		// ��ʱʲôҲ����...
	}

}
