using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 普通僵尸敌人的动画控制器
/// </summary>
public class ZombieAnim : MonoBehaviour
{
	#region 基本组件和变量

	[Header("僵尸动画控制器")]
	[SerializeField] private Animator zombieAnimator;

	#endregion

	#region 僵尸角色的动画更新

	/// <summary>
	/// 播放待着不动和环视四周的动画
	/// </summary>
	/// <param name="isLookAround"></param>
	public void PlayLookAroundAnim(bool isLookAround)
	{
		zombieAnimator.SetBool("isLookAround", isLookAround);
	}

	/// <summary>
	/// 僵尸巡逻游荡时，播放巡逻游荡动画
	/// </summary>
	/// <param name="isPatrol"></param>
	public void PlayPatrolWalkAnim(bool isPatrol)
	{
		zombieAnimator.SetBool("isPatrol", isPatrol);
	}

	/// <summary>
	/// 僵尸追击玩家时，播放追击玩家的动画
	/// </summary>
	/// <param name="isPursuit"></param>
	public void PlayPursuitWalkAnim(bool isPursuit)
	{
		zombieAnimator.SetBool("isPursuit", isPursuit);
	}

	/// <summary>
	/// 攻击 CD 未到时，短暂地播放一次 Idle 动画
	/// </summary>
	/// <param name="isIdle"></param>
	public void PlayPursuitIdleAnim(bool isIdle)
	{
		zombieAnimator.SetBool("pursuit_idle", isIdle);
	}

	/// <summary>
	/// 根据传入的编号播放对应编号的攻击动画，注意编号要对应动画状态机
	/// </summary>
	/// <param name="modeIndex"></param>
	public void PlayAttackAnim(int modeIndex)
	{
		if (modeIndex == 1)
		{
			zombieAnimator.SetTrigger("attack_1");
		}
		if (modeIndex == 2)
		{
			zombieAnimator.SetTrigger("attack_2");
		}
	}

	/// <summary>
	/// 僵尸进入受伤硬直状态时，播放向后退的动画
	/// </summary>
	public void PlayHardStraightAnim(bool hardStraight)
	{
		zombieAnimator.SetBool("hardStraight", hardStraight);
	}

	/// <summary>
	/// 僵尸死亡时，播放死亡动画
	/// </summary>
	/// <param name="isDead"></param>
	public void PlayDeadAnim(bool isDead)
	{
		zombieAnimator.SetBool("isDead", isDead);
	}

	/// <summary>
	/// 重置动画状态
	/// </summary>
	public void ResetAnim()
	{
		zombieAnimator.SetBool("isLookAround", false);
		zombieAnimator.SetBool("isPatrol", false);
		zombieAnimator.SetBool("isPursuit", false);
		zombieAnimator.SetBool("pursuit_idle", false);
		zombieAnimator.SetBool("isDead", false);
		zombieAnimator.SetBool("hardStraight", false);
	}

	#endregion

}
