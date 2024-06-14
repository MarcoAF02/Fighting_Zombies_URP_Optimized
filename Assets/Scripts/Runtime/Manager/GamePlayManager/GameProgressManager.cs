using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 当前刷怪的设计：
// 所有怪都是事先在场景内摆好的，根据敌人脚本决定该敌人在什么阶段生成
// 玩家 PlayerController 的引用记录在该脚本中（观察者模式）
// 敌人 Controller 的引用记录在该脚本中（观察者模式）

/// <summary>
/// 游戏阶段的定义
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
/// 游戏阶段的定义：这个用于通知玩家消息
/// </summary>
public enum GameTargetTipState
{
	None,
	EarlyStage,
	LaterStage
}

/// <summary>
/// 游戏进程管理器，管理游戏进程
/// </summary>
public class GameProgressManager : MonoSingleton<GameProgressManager>
{
	#region 基本组件和变量

	[Header("游戏进度事件发布器")]
	public EventHandler_GameManager eventHandler_GameManager;

	[Header("时间设定集合")]
	[SerializeField] private ProgressTimer progressTimer;

	[Header("当前关卡中的目标杀敌数")]
	[SerializeField] private int targetKillCount;
	private int currentKillCount; // 当前杀敌数

	[Header("杀敌数是否达标")]
	public bool isKillComplete;

	[Header("当前场景中的玩家（唯一）")]
	public PlayerController playerController;

	[Header("游戏目标提示信息")]
	[Header("游戏最终目标提示")]
	[SerializeField] private string gameTargetTip;

	[Header("游戏进入后期的目标提示")]
	[SerializeField] private string gameLaterTargetTip;
	[Header("游戏目标提示的显示时间")]
	[SerializeField] private float targetTipDisplayTime;

	[Header("当前关卡中所有普通僵尸类（状态机普通僵尸敌人）")]
	[Tooltip("这是游戏进度管理的一部分")]
	public List<ZombieController> zombieControllerList = new List<ZombieController>();

	private GameProgress gameProgress = GameProgress.None; // 当前游戏阶段

	#endregion

	#region 基本生命周期函数

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

	#region 推进游戏进度

	/// <summary>
	/// 增加杀敌数
	/// </summary>
	public void AddCurrentKillCount()
	{
		currentKillCount++;

		// Debug.Log("增加杀敌数");

		if (currentKillCount >= targetKillCount) // 通关
		{
			// Debug.Log("杀敌数达到目标");
			currentKillCount = targetKillCount;
			isKillComplete = true;
		}
	}

	#endregion

	#region 改变游戏阶段

	// 关于协程等待的时间：
	// https://docs.unity.cn/cn/2022.3/ScriptReference/WaitForSeconds.html

	/// <summary>
	/// 改变游戏阶段，但是延迟调用
	/// </summary>
	/// <param name="_gameProgress"></param>
	/// <returns></returns>
	public IEnumerator PerformChangeGameProgress_IE(GameProgress _gameProgress, float _time)
	{
		yield return new WaitForSeconds(_time);
		PerformChangeGameProgress(_gameProgress);
	}

	/// <summary>
	/// 实际地改变游戏阶段
	/// </summary>
	/// <param name="gameProgress"></param>
	public void PerformChangeGameProgress(GameProgress _gameProgress)
	{
		gameProgress = _gameProgress; // 根据传入的参数改变游戏阶段，改变后呼叫事件发布者，提示所有订阅事件的对象响应事件
		eventHandler_GameManager.InvokeChangeGameProgress(gameProgress);
	}

	/// <summary>
	/// （事件）不同游戏阶段的存档
	/// </summary>
	/// <param name="_gameProgress"></param>
	public void SaveGameData(GameProgress _gameProgress)
	{
		if (_gameProgress == GameProgress.GameComplete)
		{
			SaveLoadManager.Instance.SaveGamePlayData(); // 存档
		}
	}

	/// <summary>
	/// （事件）生成游戏不同阶段的怪物（脚本触发生成怪物）
	/// </summary>
	public void GenerateNewMonster(GameProgress _gameProgress)
	{
		for (int i = 0; i < zombieControllerList.Count; i++) // 无论什么时候都会隐藏没有设置生成阶段的敌人
		{
			if (zombieControllerList[i].progressState == ProgressState.None)
			{
				try
				{
					zombieControllerList[i].gameObject.SetActive(false);
				}
				catch
				{
					Debug.Log("游戏已经结束，无需操作怪物");
				}
			}
		}

		if (_gameProgress == GameProgress.None)
		{
			Debug.LogWarning("游戏进程为 None，请检查关卡设计的正确性");
			return;
		}

		if (_gameProgress == GameProgress.EarlyStage)
		{
			// Debug.Log("刷前期怪");

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
						Debug.Log("游戏已经结束，无需操作怪物");
					}
				}

				if (zombieControllerList[i].progressState == ProgressState.LaterEnemy)
				{
					try
					{
						zombieControllerList[i].gameObject.SetActive(false); // 先把后期怪藏起来
					}
					catch
					{
						Debug.Log("游戏已经结束，无需操作怪物");
					}
				}
			}
		}

		if (_gameProgress == GameProgress.LaterStage)
		{
			// Debug.Log("刷后期怪");

			for (int i = 0; i < zombieControllerList.Count; i++)
			{
				if (zombieControllerList[i].progressState == ProgressState.LaterEnemy)
				{
					try
					{
						zombieControllerList[i].gameObject.SetActive(true); // 显示后期怪，前期怪不做操作
					}
					catch
					{
						Debug.Log("游戏已经结束，无需操作怪物");
					}
				}
			}
		}
	}

	#endregion

	#region 通知玩家游戏目标

	/// <summary>
	/// 通知玩家不同阶段的游戏目标
	/// </summary>
	/// <param name="gameTargetTipState"></param>
	public void NoticePlayerGameTarget(GameTargetTipState gameTargetTipState)
	{
		if (gameTargetTipState == GameTargetTipState.None)
		{
			Debug.LogWarning("游戏提示目标设计有误，请检查交互物品的设置");
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

	#region 对外可访问的属性

	/// <summary>
	/// 游戏目标
	/// </summary>
	public GameProgress CurrentGameProgress => gameProgress;

	/// <summary>
	/// 游戏总目标提示
	/// </summary>
	public string GameTargetTip => gameTargetTip;

	/// <summary>
	/// 游戏后期目标提示
	/// </summary>
	public string GameLaterTargetTip => gameLaterTargetTip;

	/// <summary>
	/// 剩余的杀敌数
	/// </summary>
	public int ResidueKillCount => targetKillCount - currentKillCount;

	#endregion

}
