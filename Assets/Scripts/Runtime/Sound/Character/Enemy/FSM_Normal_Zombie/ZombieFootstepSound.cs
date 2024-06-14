using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���˽Ų���������Ϸ��Ч
// ���� 3D ��Ч

// ׷����ң���ʧ�����һ�������ĽŲ��������ͽŲ������
// Ѳ��״̬����һ�������ĽŲ��������ͽŲ������

public enum ZombieGroundMaterial
{
	Concrete, // �������ذ�
	ConcreteStaircase, // ������̨��
	Brick, // ʯש
}

public class ZombieFootstepSound : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��ʬ������������")]
	[SerializeField] private ZombieController zombieController;

	[Header("�Ų�����Դ���")]
	[SerializeField] private AudioSource footstepAudioSource;
	[Header("��ʬ������Դ���")]
	[SerializeField] private AudioSource fallDownAudioSource;

	[Header("��ɫ�����⹦��")]
	[SerializeField] private Transform groundCheckPoint;
	[SerializeField] private float groundCheckRadius;

	[Header("��ͬ״̬�Ų����ļ��ʱ��")]
	[Header("Ѳ��ʱ�Ų����ļ��ʱ��")]
	[SerializeField] private float patrolFootstepIntervalTime;
	private float patrolFootstepTotaltime;

	[Header("׷�����ʱ�Ų����ļ��ʱ��")]
	[SerializeField] private float pursuitFootstepIntervalTime;
	private float pursuitFootstepTotaltime;

	[Header("�������� 1 �����ĽŲ�������")]
	[SerializeField] private int attack_1_footstepRate = 1;
	[Header("�������� 2 �����ĽŲ�������")]
	[SerializeField] private int attack_2_footstepRate = 3;
	[Header("����ʱ�����ĽŲ�������")]
	[SerializeField] private int moveback_footstepRate = 3;
	[Header("����ʱ�����ĽŲ�������")]
	[SerializeField] private int dead_footstepRate = 3;
	[Header("�������� 1 �ĽŲ������")]
	[SerializeField] private float attack_1_footstepRateTime = 1;
	[Header("���ں����������������ʱ��ķŴ��ʣ�������ڷŴ��ʵĻ���")]
	[SerializeField] private float attack_1_footstepRateTimeMag = 1;
	[Header("�������� 2 �ĽŲ������")]
	[SerializeField] private float attack_2_footstepRateTime = 1;
	[Header("���ں����������������ʱ��ķŴ��ʣ�������ڷŴ��ʵĻ���")]
	[SerializeField] private float attack_2_footstepRateTimeMag = 1;
	[Header("����ʱ�ĽŲ������")]
	[SerializeField] private float moveback_footstepRateTime = 1;
	[Header("����ʱ�Ų�������ķŴ��ʣ�������ڷŴ��ʵĻ���")]
	[SerializeField] private float moveback_footstepRateTimeMag = 0.8f;
	[Header("����ʱ�ĽŲ������")]
	[SerializeField] private float dead_footstepRateTime;
	[Header("����ʱ�Ų�������ķŴ��ʣ�������ڷŴ��ʵĻ���")]
	[SerializeField] private float dead_footstepRateTimeMag;

	// �Ų�������������׷����Һ�Ѳ�ߵĽŲ�����������
	[Header("��������")]
	[Header("Ѳ��ʱ�ĽŲ�������")]
	[SerializeField] private float patrolFootstepVolume;
	[Header("׷�����ʱ�ĽŲ�������")]
	[SerializeField] private float pursuitFootstepVolume;
	[Header("��������ʱ�ĽŲ�������")]
	[SerializeField] private float attackFootstepVolume;
	[Header("����Ӳֱ������ʱ�ĽŲ�������")]
	[SerializeField] private float deadAndHSFootstepVolume;
	[Header("��ʬ���غ�������������С")]
	[SerializeField] private float fallDownVolume;

	[Header("��ʬ�����󵹵ص������б�")]
	[SerializeField] private List<AudioClip> fallDownAudioClipList = new List<AudioClip>();
	[Header("�������ذ���Ƶ�б�")]
	[SerializeField] private List<AudioClip> footstepAudioList_Concrete = new List<AudioClip>();
	[Header("������̨����Ƶ�б�")]
	[SerializeField] private List<AudioClip> footstepAudioList_ConcreteStaircase = new List<AudioClip>();

	private ZombieGroundMaterial zombieGroundMaterial = ZombieGroundMaterial.Concrete;

	// ʹ���еĴ�����ʱ��
	private int curAttackFootstepRate_1;
	private int curAttackFootstepRate_2;
	private int curMovebackFootstepRate;
	private int curDeadFootstepRate;
	private float curAttackFootstepRateTime_1;
	private float curAttackFootstepRateTime_2;
	private float curMovebackFootstepRateTime;
	private float curDeadFootstepRateTime;

	// Э��
	private Coroutine playAttack_1_Footstep_IECor;
	private Coroutine playAttack_2_Footstep_IECor;
	private Coroutine playHardStraightFootstepSound_IECor;
	private Coroutine playDeadFootstepSound_IECor;

	#endregion

	#region �������ͼ��

	/// <summary>
	/// ִ�е����⣺�Ų������Ź���
	/// </summary>
	public void GroundMaterialCheck()
	{
		RaycastHit hitObj;

		if (Physics.Raycast(groundCheckPoint.position, -groundCheckPoint.up, out hitObj, groundCheckRadius))
		{
			if (hitObj.collider.CompareTag(zombieController.layerAndTagCollection_Enemy.concrete))
			{
				zombieGroundMaterial = ZombieGroundMaterial.Concrete;
				return;
			}
			if (hitObj.collider.CompareTag(zombieController.layerAndTagCollection_Enemy.concreteStaircase))
			{
				zombieGroundMaterial = ZombieGroundMaterial.ConcreteStaircase;
				return;
			}
			if (hitObj.collider.CompareTag(zombieController.layerAndTagCollection_Enemy.brick))
			{
				zombieGroundMaterial = ZombieGroundMaterial.Brick;
				return;
			}
		}

		zombieGroundMaterial = ZombieGroundMaterial.Concrete;
	}

	#endregion

	#region �Ų�����Ч���Ź���

	/// <summary>
	/// Ѳ��ʱ�ĽŲ���
	/// </summary>
	public void PlayPatrolFootstepSound()
	{
		patrolFootstepTotaltime += Time.deltaTime;
		if (patrolFootstepTotaltime > 32768f) patrolFootstepTotaltime = 32768f;

		if (patrolFootstepTotaltime >  patrolFootstepIntervalTime)
		{
			patrolFootstepTotaltime = 0f;
			footstepAudioSource.volume = patrolFootstepVolume;
			PlaySingleFootstepSound();
		}
	}

	/// <summary>
	/// ׷�����ʱ�ĽŲ���
	/// </summary>
	public void PlayPursuitFootstepSound()
	{
		pursuitFootstepTotaltime += Time.deltaTime;
		if (pursuitFootstepTotaltime > 32768f) pursuitFootstepTotaltime = 32768f;
		if (zombieController.zombieBattle.isAttacking) return;
		if (!zombieController.zombieBattle.continuePursuit) return;

		if (pursuitFootstepTotaltime > pursuitFootstepIntervalTime)
		{
			pursuitFootstepTotaltime = 0f;
			footstepAudioSource.volume = pursuitFootstepVolume;
			PlaySingleFootstepSound();
		}
	}

	/// <summary>
	/// ��������Ӳֱ�ĽŲ���������Ӳֱ״̬��˲�䲥�ţ�
	/// </summary>
	public void PlayHardStraightFootstepSound()
	{
		footstepAudioSource.volume = deadAndHSFootstepVolume;
		playHardStraightFootstepSound_IECor = StartCoroutine(PlayHardStraightFootstepSound_IE());
	}

	private IEnumerator PlayHardStraightFootstepSound_IE()
	{
		curMovebackFootstepRate = moveback_footstepRate;
		curMovebackFootstepRateTime = moveback_footstepRateTime;
		
		while (true)
		{
			yield return new WaitForSeconds(curMovebackFootstepRateTime);

			PlaySingleFootstepSound();

			curMovebackFootstepRateTime *= moveback_footstepRateTimeMag;
			curMovebackFootstepRate--;

			if (curMovebackFootstepRate <= 0) yield break;
		}
	}

	/// <summary>
	/// ��������ʱ�ĽŲ���
	/// </summary>
	public void PlayDeadFootstepSound()
	{
		footstepAudioSource.volume = deadAndHSFootstepVolume;
		playDeadFootstepSound_IECor = StartCoroutine(PlayDeadFootstepSound_IE());
	}

	private IEnumerator PlayDeadFootstepSound_IE()
	{
		curDeadFootstepRate = dead_footstepRate;
		curDeadFootstepRateTime = dead_footstepRateTime;

		while (true)
		{
			yield return new WaitForSeconds(curDeadFootstepRateTime);

			PlaySingleFootstepSound();

			curDeadFootstepRateTime *= dead_footstepRateTimeMag;
			curDeadFootstepRate--;

			if (curDeadFootstepRate <= 0)
			{
				PlayFallDownSound();
				yield break;
			}
		}
	}

	/// <summary>
	/// ���ŷ�������ʱ�ĽŲ���
	/// </summary>
	public void PlayAttackFootstep(int attackIndex)
	{
		footstepAudioSource.volume = attackFootstepVolume;

		if (attackIndex == 1)
		{
			if (playAttack_2_Footstep_IECor != null)
			{
				StopCoroutine(playAttack_2_Footstep_IECor);
			}

			playAttack_1_Footstep_IECor = StartCoroutine(PlayAttack_1_Footstep_IE());
		}

		if (attackIndex == 2)
		{
			if (playAttack_1_Footstep_IECor != null)
			{
				StopCoroutine(playAttack_1_Footstep_IECor);
			}

			playAttack_2_Footstep_IECor = StartCoroutine(PlayAttack_2_Footstep_IE());
		}
	}

	// ���Ź�����Ϊ 1 �ĽŲ���
	private IEnumerator PlayAttack_1_Footstep_IE()
	{
		curAttackFootstepRate_1 = attack_1_footstepRate;
		curAttackFootstepRateTime_1 = attack_1_footstepRateTime;

		while (true)
		{
			yield return new WaitForSeconds(curAttackFootstepRateTime_1);

			PlaySingleFootstepSound();

			curAttackFootstepRateTime_1 *= attack_1_footstepRateTimeMag;
			curAttackFootstepRate_1--;

			if (curAttackFootstepRate_1 <= 0) yield break;
		}
	}
	
	// ���Ź�����Ϊ 2 �ĽŲ���
	private IEnumerator PlayAttack_2_Footstep_IE()
	{
		curAttackFootstepRate_2 = attack_2_footstepRate;
		curAttackFootstepRateTime_2 = attack_2_footstepRateTime;

		while (true)
		{
			yield return new WaitForSeconds(curAttackFootstepRateTime_2);

			PlaySingleFootstepSound();

			curAttackFootstepRateTime_2 *= attack_2_footstepRateTimeMag;
			curAttackFootstepRate_2--;

			if (curAttackFootstepRate_2 <= 0) yield break;
		}
	}

	/// <summary>
	/// ��������һ�νŲ���
	/// </summary>
	public void PlaySingleFootstepSound()
	{
		// TODO: ��Ӹ�����ʵĽŲ���

		if (zombieGroundMaterial == ZombieGroundMaterial.Concrete)
		{
			int randomIndex = UnityEngine.Random.Range(0, footstepAudioList_Concrete.Count);
			footstepAudioSource.clip = footstepAudioList_Concrete[randomIndex];
		}
		if (zombieGroundMaterial == ZombieGroundMaterial.ConcreteStaircase)
		{
			int randomIndex = UnityEngine.Random.Range(0, footstepAudioList_ConcreteStaircase.Count);
			footstepAudioSource.clip = footstepAudioList_ConcreteStaircase[randomIndex];
		}

		footstepAudioSource.Play();
	}

	/// <summary>
	/// ��ʬ�������ŵ�����Ч
	/// </summary>
	private void PlayFallDownSound()
	{
		fallDownAudioSource.volume = fallDownVolume;

		int randomIndex = UnityEngine.Random.Range(0, fallDownAudioClipList.Count);
		fallDownAudioSource.clip = fallDownAudioClipList[randomIndex];

		fallDownAudioSource.Play();
	}

	#endregion

}
