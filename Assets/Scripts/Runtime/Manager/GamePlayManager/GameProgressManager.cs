using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ǰˢ�ֵ���ƣ�
// ���йֶ��������ڳ����ڰںõģ����ݵ��˽ű������õ�����ʲô�׶�����
// ��� PlayerController �����ü�¼�ڸýű��У��۲���ģʽ��
// ���� Controller �����ü�¼�ڸýű��У��۲���ģʽ��

/// <summary>
/// ��Ϸ�׶εĶ���
/// </summary>
public enum GameProgress
{
	None,
	EarlyStage,
	LaterStage,
	GameComplete,
	GameFail
}

/// <summary>
/// ��Ϸ�׶εĶ��壺�������֪ͨ�����Ϣ
/// </summary>
public enum GameTargetTipState
{
	None,
	EarlyStage,
	LaterStage
}

/// <summary>
/// ��Ϸ���̹�������������Ϸ����
/// </summary>
public class GameProgressManager : MonoSingleton<GameProgressManager>
{
	#region ��������ͱ���

	[Header("��Ϸ�����¼�������")]
	public EventHandler_GameManager eventHandler_GameManager;

	[Header("ʱ���趨����")]
	[SerializeField] private ProgressTimer progressTimer;

	[Header("��ǰ�ؿ��е�Ŀ��ɱ����")]
	[SerializeField] private int targetKillCount;
	private int currentKillCount; // ��ǰɱ����

	[Header("ɱ�����Ƿ���")]
	public bool isKillComplete;

	[Header("��ǰ�����е���ң�Ψһ��")]
	public PlayerController playerController;

	[Header("��ϷĿ����ʾ��Ϣ")]
	[Header("��Ϸ����Ŀ����ʾ")]
	[SerializeField] private string gameTargetTip;

	[Header("��Ϸ������ڵ�Ŀ����ʾ")]
	[SerializeField] private string gameLaterTargetTip;
	[Header("��ϷĿ����ʾ����ʾʱ��")]
	[SerializeField] private float targetTipDisplayTime;

	[Header("��ǰ�ؿ���������ͨ��ʬ�ࣨ״̬����ͨ��ʬ���ˣ�")]
	[Tooltip("������Ϸ���ȹ����һ����")]
	public List<ZombieController> zombieControllerList = new List<ZombieController>();

	private GameProgress gameProgress = GameProgress.None; // ��ǰ��Ϸ�׶�

	#endregion

	#region �����������ں���

	protected override void Awake()
	{
		base.Awake();
		eventHandler_GameManager = GameObject.FindGameObjectWithTag("EventHandler_GameManager").GetComponent<EventHandler_GameManager>();
	}

	private void Start()
	{
		eventHandler_GameManager.ChangeGameProgressEvent += GenerateNewMonster;
		eventHandler_GameManager.ChangeGameProgressEvent += SaveGameData;
		StartCoroutine(PerformChangeGameProgress_IE(GameProgress.EarlyStage, 0.2f));
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		eventHandler_GameManager.ChangeGameProgressEvent -= GenerateNewMonster;
		eventHandler_GameManager.ChangeGameProgressEvent -= SaveGameData;
	}

	#endregion

	#region �ƽ���Ϸ����

	/// <summary>
	/// ����ɱ����
	/// </summary>
	public void AddCurrentKillCount()
	{
		currentKillCount++;

		// Debug.Log("����ɱ����");

		if (currentKillCount >= targetKillCount) // ͨ��
		{
			// Debug.Log("ɱ�����ﵽĿ��");
			currentKillCount = targetKillCount;
			isKillComplete = true;
		}
	}

	#endregion

	#region �ı���Ϸ�׶�

	// ����Э�̵ȴ���ʱ�䣺
	// https://docs.unity.cn/cn/2022.3/ScriptReference/WaitForSeconds.html

	/// <summary>
	/// �ı���Ϸ�׶Σ������ӳٵ���
	/// </summary>
	/// <param name="_gameProgress"></param>
	/// <returns></returns>
	public IEnumerator PerformChangeGameProgress_IE(GameProgress _gameProgress, float _time)
	{
		yield return new WaitForSeconds(_time);
		PerformChangeGameProgress(_gameProgress);
	}

