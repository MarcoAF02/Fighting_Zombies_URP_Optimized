using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState
{
	/// <summary>
	/// 进入状态时需要执行的内容
	/// </summary>
	/// <param name="playerController"></param>
	/// <param name="cameraController"></param>
	public abstract void EnterState(PlayerController playerController);

	/// <summary>
	/// 在状态内，每帧更新需要执行的内容
	/// </summary>
	/// <param name="playerController"></param>
	/// <param name="cameraController"></param>
	public abstract void OnUpdate(PlayerController playerController);

}
