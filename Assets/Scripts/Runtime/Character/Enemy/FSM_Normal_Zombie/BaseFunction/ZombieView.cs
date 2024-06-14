using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 僵尸视野，负责管理僵尸视野的更新变化
/// </summary>
public class ZombieView : MonoBehaviour
{
	#region 基本组件和变量

	[Header("基本组件和变量")]
	[Header("僵尸控制器")]
	public ZombieController zombieController;

	[Header("视野相关变量")]
	[Header("视线发射的位置")]
	[SerializeField] private Transform sightStartTrans;

	[Header("[-1, 1] 的范围对应角度制 [0, 360]")]
	[Header("静止不动时的视野距离")]
	[Range(1, 320)]
	[SerializeField] private float stillnessSightRadius;
	[Header("静止不动时的视野角度")]
	[Range(-1, 1)]
	[SerializeField] private float stillnessSightAngle;
	[Header("四下环视时的视野距离")]
	[Range(1, 320)]
	[SerializeField] private float lookAroundSightRadius;
	[Header("四下环视时的视野角度")]
	[Range(-1, 1)]
	[SerializeField] private float lookAroundSightAngle;
	[Header("巡逻时的视野距离")]
	[Range(1, 320)]
	[SerializeField] private float patrolSightRadius;
	[Header("巡逻时的视野角度")]
	[Range(-1, 1)]
	[SerializeField] private float patrolSightAngle;
	[Header("锁敌时的视野距离")]
	[Range(1, 320)]
	[SerializeField] private float pursuitSightRadius;
	[Header("始终追杀玩家模式下的视野距离")]
	[Range(120, 8000)]
	[SerializeField] private float alwaysPursuitSightRadius;
	[Header("始终追杀玩家模式下的视野角度")]
	[Range(-1, 1)]
	[SerializeField] private float alwaysPursuitSightAngle;

	private float currentSightRadius; // 当前的视野范围
	private float currentSightAngle; // 当前的视野角度

	[Header("玩家能悄悄接近的最小距离")]
	[Tooltip("玩家只要进入这个距离无论怎么躲藏都会被发现")]
	[Range(1, 160)]
	[SerializeField] private float minSightRadius;

	// ==================== 调试用的变量 ==================== //

	private Vector3 viewDir = Vector3.zero;

	#endregion

	#region 被动视线感知用变量

	[Header("强硬锁定玩家的持续时间")]
	[Tooltip("玩家攻击僵尸后会启用这个时间，该时间归零之前僵尸会一直锁定并跟踪玩家，该值越大，游戏难度越高")]
	[SerializeField] private float maxLockPlayerTime;
	private float lockPlayerTotalTime;

	#endregion

	#region 基本生命周期函数

	private void Start()
	{
		lockPlayerTotalTime = 0;
	}

	private void Update()
	{
		lockPlayerTotalTime -= Time.deltaTime;
		if (lockPlayerTotalTime <= 0) lockPlayerTotalTime = 0;
	}

	#endregion

	#region 视线感知功能

	/// <summary>
	/// 直接锁定玩家
	/// </summary>
	/// <param name="_playerTrans"></param>
	public void DirectLockPlayer(Transform _playerTrans)
	{
		lockPlayerTotalTime = maxLockPlayerTime;
		zombieController.zombieBattle.attackTargetTrans = _playerTrans;
	}