	/// <summary>
	/// ʵ�ʵظı���Ϸ�׶�
	/// </summary>
	/// <param name="gameProgress"></param>
	public void PerformChangeGameProgress(GameProgress _gameProgress)
	{
		gameProgress = _gameProgress; // ���ݴ���Ĳ����ı���Ϸ�׶Σ��ı������¼������ߣ���ʾ���ж����¼��Ķ�����Ӧ�¼�
		eventHandler_GameManager.InvokeChangeGameProgress(gameProgress);
	}

	/// <summary>
	/// ���¼�����ͬ��Ϸ�׶εĴ浵
	/// </summary>
	/// <param name="_gameProgress"></param>
	public void SaveGameData(GameProgress _gameProgress)
	{
		if (_gameProgress == GameProgress.GameComplete)
		{
			SaveLoadManager.Instance.SaveGamePlayData(); // �浵
		}
	}

	/// <summary>
	/// ���¼���������Ϸ��ͬ�׶εĹ���ű��������ɹ��
	/// </summary>
	public void GenerateNewMonster(GameProgress _gameProgress)
	{
		for (int i = 0; i < zombieControllerList.Count; i++) // ����ʲôʱ�򶼻�����û���������ɽ׶εĵ���
		{
			if (zombieControllerList[i].progressState == ProgressState.None)
			{
				try
				{
					zombieControllerList[i].gameObject.SetActive(false);
				}
				catch
				{
					Debug.Log("��Ϸ�Ѿ������������������");
				}
			}
		}

		if (_gameProgress == GameProgress.None)
		{
			Debug.LogWarning("��Ϸ����Ϊ None������ؿ���Ƶ���ȷ��");
			return;
		}

		if (_gameProgress == GameProgress.EarlyStage)
		{
			// Debug.Log("ˢǰ�ڹ�");

			for (int i = 0; i < zombieControllerList.Count; i++)
			{
				if (zombieControllerList[i].progressState == ProgressState.EarlyEnemy)
				{
					try
					{
						zombieControllerList[i].gameObject.SetActive(true);
					}
					catch
					{
						Debug.Log("��Ϸ�Ѿ������������������");
					}
				}

				if (zombieControllerList[i].progressState == ProgressState.LaterEnemy)
				{
					try
					{
						zombieControllerList[i].gameObject.SetActive(false); // �ȰѺ��ڹֲ�����
					}
					catch
					{
						Debug.Log("��Ϸ�Ѿ������������������");
					}
				}
			}
		}

		if (_gameProgress == GameProgress.LaterStage)
		{
			// Debug.Log("ˢ���ڹ�");

			for (int i = 0; i < zombieControllerList.Count; i++)
			{
				if (zombieControllerList[i].progressState == ProgressState.LaterEnemy)
				{
					try
					{
						zombieControllerList[i].gameObject.SetActive(true); // ��ʾ���ڹ֣�ǰ�ڹֲ�������
					}
					catch
					{
						Debug.Log("��Ϸ�Ѿ������������������");
					}
				}
			}
		}
	}

	#endregion

	#region ֪ͨ�����ϷĿ��

	/// <summary>
	/// ֪ͨ��Ҳ�ͬ�׶ε���ϷĿ��
	/// </summary>
	/// <param name="gameTargetTipState"></param>
	public void NoticePlayerGameTarget(GameTargetTipState gameTargetTipState)
	{
		if (gameTargetTipState == GameTargetTipState.None)
		{
			Debug.LogWarning("��Ϸ��ʾĿ������������齻����Ʒ������");
			return;
		}

		if (gameTargetTipState == GameTargetTipState.EarlyStage)
		{
			playerController.tipMessageController.ShowTipMessage(gameTargetTip, targetTipDisplayTime);
		}

		if (gameTargetTipState == GameTargetTipState.LaterStage)
		{
			playerController.tipMessageController.ShowTipMessage(gameLaterTargetTip, targetTipDisplayTime);
		}
	}

	#endregion

	#region ����ɷ��ʵ�����

	/// <summary>
	/// ��ϷĿ��
	/// </summary>
	public GameProgress CurrentGameProgress => gameProgress;

	/// <summary>
	/// ��Ϸ��Ŀ����ʾ
	/// </summary>
	public string GameTargetTip => gameTargetTip;

	/// <summary>
	/// ��Ϸ����Ŀ����ʾ
	/// </summary>
	public string GameLaterTargetTip => gameLaterTargetTip;

	/// <summary>
	/// ʣ���ɱ����
	/// </summary>
	public int ResidueKillCount => targetKillCount - currentKillCount;

	#endregion

}
