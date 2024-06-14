using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ʬ��Ұ���������ʬ��Ұ�ĸ��±仯
/// </summary>
public class ZombieView : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��������ͱ���")]
	[Header("��ʬ������")]
	public ZombieController zombieController;

	[Header("��Ұ��ر���")]
	[Header("���߷����λ��")]
	[SerializeField] private Transform sightStartTrans;

	[Header("[-1, 1] �ķ�Χ��Ӧ�Ƕ��� [0, 360]")]
	[Header("��ֹ����ʱ����Ұ����")]
	[Range(1, 320)]
	[SerializeField] private float stillnessSightRadius;
	[Header("��ֹ����ʱ����Ұ�Ƕ�")]
	[Range(-1, 1)]
	[SerializeField] private float stillnessSightAngle;
	[Header("���»���ʱ����Ұ����")]
	[Range(1, 320)]
	[SerializeField] private float lookAroundSightRadius;
	[Header("���»���ʱ����Ұ�Ƕ�")]
	[Range(-1, 1)]
	[SerializeField] private float lookAroundSightAngle;
	[Header("Ѳ��ʱ����Ұ����")]
	[Range(1, 320)]
	[SerializeField] private float patrolSightRadius;
	[Header("Ѳ��ʱ����Ұ�Ƕ�")]
	[Range(-1, 1)]
	[SerializeField] private float patrolSightAngle;
	[Header("����ʱ����Ұ����")]
	[Range(1, 320)]
	[SerializeField] private float pursuitSightRadius;
	[Header("ʼ��׷ɱ���ģʽ�µ���Ұ����")]
	[Range(120, 8000)]
	[SerializeField] private float alwaysPursuitSightRadius;
	[Header("ʼ��׷ɱ���ģʽ�µ���Ұ�Ƕ�")]
	[Range(-1, 1)]
	[SerializeField] private float alwaysPursuitSightAngle;

	private float currentSightRadius; // ��ǰ����Ұ��Χ
	private float currentSightAngle; // ��ǰ����Ұ�Ƕ�

	[Header("��������Ľӽ�����С����")]
	[Tooltip("���ֻҪ�����������������ô��ض��ᱻ����")]
	[Range(1, 160)]
	[SerializeField] private float minSightRadius;

	// ==================== �����õı��� ==================== //

	private Vector3 viewDir = Vector3.zero;

	#endregion

	#region �������߸�֪�ñ���

	[Header("ǿӲ������ҵĳ���ʱ��")]
	[Tooltip("��ҹ�����ʬ����������ʱ�䣬��ʱ�����֮ǰ��ʬ��һֱ������������ң���ֵԽ����Ϸ�Ѷ�Խ��")]
	[SerializeField] private float maxLockPlayerTime;
	private float lockPlayerTotalTime;

	#endregion

	#region �����������ں���

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

	#region ���߸�֪����

	/// <summary>
	/// ֱ���������
	/// </summary>
	/// <param name="_playerTrans"></param>
	public void DirectLockPlayer(Transform _playerTrans)
	{
		lockPlayerTotalTime = maxLockPlayerTime;
		zombieController.zombieBattle.attackTargetTrans = _playerTrans;
	}

	/// <summary>
	/// δ����ʱ���ڱ�׼�����µ����߸�֪
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

			// ������һ�����
			if (GameProgressManager.Instance.playerController != null)
			{
				zombieController.zombieBattle.attackTargetTrans = GameProgressManager.Instance.playerController.transform;
			}
			else
			{
				Debug.LogWarning("��ǰ��Ϸ����δע�����");
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
		if (playerColliders.Length == 1) // ��Ŀ�귢��ֱ�ߣ������Ƿ��ܻ���
		{
			GameObject viewTarget = playerColliders[0].GetComponent<PlayerController>().enemyViewTarget;
			Vector3 targetDir = (viewTarget.transform.position - sightStartTrans.position).normalized;
			viewDir = targetDir; // �����ñ���

			int ignoreLayerIndex = (1 << zombieController.layerAndTagCollection_Enemy.airWallLayerIndex) | (1 << zombieController.layerAndTagCollection_Enemy.doorInteractiveLayerIndex) | (1 << zombieController.layerAndTagCollection_Enemy.ignoreNavMeshLayerIndex); // ���Ե���ײ
			ignoreLayerIndex = ~(ignoreLayerIndex);

			RaycastHit hitObj;

			if (Physics.Raycast(sightStartTrans.position, Vector3.Normalize(targetDir), out hitObj, currentSightRadius, ignoreLayerIndex))
			{
				if (hitObj.collider.CompareTag("Player")) // ��������˼������Ƿ���ָ������Ұ�Ƕ���
				{
					if (Vector3.Dot(sightStartTrans.forward, targetDir) > currentSightAngle)
					{
						// Debug.Log("���������");
						zombieController.zombieBattle.attackTargetTrans = playerColliders[0].transform;
					}
					else
					{
						// Debug.Log("���뷶Χ����û�������");
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
			Debug.LogWarning("�����ϷĿǰ�ǵ�����Ϸ����Ӧ�ü�⵽�������");
		}
	}

	/// <summary>
	/// �ڽ����뷶Χ�ڣ�����һ���ᷢ�����
	/// </summary>
	public void UnlockedEnemyViewCloseRange()
	{
		Collider[] playerColliders = Physics.OverlapSphere(sightStartTrans.position, minSightRadius, 1 << zombieController.layerAndTagCollection_Enemy.playerLayerIndex);

		if (playerColliders.Length == 1)
		{
			Debug.Log("��ҽ�����С�ӽ�����");
			zombieController.zombieBattle.attackTargetTrans = playerColliders[0].transform;
		}
		if (playerColliders.Length > 1)
		{
			zombieController.zombieBattle.attackTargetTrans = null;
			Debug.LogWarning("�����ϷĿǰ�ǵ�����Ϸ����Ӧ�ü�⵽�������");
		}
	}

	/// <summary>
	/// ����ʱ��ʼ�ռ���׼���߷�Χ�ڵ����
	/// </summary>
	public void LockedEnemyView()
	{
		if (zombieController.currentStateEnum == CurrentStateEnum.AlwaysPursuit)
		{
			// ������һ�����
			if (GameProgressManager.Instance.playerController != null)
			{
				zombieController.zombieBattle.attackTargetTrans = GameProgressManager.Instance.playerController.transform;
			}
			else
			{
				Debug.LogWarning("��ǰ��Ϸ����δע�����");
				zombieController.zombieBattle.attackTargetTrans = null;
			}
		}
		else
		{
			Collider[] playerColliders = Physics.OverlapSphere(sightStartTrans.position, currentSightRadius, 1 << zombieController.layerAndTagCollection_Enemy.playerLayerIndex);

			if (playerColliders.Length == 0) // ���������Ұ��Ŀ������Ϊ��
			{
				if (lockPlayerTotalTime <= 0)
				{
					zombieController.zombieBattle.attackTargetTrans = null;
				}
			}
			if (playerColliders.Length == 1)
			{
				// Debug.Log("�����׷����Ұ��Χ��");
				zombieController.zombieBattle.attackTargetTrans = playerColliders[0].transform;
			}
			if (playerColliders.Length > 1)
			{
				zombieController.zombieBattle.attackTargetTrans = null;
				Debug.LogWarning("�����ϷĿǰ�ǵ�����Ϸ����Ӧ�ü�⵽�������");
			}
		}
	}

	#endregion

	#region DEBUG �ú���

	/// <summary>
	/// �������е����߼��
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
