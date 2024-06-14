using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauseState : PlayerBaseState
{
	public override void EnterState(PlayerController playerController)
	{
		Cursor.lockState = CursorLockMode.None; // 解锁鼠标

		// 从暂停菜单返回时，有非常短暂的时间不能瞄准
		playerController.fixAimActionCurTime = playerController.fixAimActionMaxTime;
	}

	public override void OnUpdate(PlayerController playerController)
	{
		// 暂时什么也不做...
	}

}
