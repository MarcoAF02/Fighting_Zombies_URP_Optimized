using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 注意：Idle 和 Patrol 是独立的两个状态
// 所有状态的起点都是 Idle（由静到动）

/// <summary>
/// 记录该僵尸是前期敌人还是后期敌人
/// </summary>
public enum ProgressState
{
	None,
	EarlyEnemy,
	LaterEnemy
}

/// <summary>
/// 设置僵尸在未锁敌时的行为模式
/// </summary>
public enum UnlockedEnemyMode
{
	None,
	Stand,
	AlwaysPatrol, // 始终巡逻
	RandomPatrol, // 随机地在巡逻和站桩间切换
	AlwaysPursuit, // 永远追击玩家
}

/// <summary>
/// 记录未锁敌时处在什么状态
/// </summary>
public enum CurrentStateEnum
{
	None,
	IdleStillness,
	IdleLookAround,
	Patrol,
	AlwaysPursuit,
}

/// <summary>
/// 僵尸类敌人的控制器：负责管理僵尸移动导航，状态切换和战斗
/// </summary>
[RequireComponent(typeof(ZombieView))]
[RequireComponent(typeof(ZombieBattle))]
[RequireComponent(typeof(ZombieHealth))]
[RequireComponent(typeof(ZombieAnim))]
public class ZombieController : MonoBehaviour
{
	#region 基本组件和变量

	[Header("Layer Tag 信息集合")]
	public LayerAndTagCollection_Enemy layerAndTagCollection_Enemy;

	[Header("本敌人是否会触发玩家提示信息")]
	public bool canNoticePlayer;
	[Header("如果会通知玩家信息，发出的通知属于什么阶段")]
	public GameTargetTipState gameTargetTipState;

	[Header("该敌人死亡时，从地上消失的延迟时间")]
	[SerializeField] private float delayDisappearTime = 12f;

	[Header("基本组件和变量")]
	[Header("导航网格代理组件")]
	public NavMeshAgent navMeshAgent;

	[Header("视野控制器")]
	public ZombieView zombieView;
	[Header("战斗控制器")]
	public ZombieBattle zombieBattle;
	[Header("生命值控制器")]
	public ZombieHealth zombieHealth;
	[Header("与门交互控制器")]
	public ZombieDoorInteractive zombieDoorInteractive;

	[Header("动画控制器")]
	public ZombieAnim zombieAnim;

	[Header("脚步声控制器")]
	public ZombieFootstepSound zombieFootstepSound;
	[Header("吼叫声控制器")]
	public ZombieRoarSound zombieRoarSound;

	[Header("设置该僵尸是前期怪还是后期怪")]
	public ProgressState progressState = ProgressState.None;

	// ==================== 未锁敌时的相关变量 ==================== //

	[Header("设置僵尸未锁敌时的基本行为模式")]
	public UnlockedEnemyMode unlockedEnemyMode = UnlockedEnemyMode.Stand;
	[Header("未锁敌时，巡逻和站桩状态的比例（仅在 UnlockedEnemyMode 设置为 RandomPatrol 时有效）")]
	[Range(0, 2000)]
	public int standStateProportion;

	[Header("Idle 状态下，完全静止和环视四周状态的比例")]
	[Range(0, 2000)]
	public int stillnessProportion;

	[Header("Idle 状态可以持续多久")]
	[Tooltip("最多持续多长时间后，会试图重新选择一次 Idle 或 Patrol 状态")]
	[Range(1, 120)]
	public float maxIdleTime;
	[HideInInspector] public float totalIdleTime;

	[Header("Patrol 状态可以持续多久")]
	[Tooltip("最多持续多长时间后，会试图重新选择一次 Idle 或 Patrol 状态")]
	[Range(1, 120)]
	public float maxPatrolTime;
	[HideInInspector] public float totalPatrolTime;

	[Header("Idle（站桩） 状态下，站着不动的最长时间")]
	[Tooltip("最多持续多长时间后，会试图在完全站着不动和环视周围间切换")]
	[Range(1, 60)]
	public float maxStillnessTime;
	[HideInInspector] public float totalStillnessTime;

	[Header("Lost Player（丢失目标玩家）状态最多可以持续多长时间")]
	[Tooltip("最多持续多长时间后，会退出丢失玩家状态")]
	[Range(1, 30)]
	public float maxLostPlayerTime;
	[HideInInspector] public float totalLostPlayerTime;

	[Header("巡逻状态下，设置僵尸的巡逻点位")]
	[SerializeField] private List<Transform> patrolPointList = new List<Transform>();

	[Header("当前阶段找到的巡逻点位（根据上方设置的巡逻点位自动赋值）")]
	private List<PatrolPointDetail> currentPatrolPath = new List<PatrolPointDetail>();

	[Header("巡逻状态下，僵尸离目标点位有多近时，设置该点位为已到达")]
	public float minContactDistance;

	// ==================== 移动速度和旋转角度相关变量 ==================== //

	[Header("各种状态下的移动速度")]
	[Header("巡逻时的移动速度")]
	public float patrolMoveSpeed;
	[Header("追击玩家时的移动速度")]
	public float pursuitMoveSpeed;

	[Header("各种状态下的旋转速度")]
	[Header("巡逻时的旋转速度")]
	public float patrolAngularSpeed;
	[Header("追击玩家时的旋转速度")]
	public float pursuitAnglarSpeed;

	// ==================== 状态机实际声明 ==================== //

