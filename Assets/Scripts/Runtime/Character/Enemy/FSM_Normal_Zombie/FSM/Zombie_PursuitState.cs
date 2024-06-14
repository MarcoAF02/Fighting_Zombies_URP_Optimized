using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_PursuitState : Zombie_BaseState
{
	public override void EnterState(ZombieController zombieController)
	{
		zombieController.zombieHealth.lastState = this;
		zombieController.zombieBattle.continuePursuit = true;
		zombieController.zombieAnim.PlayPursuitWalkAnim(true);
		zombieController.zombieAnim.PlayPursuitIdleAnim(false);
		zombieController.navMeshAgent.speed = zombieController.pursuitMoveSpeed;
		zombieController.navMeshAgent.angularSpeed = zombieController.pursuitAnglarSpeed;

		zombieController.zombieRoarSound.PlayShockRoarSound();
	}

	public override void OnUpdate(ZombieController zombieController)
	{
		zombieController.zombieView.LockedEnemyView();
		zombieController.zombieDoorInteractive.ZombieInteractiveWithDoor();
		zombieController.zombieFootstepSound.GroundMaterialCheck();
		zombieController.zombieFootstepSound.PlayPursuitFootstepSound(); // ½©Ê¬×·»÷½Å²½Éù

		if (!zombieController.zombieBattle.isAttacking &&
			zombieController.zombieRoarSound.isShocked) // ²»ÊÇ¹¥»÷µÄÊ±ºò²ÅÓÐ×·»÷ºð½ÐÉù
		{
			zombieController.zombieRoarSound.PlayPursuitRoarSound(); // ½©Ê¬×·»÷ºð½ÐÉù
		}

		if (zombieController.zombieBattle.attackTargetTrans != null)
		{
			zombieController.zombieBattle.PursuitPlayer();
		}
		else
		{
			if (zombieController.unlockedEnemyMode != UnlockedEnemyMode.AlwaysPursuit)
			{
				zombieController.SwitchState(zombieController.lostPlayerState);
				return;
			}

			// Debug.Log("Ê¼ÖÕËøµÐ×´Ì¬ÏÂµÄ½©Ê¬ÔÝÊ±Ã»ÓÐÕÒµ½Íæ¼Ò");
		}
	}

}
