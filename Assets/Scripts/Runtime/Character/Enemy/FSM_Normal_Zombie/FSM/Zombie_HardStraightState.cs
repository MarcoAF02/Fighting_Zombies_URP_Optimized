using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_HardStraightState : Zombie_BaseState
{
	public override void EnterState(ZombieController zombieController)
	{
		zombieController.zombieBattle.StopAllAttackBehaviour();

		zombieController.navMeshAgent.angularSpeed = 0f;
		zombieController.navMeshAgent.speed = zombieController.zombieHealth.hardStraightBackSpeed;
		zombieController.zombieHealth.getInHardStraightTotalTime = zombieController.zombieHealth.getInHardStraightMaxTime;
		zombieController.zombieAnim.PlayHardStraightAnim(true);

		zombieController.zombieFootstepSound.PlayHardStraightFootstepSound();
		zombieController.zombieRoarSound.PlayHardStraightRoarSound(); // ≤•∑≈“Ù–ß
	}

	public override void OnUpdate(ZombieController zombieController)
	{
		zombieController.zombieHealth.getInHardStraightTotalTime -= Time.deltaTime;

		if (zombieController.zombieHealth.getInHardStraightTotalTime < 0)
		{
			zombieController.zombieHealth.getInHardStraightTotalTime = zombieController.zombieHealth.getInHardStraightMaxTime;
			zombieController.zombieAnim.PlayHardStraightAnim(false);
			zombieController.SwitchState(zombieController.zombieHealth.lastState);
		}

		zombieController.zombieHealth.HardStraightMoveBack();
	}
}
