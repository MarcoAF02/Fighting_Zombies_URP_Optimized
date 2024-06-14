using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_LostPlayerState : Zombie_BaseState
{
	public override void EnterState(ZombieController zombieController)
	{
		Debug.Log("目标丢失，切换至 Lost Player 状态");
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
		zombieController.zombieFootstepSound.PlayPursuitFootstepSound(); // 僵尸追击脚步声

		zombieController.totalLostPlayerTime -= Time.deltaTime;

		if (zombieController.totalLostPlayerTime < 0) // 丢失玩家一段时间后，如果仍未找到玩家，就进入未锁敌状态
		{
			zombieController.SetupUnlockedEnemyMode();
		}

		if (zombieController.zombieBattle.attackTargetTrans != null)
		{
			zombieController.SwitchState(zombieController.pursuitState);
		}
	}
}
