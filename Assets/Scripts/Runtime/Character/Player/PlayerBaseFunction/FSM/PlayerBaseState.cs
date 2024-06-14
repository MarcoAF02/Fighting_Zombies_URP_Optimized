using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState
{
	/// <summary>
	/// ����״̬ʱ��Ҫִ�е�����
	/// </summary>
	/// <param name="playerController"></param>
	/// <param name="cameraController"></param>
	public abstract void EnterState(PlayerController playerController);

	/// <summary>
	/// ��״̬�ڣ�ÿ֡������Ҫִ�е�����
	/// </summary>
	/// <param name="playerController"></param>
	/// <param name="cameraController"></param>
	public abstract void OnUpdate(PlayerController playerController);

}
