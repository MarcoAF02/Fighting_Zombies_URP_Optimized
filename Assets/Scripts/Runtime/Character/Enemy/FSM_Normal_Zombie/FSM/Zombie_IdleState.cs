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
		zombieController.RandomSetIdleMode(); // ���õ��� Idle ״̬

		Debug.Log("���� Idle ״̬");
	}

	public override void OnUpdate(ZombieController zombieController)
	{
		zombieController.totalIdleTime -= Time.deltaTime;
		zombieController.totalStillnessTime -= Time.deltaTime;

		zombieController.zombieView.UnlockedEnemyView(); // ������Ұ
		zombieController.zombieView.UnlockedEnemyViewCloseRange();
		zombieController.zombieRoarSound.PlayIdleRoarSound(); // ���ž�ֹʱ����Ч

		if (zombieController.totalStillnessTime < 0) // �ȴ���ָ��ʱ������ѡ��ֹ�����ͻ�������ģʽ
		{
			zombieController.totalStillnessTime = zombieController.maxStillnessTime;
			zombieController.RandomSetIdleMode();
		}

		if (zombieController.totalIdleTime < 0)
		{
			zombieController.totalIdleTime = zombieController.maxIdleTime;
			zombieController.SetupUnlockedEnemyMode();
		}

		if (zombieController.zombieBattle.attackTargetTrans != null) // �ҵ�Ŀ��ͽ���׷����ҵ�״̬
		{
			zombieController.SwitchState(zombieController.pursuitState);
		}
	}

}
