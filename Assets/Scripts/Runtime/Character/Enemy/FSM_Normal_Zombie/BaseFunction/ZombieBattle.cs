using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZombieWeaponType
{
	None,
	Axe,
	Kinfe,
	Hanger
}

/// <summary>
/// ��ʬ����˵�ս��������������ʬ����˵�����ս��
/// </summary>
public class ZombieBattle : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��������ͱ���")]
	[Header("�ֶ�ָ���ý�ʬ���е�����")]
	public ZombieWeaponType zombieWeaponType = ZombieWeaponType.None;

	[Header("��ʬ������")]
	public ZombieController zombieController;

	[Header("��ʬ������Ч")]
	public ZombieAttackSound zombieAttackSound;

	[Header("׷�������ʹ�õı���")]
	[Header("��ǰ�ҵ��Ĺ���Ŀ�꣨��ң�")]
	public Transform attackTargetTrans;

	[Header("��������ʱ��������ǰ���ƶ�")]
	[Header("������ǰ���ƶ��ĵ���λ��")]
	[SerializeField] private Transform moveForwardTrans;
	[Header("������ǰ���ƶ����ٶ�")]
	[SerializeField] private float moveForwardSpeed;

	[Header("��������")]
	[Tooltip("��Ҿ�����ʱ��������")]
	[SerializeField] private float attackRange;
	[Header("��ҽ��빥����Χ������ת��������ҵ��ٶ�")]
	[SerializeField] private float turnAroundSpeed;

	[Header("���� CD ʱ��")]
	[SerializeField] private float attackCDTime;
	private float attackTotalTime;

	[Header("���� 1 �����೤ʱ��")]
	[SerializeField] private float attack_1_durationTime;
	[Header("���� 2 �����೤ʱ��")]
	[SerializeField] private float attack_2_durationTime;

	[Header("ʹ�ù������� 1 �͹������� 2 �ı���")]
	[Range(0, 2000)]
	public float attackModeProportion;

	// ==================== ����������ر��� ==================== //

	// TODO: ��ǰ��������ʵ�����µ�覴ã�
	// 1. �������߲������Ž�ɫ���������
	// 2. �����������ʹ��ͬһ�����߷����

	[Header("��������ر���")]
	[Header("���� 1 ���ߵķ���㣨�б�")]
	[SerializeField] private List<Transform> attack_1_StartTransList;
	[Header("���� 2 ���ߵķ���㣨�б�")]
	[SerializeField] private List<Transform> attack_2_StartTransList;

	[Header("�������ߵĳ���")]
	[Tooltip("ִ��������ʱ�����߳���")]
	[SerializeField] private float attackDistance;
	[Header("���ں����������������߳��ȵķŴ��ʣ����Ӧ�÷Ŵ�Ļ���")]
	[SerializeField] private float distanceMag = 1f;

	[Header("�������� 1 �Ĺ�������")]
	[SerializeField] private int attack_1_rate = 1;
	[Header("�������� 2 �Ĺ�������")]
	[SerializeField] private int attack_2_rate = 2;

	[Header("�������� 1 �Ĺ������")]
	[SerializeField] private float attack_1_rateTime = 1;
	[Header("���ں����������������ʱ��ķŴ��ʣ����Ӧ�÷Ŵ�Ļ���")]
	[SerializeField] private float attack_1_rateTimeMag = 1;

	[Header("�������� 2 �Ĺ������")]
	[SerializeField] private float attack_2_rateTime = 1;
	[Header("���ں����������������ʱ��ķŴ��ʣ����Ӧ�÷Ŵ�Ļ���")]
	[SerializeField] private float attack_2_rateTimeMag = 1;

	[Header("�����˺�")]
	[Header("�������� 1 �Ļ��������˺�")]
	[SerializeField] private float attack_1_damage;
	[Header("���ں����������������� 1 ���˺��Ŵ��ʣ����Ӧ�÷Ŵ�Ļ���")]
	[SerializeField] private float attack_1_damageMag;

	[Header("�������� 2 �Ļ��������˺�")]
	[SerializeField] private float attack_2_damage;
	[Header("���ں����������������� 2 ���˺��Ŵ��ʣ����Ӧ�÷Ŵ�Ļ���")]
	[SerializeField] private float attack_2_damageMag;

	private float currentAttackDistance; // ��ǰʹ�õĹ�������
	private float currentRate_attack_1;
	private float currentRate_attack_2; // ��ǰʹ�õĹ�������
	private float currentRateTime_attack_1;
	private float currentRateTime_attack_2; // ��ǰʹ�õĹ���ʱ����
	private float currentAttackDamage; // ��ǰʹ�õĹ����˺�

	[HideInInspector] public bool isAttacking; // ��¼�Ƿ��ڹ���������ǲ�����
	[HideInInspector] public bool continuePursuit; // ����ִ����Ϻ��Ƿ���Լ���׷������

	// Э��
	private Coroutine performAttack_IECor;
	private Coroutine physicsAttack_IECor;

	#endregion

	#region �����������ں���

	private void Start()
	{
		CheckSomeValueIsItCorrect(); // ���������õ���ȷ��
	}

	private void Update()
	{
		if (GameProgressManager.Instance.CurrentGameProgress == GameProgress.GameFail)
		{
			attackTargetTrans = null; // �����κ�ʱ��ֻҪ��Ϸʧ���˾���չ���Ŀ��
		}
	}

	#endregion

	#region ����״̬�µĵ���Ѱ·��ս��

	/// <summary>
	/// ����׷����ң���������һ��������ʱվס��������������
	/// </summary>
	public void PursuitPlayer()
	{
		attackTotalTime -= Time.deltaTime;
		if (attackTotalTime < -16384f) attackTotalTime = -16384f;

		if (Vector3.Distance(transform.position, attackTargetTrans.position) <= attackRange)
		{
			zombieController.navMeshAgent.speed = moveForwardSpeed;
			zombieController.navMeshAgent.SetDestination(moveForwardTrans.position);
			zombieController.zombieAnim.PlayPursuitIdleAnim(true);

			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(attackTargetTrans.position - transform.position), turnAroundSpeed * Time.deltaTime);

			if (attackTotalTime < 0f)
			{
				continuePursuit = false;
				performAttack_IECor = StartCoroutine(PerformAttack());
				attackTotalTime = attackCDTime;
			}
		}
		else
		{
			if (continuePursuit)
			{
				zombieController.navMeshAgent.speed = zombieController.pursuitMoveSpeed;
				zombieController.navMeshAgent.SetDestination(attackTargetTrans.position);
				zombieController.zombieAnim.PlayPursuitIdleAnim(false);
			}
			else
			{
				zombieController.zombieAnim.PlayPursuitIdleAnim(true);
				zombieController.navMeshAgent.speed = moveForwardSpeed;
				zombieController.navMeshAgent.SetDestination(moveForwardTrans.position);
			}
		}
	}

	/// <summary>
	/// ʵ�ʵ�ִ�й���
	/// </summary>
	public IEnumerator PerformAttack()
	{
		int randomIndex = Random.Range(0, 2000);
		isAttacking = true;

		zombieController.zombieRoarSound.PlayAttackRoarSound();

		if (randomIndex < attackModeProportion)
		{
			physicsAttack_IECor = StartCoroutine(PhysicsAttack(1));
			zombieController.zombieAnim.PlayAttackAnim(1);
			zombieController.zombieFootstepSound.PlayAttackFootstep(1);
			yield return new WaitForSeconds(attack_1_durationTime);
			continuePursuit = true;
			isAttacking = false;
		}
		else
		{
			physicsAttack_IECor = StartCoroutine(PhysicsAttack(2));
			zombieController.zombieAnim.PlayAttackAnim(2);
			zombieController.zombieFootstepSound.PlayAttackFootstep(2);
			yield return new WaitForSeconds(attack_2_durationTime);
			continuePursuit = true;
			isAttacking = false;
		}
	}

	// ��ǰ��ʱ�򣬿����������
	// ����������
	// ����Ժ󣬼ǵý���ҽ�ս�����Ĺ������߲�����������ҽ̳���ʾ������ BUG ��Ҫ�޸�

	// 2024/2/27����������

	/// <summary>
	/// ��������ʵ�ʶ��������˺�
	/// </summary>
	public IEnumerator PhysicsAttack(int attackType)
	{
		currentAttackDistance = attackDistance;

		if (attackType == 1)
		{
			currentRateTime_attack_1 = attack_1_rateTime;
			currentRate_attack_1 = attack_1_rate;
			currentAttackDamage = attack_1_damage;

			while (true)
			{
				yield return new WaitForSeconds(currentRateTime_attack_1);

				// ���乥������
				EmitAttackRay(attackType, currentAttackDistance, currentAttackDamage);

				zombieAttackSound.PlayAttackSound(); // ���Ź�����Ч

				currentAttackDistance *= distanceMag;
				currentRateTime_attack_1 *= attack_1_rateTimeMag;
				currentAttackDamage *= attack_1_damageMag;
				currentRate_attack_1--;

				if (currentRate_attack_1 <= 0) yield break;
			}
		}
		if (attackType == 2)
		{
			currentRateTime_attack_2 = attack_2_rateTime;
			currentRate_attack_2 = attack_2_rate;
			currentAttackDamage = attack_2_damage;

			while (true)
			{
				yield return new WaitForSeconds(currentRateTime_attack_2);

				// ���乥������
				EmitAttackRay(attackType, currentAttackDistance, currentAttackDamage);

				zombieAttackSound.PlayAttackSound(); // ���Ź�����Ч

				currentAttackDistance *= distanceMag;
				currentRateTime_attack_2 *= attack_2_rateTimeMag;
				currentAttackDamage *= attack_2_damageMag;
				currentRate_attack_2--;

				if (currentRate_attack_2 <= 0) yield break;
			}
		}
	}

	/// <summary>
	/// ���乥������
	/// </summary>
	private void EmitAttackRay(int attackType, float curAtkDis, float curAtkDamage)
	{
		if (attackType == 1)
		{
			// ����ÿ�����߷���㶼����һ������
			for (int i = 0; i < attack_1_StartTransList.Count; i++)
			{
				RaycastHit hitObj;
				int playerLayerIndex = 1 << zombieController.layerAndTagCollection_Enemy.playerLayerIndex;

				if (Physics.Raycast(attack_1_StartTransList[i].position, attack_1_StartTransList[i].forward, out hitObj, curAtkDis, playerLayerIndex))
				{
					hitObj.collider.GetComponentInChildren<PlayerHealth>()?.TakeDamage(curAtkDamage);
					return; // ���к�ֱ�ӷ��أ����ټ������
				}
			}
		}

		if (attackType == 2)
		{
			for (int i = 0; i < attack_2_StartTransList.Count; i ++)
			{
				RaycastHit hitObj;
				int playerLayerIndex = 1 << zombieController.layerAndTagCollection_Enemy.playerLayerIndex;

				if (Physics.Raycast(attack_2_StartTransList[i].position, attack_2_StartTransList[i].forward, out hitObj, curAtkDis, playerLayerIndex))
				{
					hitObj.collider.GetComponentInChildren<PlayerHealth>()?.TakeDamage(curAtkDamage);
					return; // ���к�ֱ�ӷ��أ����ټ������
				}
			}
		}
	}

	/// <summary>
	/// ֹͣ���й�����Ϊ
	/// </summary>
	public void StopAllAttackBehaviour()
	{
		StopAllCoroutines();
	}

	#endregion

	#region DEBUG �ú���

	/// <summary>
	/// ���һЩ�����������Ƿ�����ȷ��
	/// </summary>
	private void CheckSomeValueIsItCorrect()
	{
		if (attack_1_durationTime >= attack_2_durationTime)
		{
			Debug.LogWarning("���󣬹������� 1���������������ʱ�䲻�ܳ����������� 2���ػ���");
		}

		if (attackCDTime <= attack_1_durationTime || attackCDTime <= attack_2_durationTime)
		{
			Debug.LogWarning("���󣬹��� CD ʱ�����������й�������ʱ�䡣�����µ�������");
		}
	}

	#endregion

}
