using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PauseMenuBaseState
{
	/// <summary>
	/// ����״̬ʱ��Ҫִ�е�����
	/// </summary>
	/// <param name="pauseMenu"></param>
	/// <param name="firstEnterState"></param>
	public abstract void EnterState(PauseMenu pauseMenu, bool firstEnterState);

	/// <summary>
	/// ÿ֡����ʱ��Ҫִ�е�����
	/// </summary>
	/// <param name="pauseMenu"></param>
	public abstract void OnUpdate(PauseMenu pauseMenu);

}
