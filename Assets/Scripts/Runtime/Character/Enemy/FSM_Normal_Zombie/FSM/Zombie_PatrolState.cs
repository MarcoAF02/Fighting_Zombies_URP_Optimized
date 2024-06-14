using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_PatrolState : Zombie_BaseState
{
	public Vector3 currentMoveTarget = Vector3.zero; // ��ǰ���ƶ�����Ŀ���

	public override void EnterState(ZombieController zombieController)
	{
		zombieController.zombieHealth.lastState = this;
		zombieController.navMeshAgent.speed = zombieController.patrolMoveSpeed;
		zombieController.navMeshAgent.angularSpeed = zombieController.patrolAngularSpeed;
		zombieController.totalPatrolTime = zombieController.maxPatrolTime;
		zombieController.zombieAnim.PlayPursuitWalkAnim(false);
		zombieController.zombieAnim.PlayPursuitIdleAnim(false);
		zombieController.zombieAnim.PlayPatrolWalkAnim(true);

		Debug.Log("���� Patrol ״̬");
	}

	public override void OnUpdate(ZombieController zombieController)
	{
		zombieController.totalPatrolTime -= Time.deltaTime;
		currentMoveTarget = zombieController.PatrolMoveToTarget(); // �ƶ���Ŀ��λ��

		zombieController.zombieView.UnlockedEnemyView(); // ������Ұ
		zombieController.zombieView.UnlockedEnemyViewCloseRange();
		zombieController.zombieDoorInteractive.ZombieInteractiveWithDoor();
		zombieController.zombieFootstepSound.GroundMaterialCheck();
		zombieController.zombieFootstepSound.PlayPatrolFootstepSound(); // ��ʬѲ�߽Ų���
		zombieController.zombieRoarSound.PlayPatrolRoarSound(); // ��ʬѲ��ʱ�ĺ��������

		if (zombieController.totalPatrolTime < 0) // ����������ã�Ѳ��ʱ��̫��Ҳ����� Idle ״̬
		{
			zombieController.totalPatrolTime = zombieController.maxPatrolTime;
			zombieController.SetupUnlockedEnemyMode();
		}

		if (Vector3.Distance(zombieController.transform.position, currentMoveTarget) < zombieController.minContactDistance) // �ж��ǲ��ǵ���Ŀ���λ
		{
			zombieController.MarkCurrentPoint(currentMoveTarget); // �ִ�Ŀ���λ����λ���Ϊ��ȥ��
		}

		if (zombieController.zombieBattle.attackTargetTrans != null) // �ҵ�Ŀ��ͽ���׷����ҵ�״̬
		{
			zombieController.SwitchState(zombieController.pursuitState);
		}
	}

}