	/// <summary>
	/// 未锁敌时，在标准距离下的视线感知
	/// </summary>
	public void UnlockedEnemyView()
	{
		if (zombieController.currentStateEnum == CurrentStateEnum.IdleStillness)
		{
			currentSightRadius = stillnessSightRadius;
			currentSightAngle = stillnessSightAngle;
		}
		if (zombieController.currentStateEnum == CurrentStateEnum.IdleLookAround)
		{
			currentSightRadius = lookAroundSightRadius;
			currentSightAngle = lookAroundSightAngle;
		}
		if (zombieController.currentStateEnum == CurrentStateEnum.Patrol)
		{
			currentSightRadius = patrolSightRadius;
			currentSightAngle = patrolSightAngle;
		}
		if (zombieController.currentStateEnum == CurrentStateEnum.AlwaysPursuit)
		{
			currentSightRadius = alwaysPursuitSightRadius;
			currentSightAngle = alwaysPursuitSightAngle;

			// 主动找一下玩家
			if (GameProgressManager.Instance.playerController != null)
			{
				zombieController.zombieBattle.attackTargetTrans = GameProgressManager.Instance.playerController.transform;
			}
			else
			{
				Debug.LogWarning("当前游戏场景未注册玩家");
			}
		}

		if (zombieController.currentStateEnum == CurrentStateEnum.AlwaysPursuit) return;

		// Debug.Log(zombieController.currentStateEnum);

		Collider[] playerColliders = Physics.OverlapSphere(sightStartTrans.position, currentSightRadius, 1 << zombieController.layerAndTagCollection_Enemy.playerLayerIndex);

		if (playerColliders.Length == 0)
		{
			if (lockPlayerTotalTime <= 0)
			{
				zombieController.zombieBattle.attackTargetTrans = null;
			}
		}
		if (playerColliders.Length == 1) // 对目标发射直线，看看是否能击中
		{
			GameObject viewTarget = playerColliders[0].GetComponent<PlayerController>().enemyViewTarget;
			Vector3 targetDir = (viewTarget.transform.position - sightStartTrans.position).normalized;
			viewDir = targetDir; // 调试用变量

			int ignoreLayerIndex = (1 << zombieController.layerAndTagCollection_Enemy.airWallLayerIndex) | (1 << zombieController.layerAndTagCollection_Enemy.doorInteractiveLayerIndex) | (1 << zombieController.layerAndTagCollection_Enemy.ignoreNavMeshLayerIndex); // 忽略的碰撞
			ignoreLayerIndex = ~(ignoreLayerIndex);

			RaycastHit hitObj;

			if (Physics.Raycast(sightStartTrans.position, Vector3.Normalize(targetDir), out hitObj, currentSightRadius, ignoreLayerIndex))
			{
				if (hitObj.collider.CompareTag("Player")) // 用向量点乘检查玩家是否在指定的视野角度内
				{
					if (Vector3.Dot(sightStartTrans.forward, targetDir) > currentSightAngle)
					{
						// Debug.Log("看见玩家了");
						zombieController.zombieBattle.attackTargetTrans = playerColliders[0].transform;
					}
					else
					{
						// Debug.Log("进入范围但是没看见玩家");
						if (lockPlayerTotalTime <= 0)
						{
							zombieController.zombieBattle.attackTargetTrans = null;
						}
					}
				}
			}
		}

		if (playerColliders.Length > 1)
		{
			zombieController.zombieBattle.attackTargetTrans = null;
			Debug.LogWarning("这个游戏目前是单机游戏，不应该检测到多名玩家");
		}
	}

	/// <summary>
	/// 在近距离范围内，敌人一定会发现玩家
	/// </summary>
	public void UnlockedEnemyViewCloseRange()
	{
		Collider[] playerColliders = Physics.OverlapSphere(sightStartTrans.position, minSightRadius, 1 << zombieController.layerAndTagCollection_Enemy.playerLayerIndex);

		if (playerColliders.Length == 1)
		{
			Debug.Log("玩家进入最小接近距离");
			zombieController.zombieBattle.attackTargetTrans = playerColliders[0].transform;
		}
		if (playerColliders.Length > 1)
		{
			zombieController.zombieBattle.attackTargetTrans = null;
			Debug.LogWarning("这个游戏目前是单机游戏，不应该检测到多名玩家");
		}
	}

	/// <summary>
	/// 锁敌时，始终检测标准视线范围内的玩家
	/// </summary>
	public void LockedEnemyView()
	{
		if (zombieController.currentStateEnum == CurrentStateEnum.AlwaysPursuit)
		{
			// 主动找一下玩家
			if (GameProgressManager.Instance.playerController != null)
			{
				zombieController.zombieBattle.attackTargetTrans = GameProgressManager.Instance.playerController.transform;
			}
			else
			{
				Debug.LogWarning("当前游戏场景未注册玩家");
				zombieController.zombieBattle.attackTargetTrans = null;
			}
		}
		else
		{
			Collider[] playerColliders = Physics.OverlapSphere(sightStartTrans.position, currentSightRadius, 1 << zombieController.layerAndTagCollection_Enemy.playerLayerIndex);

			if (playerColliders.Length == 0) // 玩家脱离视野，目标设置为空
			{
				if (lockPlayerTotalTime <= 0)
				{
					zombieController.zombieBattle.attackTargetTrans = null;
				}
			}
			if (playerColliders.Length == 1)
			{
				// Debug.Log("玩家在追击视野范围内");
				zombieController.zombieBattle.attackTargetTrans = playerColliders[0].transform;
			}
			if (playerColliders.Length > 1)
			{
				zombieController.zombieBattle.attackTargetTrans = null;
				Debug.LogWarning("这个游戏目前是单机游戏，不应该检测到多名玩家");
			}
		}
	}

	#endregion

	#region DEBUG 用函数

	/// <summary>
	/// 画出所有的射线检测
	/// </summary>
	private void OnDrawGizmosSelected()
	{
		Debug.DrawRay(sightStartTrans.position, viewDir * currentSightRadius, Color.green);

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(sightStartTrans.position, currentSightRadius);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(sightStartTrans.position, minSightRadius);
	}

	#endregion
}
