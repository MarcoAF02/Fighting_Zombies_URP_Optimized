using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_LostPlayerState : Zombie_BaseState
{
	public override void EnterState(ZombieController zombieController)
	{
		Debug.Log("Ŀ�궪ʧ���л��� Lost Player ״̬");
		zombieController.zombieHealth.lastState = this;
		zombieController.totalLostPlayerTime = zombieController.maxLostPlayerTime;
		zombieController.navMeshAgent.speed = zombieController.pursuitMoveSpeed;
		zombieController.navMeshAgent.angularSpeed = zombieController.pursuitAnglarSpeed;

		zombieController.zombieRoarSound.isShocked = false;
	}

	public override void OnUpdate(ZombieController zombieController)
	{
		zombieController.zombieView.LockedEnemyView();
		zombieController.zombieDoorInteractive.ZombieInteractiveWithDoor();
		zombieController.zombieFootstepSound.GroundMaterialCheck();
		zombieController.zombieFootstepSound.PlayPursuitFootstepSound(); // ��ʬ׷���Ų���

		zombieController.totalLostPlayerTime -= Time.deltaTime;

		if (zombieController.totalLostPlayerTime < 0) // ��ʧ���һ��ʱ��������δ�ҵ���ң��ͽ���δ����״̬
		{
			zombieController.SetupUnlockedEnemyMode();
		}

		if (zombieController.zombieBattle.attackTargetTrans != null)
		{
			zombieController.SwitchState(zombieController.pursuitState);
		}
	}
}
