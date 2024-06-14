using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_DeadState : Zombie_BaseState
{
	public override void EnterState(ZombieController zombieController)
	{
		zombieController.zombieBattle.StopAllAttackBehaviour();

		zombieController.navMeshAgent.speed = 0f;
		zombieController.navMeshAgent.angularSpeed = 0f;
		zombieController.zombieAnim.PlayPatrolWalkAnim(false);
		zombieController.zombieAnim.PlayPursuitWalkAnim(false);

		zombieController.zombieHealth.SetAllCollider(false);

		zombieController.zombieFootstepSound.PlayDeadFootstepSound();
		zombieController.zombieRoarSound.PlayDeadRoarSound();

		if (GameProgressManager.Instance != null)
		{
			if (zombieController.canNoticePlayer) // 如果本僵尸可以通知玩家消息则在这里通知
			{
				GameProgressManager.Instance.NoticePlayerGameTarget(zombieController.gameTargetTipState);
			}

			GameProgressManager.Instance.AddCurrentKillCount(); // 增加杀敌数
			GameProgressManager.Instance.zombieControllerList.Remove(zombieController); // 将自己从队列中移除

			GameProgressManager.Instance.eventHandler_GameManager.InvokeKillEnemyEvent(); // 响应杀敌事件

			zombieController.DestroyItselfWhenDead(); // 死亡时删除自己
		}
		else
		{
			Debug.LogWarning("游戏进程管理器不存在，请检查场景设置");
		}
	}

	public override void OnUpdate(ZombieController zombieController)
	{
		zombieController.zombieAnim.PlayDeadAnim(zombieController.zombieHealth.isDead);
	}

}
