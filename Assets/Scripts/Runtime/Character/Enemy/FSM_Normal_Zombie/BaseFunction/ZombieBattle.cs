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
/// 僵尸类敌人的战斗控制器，负责僵尸类敌人的锁敌战斗
/// </summary>
public class ZombieBattle : MonoBehaviour
{
	#region 基本组件和变量

	[Header("基本组件和变量")]
	[Header("手动指定该僵尸手中的武器")]
	public ZombieWeaponType zombieWeaponType = ZombieWeaponType.None;

	[Header("僵尸控制器")]
	public ZombieController zombieController;

	[Header("僵尸攻击音效")]
	public ZombieAttackSound zombieAttackSound;

	[Header("追击玩家所使用的变量")]
	[Header("当前找到的攻击目标（玩家）")]
	public Transform attackTargetTrans;

	[Header("发动攻击时，缓慢向前方移动")]
	[Header("缓慢向前方移动的导航位置")]
	[SerializeField] private Transform moveForwardTrans;
	[Header("缓慢向前方移动的速度")]
	[SerializeField] private float moveForwardSpeed;

	[Header("攻击距离")]
	[Tooltip("玩家距离多近时发动攻击")]
	[SerializeField] private float attackRange;
	[Header("玩家进入攻击范围，敌人转身面向玩家的速度")]
	[SerializeField] private float turnAroundSpeed;

	[Header("攻击 CD 时间")]
	[SerializeField] private float attackCDTime;
	private float attackTotalTime;

	[Header("攻击 1 持续多长时间")]
	[SerializeField] private float attack_1_durationTime;
	[Header("攻击 2 持续多长时间")]
	[SerializeField] private float attack_2_durationTime;

	[Header("使用攻击动作 1 和攻击动作 2 的比例")]
	[Range(0, 2000)]
	public float attackModeProportion;

	// ==================== 物理攻击的相关变量 ==================== //

	// TODO: 当前攻击方案实现留下的瑕疵：
	// 1. 攻击射线不是贴着角色动画发射的
	// 2. 多个攻击动作使用同一组射线发射点

	[Header("物理攻击相关变量")]
	[Header("攻击 1 射线的发射点（列表）")]
	[SerializeField] private List<Transform> attack_1_StartTransList;
	[Header("攻击 2 射线的发射点（列表）")]
	[SerializeField] private List<Transform> attack_2_StartTransList;

	[Header("攻击射线的长度")]
	[Tooltip("执行物理攻击时的射线长度")]
	[SerializeField] private float attackDistance;
	[Header("对于后续攻击，攻击射线长度的放大倍率（如果应该放大的话）")]
	[SerializeField] private float distanceMag = 1f;

	[Header("攻击类型 1 的攻击次数")]
	[SerializeField] private int attack_1_rate = 1;
	[Header("攻击类型 2 的攻击次数")]
	[SerializeField] private int attack_2_rate = 2;

	[Header("攻击类型 1 的攻击间隔")]
	[SerializeField] private float attack_1_rateTime = 1;
	[Header("对于后续攻击，攻击间隔时间的放大倍率（如果应该放大的话）")]
	[SerializeField] private float attack_1_rateTimeMag = 1;

	[Header("攻击类型 2 的攻击间隔")]
	[SerializeField] private float attack_2_rateTime = 1;
	[Header("对于后续攻击，攻击间隔时间的放大倍率（如果应该放大的话）")]
	[SerializeField] private float attack_2_rateTimeMag = 1;

	[Header("攻击伤害")]
	[Header("攻击类型 1 的基础攻击伤害")]
	[SerializeField] private float attack_1_damage;
	[Header("对于后续攻击，攻击类型 1 的伤害放大倍率（如果应该放大的话）")]
	[SerializeField] private float attack_1_damageMag;

	[Header("攻击类型 2 的基础攻击伤害")]
	[SerializeField] private float attack_2_damage;
	[Header("对于后续攻击，攻击类型 2 的伤害放大倍率（如果应该放大的话）")]
	[SerializeField] private float attack_2_damageMag;