	[HideInInspector] public CurrentStateEnum currentStateEnum = CurrentStateEnum.None;
	public Zombie_BaseState currentState; // 当前所处状态
	public Zombie_IdleState idleState = new Zombie_IdleState();
	public Zombie_PatrolState patrolState = new Zombie_PatrolState();
	public Zombie_PursuitState pursuitState = new Zombie_PursuitState();
	public Zombie_LostPlayerState lostPlayerState = new Zombie_LostPlayerState();
	public Zombie_HardStraightState hardStraightState = new Zombie_HardStraightState();
	public Zombie_DeadState deadState = new Zombie_DeadState();

	#endregion

	#region 角色状态切换

	public void SwitchState(Zombie_BaseState state)
	{
		currentState = state;
		currentState.EnterState(this);
	}

	#endregion

	#region 基本生命周期函数

	private void OnEnable()
	{
		zombieAnim.ResetAnim();
		SetupUnlockedEnemyMode();
	}

	private void Start()
	{
		RegisterMyself();
		SetupUnlockedEnemyMode();
	}

	private void Update()
	{
		currentState.OnUpdate(this);

		// 修复异常旋转
		transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);
	}

	private void OnDisable()
	{
		SetupUnlockedEnemyMode();
		zombieAnim.ResetAnim();
	}

	#endregion

	#region 游戏开始时将自己注册到游戏进程管理器中

	private void RegisterMyself()
	{
		if (GameProgressManager.Instance == null)
		{
			Debug.LogWarning("错误，游戏进程管理器没有被激活或不存在，请检查场景设置");
			return;
		}

		GameProgressManager.Instance.zombieControllerList.Add(this);
	}

	#endregion

	#region 非锁敌状态下的导航寻路

	/// <summary>
	/// 非锁敌状态下设置敌人的 Idle 状态
	/// </summary>
	public void SetupUnlockedEnemyMode()
	{
		if (unlockedEnemyMode == UnlockedEnemyMode.None)
		{
			Debug.LogWarning("未正常设置敌人未锁敌时状态，请检查游戏设计");
			return;
		}
		if (unlockedEnemyMode == UnlockedEnemyMode.Stand)
		{
			SwitchState(idleState);
		}
		if (unlockedEnemyMode == UnlockedEnemyMode.AlwaysPatrol)
		{
			SwitchState(patrolState);
		}
		if (unlockedEnemyMode == UnlockedEnemyMode.RandomPatrol)
		{
			int randomIndex = Random.Range(0, 2000); // 随机数的生成范围： (0, 400]

			if (randomIndex < standStateProportion)
			{
				SwitchState(patrolState);
			}
			else
			{
				SwitchState(idleState);
			}
		}
		if (unlockedEnemyMode == UnlockedEnemyMode.AlwaysPursuit)
		{
			currentStateEnum = CurrentStateEnum.AlwaysPursuit;
			SwitchState(pursuitState);
		}
	}

	// ==================== 原地站桩功能 ==================== //

	public void RandomSetIdleMode()
	{
		int randomIndex = Random.Range(0, 2000);

		if (randomIndex < stillnessProportion)
		{
			currentStateEnum = CurrentStateEnum.IdleStillness; // 记录状态
			zombieAnim.PlayLookAroundAnim(false);
		}
		else
		{
			currentStateEnum = CurrentStateEnum.IdleLookAround; // 记录状态
			zombieAnim.PlayLookAroundAnim(true);
		}
	}

	// ==================== 巡逻功能 ==================== //

	/// <summary>
	/// 设置未锁敌状态下的巡逻路径
	/// </summary>
	public void SetupPatrolPath()
	{
		currentStateEnum = CurrentStateEnum.Patrol; // 记录状态

		// 先清除之前用过的巡逻点位
		currentPatrolPath.Clear();

		// 根据给定好的巡逻点位设置路径
		for (int i = 0; i < patrolPointList.Count; i ++)
		{
			PatrolPointDetail newPoint = new PatrolPointDetail
			{
				trans = patrolPointList[i],
				beenThere = false
			};

			currentPatrolPath.Add(newPoint);
		}
	}

	/// <summary>
	/// 寻找指定点位并驱使其前往
	/// </summary>
	/// <returns></returns>
	public Vector3 PatrolMoveToTarget()
	{
		for (int i = 0; i < currentPatrolPath.Count; i++)
		{
			if (!currentPatrolPath[i].beenThere) // 如果这个点位没去过
			{
				navMeshAgent.SetDestination(currentPatrolPath[i].trans.position);
				return currentPatrolPath[i].trans.position;
			}
		}

		// Debug.Log("所有点位都去过了，需要重新选择一次路径");
		SetupPatrolPath();

		return PatrolMoveToTarget();
	}

	/// <summary>
	/// 标记已到达的点位为：去过
	/// </summary>
	/// <param name="pos"></param>
	public void MarkCurrentPoint(Vector3 pos)
	{
		for (int i = 0; i < currentPatrolPath.Count; i++)
		{
			if (currentPatrolPath[i].trans.position == pos)
			{
				currentPatrolPath[i].beenThere = true;
			}
		}
	}

	#endregion

	#region 死亡时删除自己

	/// <summary>
	/// 死亡时删除自己（带有时间延迟）
	/// </summary>
	public void DestroyItselfWhenDead()
	{
		Destroy(gameObject, delayDisappearTime);
	}

	#endregion

}
