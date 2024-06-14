using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ͨ��ʬ���˵Ķ���������
/// </summary>
public class ZombieAnim : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��ʬ����������")]
	[SerializeField] private Animator zombieAnimator;

	#endregion

	#region ��ʬ��ɫ�Ķ�������

	/// <summary>
	/// ���Ŵ��Ų����ͻ������ܵĶ���
	/// </summary>
	/// <param name="isLookAround"></param>
	public void PlayLookAroundAnim(bool isLookAround)
	{
		zombieAnimator.SetBool("isLookAround", isLookAround);
	}

	/// <summary>
	/// ��ʬѲ���ε�ʱ������Ѳ���ε�����
	/// </summary>
	/// <param name="isPatrol"></param>
	public void PlayPatrolWalkAnim(bool isPatrol)
	{
		zombieAnimator.SetBool("isPatrol", isPatrol);
	}

	/// <summary>
	/// ��ʬ׷�����ʱ������׷����ҵĶ���
	/// </summary>
	/// <param name="isPursuit"></param>
	public void PlayPursuitWalkAnim(bool isPursuit)
	{
		zombieAnimator.SetBool("isPursuit", isPursuit);
	}

	/// <summary>
	/// ���� CD δ��ʱ�����ݵز���һ�� Idle ����
	/// </summary>
	/// <param name="isIdle"></param>
	public void PlayPursuitIdleAnim(bool isIdle)
	{
		zombieAnimator.SetBool("pursuit_idle", isIdle);
	}

	/// <summary>
	/// ���ݴ���ı�Ų��Ŷ�Ӧ��ŵĹ���������ע����Ҫ��Ӧ����״̬��
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
	/// ��ʬ��������Ӳֱ״̬ʱ����������˵Ķ���
	/// </summary>
	public void PlayHardStraightAnim(bool hardStraight)
	{
		zombieAnimator.SetBool("hardStraight", hardStraight);
	}

	/// <summary>
	/// ��ʬ����ʱ��������������
	/// </summary>
	/// <param name="isDead"></param>
	public void PlayDeadAnim(bool isDead)
	{
		zombieAnimator.SetBool("isDead", isDead);
	}

	/// <summary>
	/// ���ö���״̬
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
