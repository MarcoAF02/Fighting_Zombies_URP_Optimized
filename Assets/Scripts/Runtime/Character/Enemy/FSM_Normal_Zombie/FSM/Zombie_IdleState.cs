using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_IdleState : Zombie_BaseState
{
	public override void EnterState(ZombieController zombieController)
	{
		zombieController.zombieHealth.lastState = this;
		zombieController.navMeshAgent.speed = 0f;
		zombieController.navMeshAgent.angularSpeed = zombieController.patrolAngularSpeed;
		zombieController.totalIdleTime = zombieController.maxIdleTime;
		zombieController.totalStillnessTime = zombieController.maxStillnessTime;
		zombieController.zombieAnim.PlayPatrolWalkAnim(false);
		zombieController.zombieAnim.PlayPursuitWalkAnim(false);
		zombieController.RandomSetIdleMode(); // 设置敌人 Idle 状态

		Debug.Log("进入 Idle 状态");
	}

	public override void OnUpdate(ZombieController zombieController)
	{
		zombieController.totalIdleTime -= Time.deltaTime;
		zombieController.totalStillnessTime -= Time.deltaTime;

		zombieController.zombieView.UnlockedEnemyView(); // 更新视野
		zombieController.zombieView.UnlockedEnemyViewCloseRange();
		zombieController.zombieRoarSound.PlayIdleRoarSound(); // 播放静止时的音效

		if (zombieController.totalStillnessTime < 0) // 等待了指定时间后，随机选择静止不动和环视两个模式
		{
			zombieController.totalStillnessTime = zombieController.maxStillnessTime;
			zombieController.RandomSetIdleMode();
		}

		if (zombieController.totalIdleTime < 0)
		{
			zombieController.totalIdleTime = zombieController.maxIdleTime;
			zombieController.SetupUnlockedEnemyMode();
		}

		if (zombieController.zombieBattle.attackTargetTrans != null) // 找到目标就进入追击玩家的状态
		{
			zombieController.SwitchState(zombieController.pursuitState);
		}
	}

}
