using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PauseMenuBaseState
{
	/// <summary>
	/// 进入状态时需要执行的内容
	/// </summary>
	/// <param name="pauseMenu"></param>
	/// <param name="firstEnterState"></param>
	public abstract void EnterState(PauseMenu pauseMenu, bool firstEnterState);

	/// <summary>
	/// 每帧更新时需要执行的内容
	/// </summary>
	/// <param name="pauseMenu"></param>
	public abstract void OnUpdate(PauseMenu pauseMenu);

}