	private float currentAttackDistance; // 当前使用的攻击距离
	private float currentRate_attack_1;
	private float currentRate_attack_2; // 当前使用的攻击轮数
	private float currentRateTime_attack_1;
	private float currentRateTime_attack_2; // 当前使用的攻击时间间隔
	private float currentAttackDamage; // 当前使用的攻击伤害

	[HideInInspector] public bool isAttacking; // 记录是否在攻击（这个是补丁）
	[HideInInspector] public bool continuePursuit; // 攻击执行完毕后，是否可以继续追击敌人

	// 协程
	private Coroutine performAttack_IECor;
	private Coroutine physicsAttack_IECor;

	#endregion

	#region 基本生命周期函数

	private void Start()
	{
		CheckSomeValueIsItCorrect(); // 检查参数设置的正确性
	}

	private void Update()
	{
		if (GameProgressManager.Instance.CurrentGameProgress == GameProgress.GameFail)
		{
			attackTargetTrans = null; // 无论任何时候，只要游戏失败了就清空攻击目标
		}
	}

	#endregion

	#region 锁敌状态下的导航寻路和战斗

	/// <summary>
	/// 持续追击玩家，当靠近到一定距离内时站住不动并发动攻击
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
	/// 实际地执行攻击
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

	// 年前的时候，开发到了这里。
	// 请从这里继续
	// 完成以后，记得将玩家近战武器的攻击射线补充完整。玩家教程提示的射线 BUG 需要修复

	// 2024/2/27：我做完啦

	/// <summary>
	/// 物理攻击：实际对玩家造成伤害
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

				// 发射攻击射线
				EmitAttackRay(attackType, currentAttackDistance, currentAttackDamage);

				zombieAttackSound.PlayAttackSound(); // 播放攻击音效

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

				// 发射攻击射线
				EmitAttackRay(attackType, currentAttackDistance, currentAttackDamage);

				zombieAttackSound.PlayAttackSound(); // 播放攻击音效

				currentAttackDistance *= distanceMag;
				currentRateTime_attack_2 *= attack_2_rateTimeMag;
				currentAttackDamage *= attack_2_damageMag;
				currentRate_attack_2--;

				if (currentRate_attack_2 <= 0) yield break;
			}
		}
	}

	/// <summary>
	/// 发射攻击射线
	/// </summary>
	private void EmitAttackRay(int attackType, float curAtkDis, float curAtkDamage)
	{
		if (attackType == 1)
		{
			// 对于每个射线发射点都发射一条射线
			for (int i = 0; i < attack_1_StartTransList.Count; i++)
			{
				RaycastHit hitObj;
				int playerLayerIndex = 1 << zombieController.layerAndTagCollection_Enemy.playerLayerIndex;

				if (Physics.Raycast(attack_1_StartTransList[i].position, attack_1_StartTransList[i].forward, out hitObj, curAtkDis, playerLayerIndex))
				{
					hitObj.collider.GetComponentInChildren<PlayerHealth>()?.TakeDamage(curAtkDamage);
					return; // 击中后直接返回，不再计算后续
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
					return; // 击中后直接返回，不再计算后续
				}
			}
		}
	}

	/// <summary>
	/// 停止所有攻击行为
	/// </summary>
	public void StopAllAttackBehaviour()
	{
		StopAllCoroutines();
	}

	#endregion

	#region DEBUG 用函数

	/// <summary>
	/// 检查一些参数的设置是否是正确的
	/// </summary>
	private void CheckSomeValueIsItCorrect()
	{
		if (attack_1_durationTime >= attack_2_durationTime)
		{
			Debug.LogWarning("错误，攻击类型 1（轻击）所持续的时间不能超过攻击类型 2（重击）");
		}

		if (attackCDTime <= attack_1_durationTime || attackCDTime <= attack_2_durationTime)
		{
			Debug.LogWarning("错误，攻击 CD 时间必须大于所有攻击所需时间。请重新调整参数");
		}
	}

	#endregion

}
