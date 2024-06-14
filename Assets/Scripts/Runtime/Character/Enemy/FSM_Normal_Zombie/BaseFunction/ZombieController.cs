using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// ע�⣺Idle �� Patrol �Ƕ���������״̬
// ����״̬����㶼�� Idle���ɾ�������

/// <summary>
/// ��¼�ý�ʬ��ǰ�ڵ��˻��Ǻ��ڵ���
/// </summary>
public enum ProgressState
{
	None,
	EarlyEnemy,
	LaterEnemy
}

/// <summary>
/// ���ý�ʬ��δ����ʱ����Ϊģʽ
/// </summary>
public enum UnlockedEnemyMode
{
	None,
	Stand,
	AlwaysPatrol, // ʼ��Ѳ��
	RandomPatrol, // �������Ѳ�ߺ�վ׮���л�
	AlwaysPursuit, // ��Զ׷�����
}

/// <summary>
/// ��¼δ����ʱ����ʲô״̬
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
/// ��ʬ����˵Ŀ��������������ʬ�ƶ�������״̬�л���ս��
/// </summary>
[RequireComponent(typeof(ZombieView))]
[RequireComponent(typeof(ZombieBattle))]
[RequireComponent(typeof(ZombieHealth))]
[RequireComponent(typeof(ZombieAnim))]
public class ZombieController : MonoBehaviour
{
	#region ��������ͱ���

	[Header("Layer Tag ��Ϣ����")]
	public LayerAndTagCollection_Enemy layerAndTagCollection_Enemy;

	[Header("�������Ƿ�ᴥ�������ʾ��Ϣ")]
	public bool canNoticePlayer;
	[Header("�����֪ͨ�����Ϣ��������֪ͨ����ʲô�׶�")]
	public GameTargetTipState gameTargetTipState;

	[Header("�õ�������ʱ���ӵ�����ʧ���ӳ�ʱ��")]
	[SerializeField] private float delayDisappearTime = 12f;

	[Header("��������ͱ���")]
	[Header("��������������")]
	public NavMeshAgent navMeshAgent;

	[Header("��Ұ������")]
	public ZombieView zombieView;
	[Header("ս��������")]
	public ZombieBattle zombieBattle;
	[Header("����ֵ������")]
	public ZombieHealth zombieHealth;
	[Header("���Ž���������")]
	public ZombieDoorInteractive zombieDoorInteractive;

	[Header("����������")]
	public ZombieAnim zombieAnim;

	[Header("�Ų���������")]
	public ZombieFootstepSound zombieFootstepSound;
	[Header("�����������")]
	public ZombieRoarSound zombieRoarSound;

	[Header("���øý�ʬ��ǰ�ڹֻ��Ǻ��ڹ�")]
	public ProgressState progressState = ProgressState.None;

	// ==================== δ����ʱ����ر��� ==================== //

	[Header("���ý�ʬδ����ʱ�Ļ�����Ϊģʽ")]
	public UnlockedEnemyMode unlockedEnemyMode = UnlockedEnemyMode.Stand;
	[Header("δ����ʱ��Ѳ�ߺ�վ׮״̬�ı��������� UnlockedEnemyMode ����Ϊ RandomPatrol ʱ��Ч��")]
	[Range(0, 2000)]
	public int standStateProportion;

	[Header("Idle ״̬�£���ȫ��ֹ�ͻ�������״̬�ı���")]
	[Range(0, 2000)]
	public int stillnessProportion;

	[Header("Idle ״̬���Գ������")]
	[Tooltip("�������೤ʱ��󣬻���ͼ����ѡ��һ�� Idle �� Patrol ״̬")]
	[Range(1, 120)]
	public float maxIdleTime;
	[HideInInspector] public float totalIdleTime;

	[Header("Patrol ״̬���Գ������")]
	[Tooltip("�������೤ʱ��󣬻���ͼ����ѡ��һ�� Idle �� Patrol ״̬")]
	[Range(1, 120)]
	public float maxPatrolTime;
	[HideInInspector] public float totalPatrolTime;

	[Header("Idle��վ׮�� ״̬�£�վ�Ų������ʱ��")]
	[Tooltip("�������೤ʱ��󣬻���ͼ����ȫվ�Ų����ͻ�����Χ���л�")]
	[Range(1, 60)]
	public float maxStillnessTime;
	[HideInInspector] public float totalStillnessTime;

	[Header("Lost Player����ʧĿ����ң�״̬�����Գ����೤ʱ��")]
	[Tooltip("�������೤ʱ��󣬻��˳���ʧ���״̬")]
	[Range(1, 30)]
	public float maxLostPlayerTime;
	[HideInInspector] public float totalLostPlayerTime;

	[Header("Ѳ��״̬�£����ý�ʬ��Ѳ�ߵ�λ")]
	[SerializeField] private List<Transform> patrolPointList = new List<Transform>();

	[Header("��ǰ�׶��ҵ���Ѳ�ߵ�λ�������Ϸ����õ�Ѳ�ߵ�λ�Զ���ֵ��")]
	private List<PatrolPointDetail> currentPatrolPath = new List<PatrolPointDetail>();

