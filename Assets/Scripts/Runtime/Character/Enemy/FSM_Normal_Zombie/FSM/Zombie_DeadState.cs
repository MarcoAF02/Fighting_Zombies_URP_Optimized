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
			if (zombieController.canNoticePlayer) // �������ʬ����֪ͨ�����Ϣ��������֪ͨ
			{
				GameProgressManager.Instance.NoticePlayerGameTarget(zombieController.gameTargetTipState);
			}

			GameProgressManager.Instance.AddCurrentKillCount(); // ����ɱ����
			GameProgressManager.Instance.zombieControllerList.Remove(zombieController); // ���Լ��Ӷ������Ƴ�

			GameProgressManager.Instance.eventHandler_GameManager.InvokeKillEnemyEvent(); // ��Ӧɱ���¼�

			zombieController.DestroyItselfWhenDead(); // ����ʱɾ���Լ�
		}
		else
		{
			Debug.LogWarning("��Ϸ���̹����������ڣ����鳡������");
		}
	}

	public override void OnUpdate(ZombieController zombieController)
	{
		zombieController.zombieAnim.PlayDeadAnim(zombieController.zombieHealth.isDead);
	}

}
