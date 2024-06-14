using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_PatrolState : Zombie_BaseState
{
	public Vector3 currentMoveTarget = Vector3.zero; // 当前想移动到的目标点

	public override void EnterState(ZombieController zombieController)
	{
		zombieController.zombieHealth.lastState = this;
		zombieController.navMeshAgent.speed = zombieController.patrolMoveSpeed;
		zombieController.navMeshAgent.angularSpeed = zombieController.patrolAngularSpeed;
		zombieController.totalPatrolTime = zombieController.maxPatrolTime;
		zombieController.zombieAnim.PlayPursuitWalkAnim(false);
		zombieController.zombieAnim.PlayPursuitIdleAnim(false);
		zombieController.zombieAnim.PlayPatrolWalkAnim(true);

		Debug.Log("进入 Patrol 状态");
	}

	public override void OnUpdate(ZombieController zombieController)
	{
		zombieController.totalPatrolTime -= Time.deltaTime;
		currentMoveTarget = zombieController.PatrolMoveToTarget(); // 移动到目标位置

		zombieController.zombieView.UnlockedEnemyView(); // 更新视野
		zombieController.zombieView.UnlockedEnemyViewCloseRange();
		zombieController.zombieDoorInteractive.ZombieInteractiveWithDoor();
		zombieController.zombieFootstepSound.GroundMaterialCheck();
		zombieController.zombieFootstepSound.PlayPatrolFootstepSound(); // 僵尸巡逻脚步声
		zombieController.zombieRoarSound.PlayPatrolRoarSound(); // 僵尸巡逻时的吼叫声播放

		if (zombieController.totalPatrolTime < 0) // 根据面板设置，巡逻时间太长也会进入 Idle 状态
		{
			zombieController.totalPatrolTime = zombieController.maxPatrolTime;
			zombieController.SetupUnlockedEnemyMode();
		}

		if (Vector3.Distance(zombieController.transform.position, currentMoveTarget) < zombieController.minContactDistance) // 判断是不是到了目标点位
		{
			zombieController.MarkCurrentPoint(currentMoveTarget); // 抵达目标点位，点位标记为已去过
		}

		if (zombieController.zombieBattle.attackTargetTrans != null) // 找到目标就进入追击玩家的状态
		{
			zombieController.SwitchState(zombieController.pursuitState);
		}
	}

}
