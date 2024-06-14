using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 僵尸敌人的状态机
/// </summary>
public abstract class Zombie_BaseState
{
	public abstract void EnterState(ZombieController zombieController); // 进入状态时需要执行的程序

	public abstract void OnUpdate(ZombieController zombieController); // 进入状态后，每帧更新时需要执行的程序
}
