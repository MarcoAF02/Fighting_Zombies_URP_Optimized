using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: 如果最后一击是击中头部，开发爆头破碎效果

/// <summary>
/// 僵尸受击受伤功能实现
/// </summary>
public class ZombieHealth : MonoBehaviour
{
	#region 基本组件和变量

	[Header("基本组件和变量")]
	[SerializeField] private ZombieController zombieController;
	[Header("物理碰撞盒")]
	[SerializeField] private Collider physicsCollider;
	[Header("伤害用 HitBox")]
	[SerializeField] private List<Collider> hitBox = new List<Collider>();

	#region 生命值和伤害有关参数

	[Header("生命值和伤害有关参数")]
	[Header("僵尸生命值")]
	[Range(0f, 2000f)]
	[SerializeField] private float maxHealth;
	[SerializeField] private float currentHealth;
	[HideInInspector] public bool isDead;
	private float lastFrameHeath;

	[Header("击中僵尸不同部位，对伤害的放大倍率")]
	[Header("击中头部")]
	[SerializeField] private float headDamageMagRate = 2f;
	[Header("击中躯干")]
	[SerializeField] private float bodyDamageMagRate = 1f;
	[Header("击中四肢")]
	[SerializeField] private float fourLimbsMagRate = 0.5f;

	[Header("僵尸受伤后退时，导航的目标点")]
	[SerializeField] private Transform moveBackTrans;

	[Header("僵尸受击后退时，向后导航的移动速度")]
	public float hardStraightBackSpeed;

	[Header("进入硬直状态的 CD 时间")]
	public float hardStraightCDTime;
	public float hardStraightTotalTime;

	[Header("进入受伤硬直状态的最大持续时间")]
	public float getInHardStraightMaxTime;
	public float getInHardStraightTotalTime; // 进入受伤硬直状态持续了多久
	public Zombie_BaseState lastState; // 记录进入受伤硬直状态前所处的状态

	#endregion

	#region 不同身体部位的 Tag 标签

	[Header("头部")]
	[SerializeField] private string headTag;
	[Header("躯干")]
	[SerializeField] private string bodyTag;
	[Header("四肢")]
	[SerializeField] private string fourLimbsTag;

	#endregion

	#region 被击中进入硬直状态

	// 硬直状态的计算方法：
	// 1. 先将最大生命值缩小一定倍数，作为触发硬直的基础伤害值
	// 2. 随机取一定范围内的随机数，加上相对于上一帧减少的生命值，看看能否超过硬直底线，可以超过就算硬直

	// lastFrameHealth - currentHealth 可以得到伤害减少值

	[Header("用于计算硬直底线的基本倍率")]
	[Range(0, 1)]
	[SerializeField] private float hardStraightMag;
	private float hardStraightValue; // 实际计算得出的硬直底线

	[Header("随机计算的硬直增量范围")]
	[SerializeField] private float maxAddLimit;
	[SerializeField] private float minAddLimit;

	#endregion

	#endregion

	#region 基本生命周期函数

	private void Awake()
	{
		SetupValue();
		SetAllCollider(true);
	}

	private void Update()
	{
		hardStraightTotalTime -= Time.deltaTime;
		if (hardStraightTotalTime <= 0) hardStraightTotalTime = 0f;
	}

	#endregion

	#region 初始化相关数值

	private void SetupValue()
	{
		isDead = false;
		currentHealth = maxHealth;
		lastFrameHeath = currentHealth;
		hardStraightValue = maxHealth * hardStraightMag;
		hardStraightTotalTime = hardStraightCDTime;
	}

	#endregion

	#region 僵尸受击受伤功能

	/// <summary>
	/// 僵尸被击中时掉血，传入的参数是命中部位的 Tag 和玩家武器的伤害值
	/// </summary>
	/// <param name="_hitTag"></param>
	/// <param name="_damage"></param>
	public void TakeDamage(string _hitTag, float _damage)
	{
		lastFrameHeath = currentHealth;

		if (_hitTag == headTag)
		{
			currentHealth = currentHealth - (_damage * headDamageMagRate);
		}
		if (_hitTag == bodyTag)
		{
			currentHealth = currentHealth - (_damage * bodyDamageMagRate);
		}
		if (_hitTag == fourLimbsTag)
		{
			currentHealth = currentHealth - (_damage * fourLimbsMagRate);
		}

		if (hardStraightTotalTime <= 0)
		{
			hardStraightTotalTime = hardStraightCDTime;
			TakeHardStraight();
		}

		if (currentHealth <= 0f) // 进入死亡状态
		{
			currentHealth = 0f;
			isDead = true;
			zombieController.SwitchState(zombieController.deadState);
		}
	}

	private void TakeHardStraight()
	{
		if (zombieController.currentState == zombieController.hardStraightState) return; // 不能重复进入硬直状态

		float totalValue = (lastFrameHeath - currentHealth) + Random.Range(minAddLimit, maxAddLimit);

		if (totalValue > hardStraightValue) // 进入硬直状态
		{
			zombieController.SwitchState(zombieController.hardStraightState);
		}
	}

	/// <summary>
	/// 僵尸受伤硬直后，向自己身后导航移动一段距离并播放受击动画
	/// </summary>
	public void HardStraightMoveBack()
	{
		zombieController.navMeshAgent.SetDestination(moveBackTrans.position);
	}

	#endregion

	#region 设置 HitBox

	/// <summary>
	/// 手动指定敌人身上的 HitBox 是否需要打开
	/// </summary>
	/// <param name="enable"></param>
	public void SetAllCollider(bool enable)
	{
		zombieController.navMeshAgent.enabled = enable;
		physicsCollider.enabled = enable;

		for (int i = 0; i < hitBox.Count; i ++)
		{
			hitBox[i].enabled = enable;
		}
	}

	#endregion

}