	[Header("Ѳ��״̬�£���ʬ��Ŀ���λ�ж��ʱ�����øõ�λΪ�ѵ���")]
	public float minContactDistance;

	// ==================== �ƶ��ٶȺ���ת�Ƕ���ر��� ==================== //

	[Header("����״̬�µ��ƶ��ٶ�")]
	[Header("Ѳ��ʱ���ƶ��ٶ�")]
	public float patrolMoveSpeed;
	[Header("׷�����ʱ���ƶ��ٶ�")]
	public float pursuitMoveSpeed;

	[Header("����״̬�µ���ת�ٶ�")]
	[Header("Ѳ��ʱ����ת�ٶ�")]
	public float patrolAngularSpeed;
	[Header("׷�����ʱ����ת�ٶ�")]
	public float pursuitAnglarSpeed;

	// ==================== ״̬��ʵ������ ==================== //

	[HideInInspector] public CurrentStateEnum currentStateEnum = CurrentStateEnum.None;
	public Zombie_BaseState currentState; // ��ǰ����״̬
	public Zombie_IdleState idleState = new Zombie_IdleState();
	public Zombie_PatrolState patrolState = new Zombie_PatrolState();
	public Zombie_PursuitState pursuitState = new Zombie_PursuitState();
	public Zombie_LostPlayerState lostPlayerState = new Zombie_LostPlayerState();
	public Zombie_HardStraightState hardStraightState = new Zombie_HardStraightState();
	public Zombie_DeadState deadState = new Zombie_DeadState();

	#endregion

	#region ��ɫ״̬�л�

	public void SwitchState(Zombie_BaseState state)
	{
		currentState = state;
		currentState.EnterState(this);
	}

	#endregion

	#region �����������ں���

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

		// �޸��쳣��ת
		transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);
	}

	private void OnDisable()
	{
		SetupUnlockedEnemyMode();
		zombieAnim.ResetAnim();
	}

	#endregion

	#region ��Ϸ��ʼʱ���Լ�ע�ᵽ��Ϸ���̹�������

	private void RegisterMyself()
	{
		if (GameProgressManager.Instance == null)
		{
			Debug.LogWarning("������Ϸ���̹�����û�б�����򲻴��ڣ����鳡������");
			return;
		}

		GameProgressManager.Instance.zombieControllerList.Add(this);
	}

	#endregion

	#region ������״̬�µĵ���Ѱ·

	/// <summary>
	/// ������״̬�����õ��˵� Idle ״̬
	/// </summary>
	public void SetupUnlockedEnemyMode()
	{
		if (unlockedEnemyMode == UnlockedEnemyMode.None)
		{
			Debug.LogWarning("δ�������õ���δ����ʱ״̬��������Ϸ���");
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
			int randomIndex = Random.Range(0, 2000); // ����������ɷ�Χ�� (0, 400]

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

	// ==================== ԭ��վ׮���� ==================== //

	public void RandomSetIdleMode()
	{
		int randomIndex = Random.Range(0, 2000);

		if (randomIndex < stillnessProportion)
		{
			currentStateEnum = CurrentStateEnum.IdleStillness; // ��¼״̬
			zombieAnim.PlayLookAroundAnim(false);
		}
		else
		{
			currentStateEnum = CurrentStateEnum.IdleLookAround; // ��¼״̬
			zombieAnim.PlayLookAroundAnim(true);
		}
	}

	// ==================== Ѳ�߹��� ==================== //

	/// <summary>
	/// ����δ����״̬�µ�Ѳ��·��
	/// </summary>
	public void SetupPatrolPath()
	{
		currentStateEnum = CurrentStateEnum.Patrol; // ��¼״̬

		// �����֮ǰ�ù���Ѳ�ߵ�λ
		currentPatrolPath.Clear();

		// ���ݸ����õ�Ѳ�ߵ�λ����·��
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
	/// Ѱ��ָ����λ����ʹ��ǰ��
	/// </summary>
	/// <returns></returns>
	public Vector3 PatrolMoveToTarget()
	{
		for (int i = 0; i < currentPatrolPath.Count; i++)
		{
			if (!currentPatrolPath[i].beenThere) // ��������λûȥ��
			{
				navMeshAgent.SetDestination(currentPatrolPath[i].trans.position);
				return currentPatrolPath[i].trans.position;
			}
		}

		// Debug.Log("���е�λ��ȥ���ˣ���Ҫ����ѡ��һ��·��");
		SetupPatrolPath();

		return PatrolMoveToTarget();
	}

	/// <summary>
	/// ����ѵ���ĵ�λΪ��ȥ��
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

	#region ����ʱɾ���Լ�

	/// <summary>
	/// ����ʱɾ���Լ�������ʱ���ӳ٣�
	/// </summary>
	public void DestroyItselfWhenDead()
	{
		Destroy(gameObject, delayDisappearTime);
	}

	#endregion

}
