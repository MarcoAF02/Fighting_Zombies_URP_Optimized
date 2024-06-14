using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: ������һ���ǻ���ͷ����������ͷ����Ч��

/// <summary>
/// ��ʬ�ܻ����˹���ʵ��
/// </summary>
public class ZombieHealth : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��������ͱ���")]
	[SerializeField] private ZombieController zombieController;
	[Header("������ײ��")]
	[SerializeField] private Collider physicsCollider;
	[Header("�˺��� HitBox")]
	[SerializeField] private List<Collider> hitBox = new List<Collider>();

	#region ����ֵ���˺��йز���

	[Header("����ֵ���˺��йز���")]
	[Header("��ʬ����ֵ")]
	[Range(0f, 2000f)]
	[SerializeField] private float maxHealth;
	[SerializeField] private float currentHealth;
	[HideInInspector] public bool isDead;
	private float lastFrameHeath;

	[Header("���н�ʬ��ͬ��λ�����˺��ķŴ���")]
	[Header("����ͷ��")]
	[SerializeField] private float headDamageMagRate = 2f;
	[Header("��������")]
	[SerializeField] private float bodyDamageMagRate = 1f;
	[Header("������֫")]
	[SerializeField] private float fourLimbsMagRate = 0.5f;

	[Header("��ʬ���˺���ʱ��������Ŀ���")]
	[SerializeField] private Transform moveBackTrans;

	[Header("��ʬ�ܻ�����ʱ����󵼺����ƶ��ٶ�")]
	public float hardStraightBackSpeed;

	[Header("����Ӳֱ״̬�� CD ʱ��")]
	public float hardStraightCDTime;
	public float hardStraightTotalTime;

	[Header("��������Ӳֱ״̬��������ʱ��")]
	public float getInHardStraightMaxTime;
	public float getInHardStraightTotalTime; // ��������Ӳֱ״̬�����˶��
	public Zombie_BaseState lastState; // ��¼��������Ӳֱ״̬ǰ������״̬

	#endregion

	#region ��ͬ���岿λ�� Tag ��ǩ

	[Header("ͷ��")]
	[SerializeField] private string headTag;
	[Header("����")]
	[SerializeField] private string bodyTag;
	[Header("��֫")]
	[SerializeField] private string fourLimbsTag;

	#endregion

	#region �����н���Ӳֱ״̬

	// Ӳֱ״̬�ļ��㷽����
	// 1. �Ƚ��������ֵ��Сһ����������Ϊ����Ӳֱ�Ļ����˺�ֵ
	// 2. ���ȡһ����Χ�ڵ�������������������һ֡���ٵ�����ֵ�������ܷ񳬹�Ӳֱ���ߣ����Գ�������Ӳֱ

	// lastFrameHealth - currentHealth ���Եõ��˺�����ֵ

	[Header("���ڼ���Ӳֱ���ߵĻ�������")]
	[Range(0, 1)]
	[SerializeField] private float hardStraightMag;
	private float hardStraightValue; // ʵ�ʼ���ó���Ӳֱ����

	[Header("��������Ӳֱ������Χ")]
	[SerializeField] private float maxAddLimit;
	[SerializeField] private float minAddLimit;

	#endregion

	#endregion

	#region �����������ں���

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

	#region ��ʼ�������ֵ

	private void SetupValue()
	{
		isDead = false;
		currentHealth = maxHealth;
		lastFrameHeath = currentHealth;
		hardStraightValue = maxHealth * hardStraightMag;
		hardStraightTotalTime = hardStraightCDTime;
	}

	#endregion

	#region ��ʬ�ܻ����˹���

	/// <summary>
	/// ��ʬ������ʱ��Ѫ������Ĳ��������в�λ�� Tag ������������˺�ֵ
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

		if (currentHealth <= 0f) // ��������״̬
		{
			currentHealth = 0f;
			isDead = true;
			zombieController.SwitchState(zombieController.deadState);
		}
	}

	private void TakeHardStraight()
	{
		if (zombieController.currentState == zombieController.hardStraightState) return; // �����ظ�����Ӳֱ״̬

		float totalValue = (lastFrameHeath - currentHealth) + Random.Range(minAddLimit, maxAddLimit);

		if (totalValue > hardStraightValue) // ����Ӳֱ״̬
		{
			zombieController.SwitchState(zombieController.hardStraightState);
		}
	}

	/// <summary>
	/// ��ʬ����Ӳֱ�����Լ���󵼺��ƶ�һ�ξ��벢�����ܻ�����
	/// </summary>
	public void HardStraightMoveBack()
	{
		zombieController.navMeshAgent.SetDestination(moveBackTrans.position);
	}

	#endregion

	#region ���� HitBox

	/// <summary>
	/// �ֶ�ָ���������ϵ� HitBox �Ƿ���Ҫ��
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
