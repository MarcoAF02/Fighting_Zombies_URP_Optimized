using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ʬ���˵�״̬��
/// </summary>
public abstract class Zombie_BaseState
{
	public abstract void EnterState(ZombieController zombieController); // ����״̬ʱ��Ҫִ�еĳ���

	public abstract void OnUpdate(ZombieController zombieController); // ����״̬��ÿ֡����ʱ��Ҫִ�еĳ���
}
