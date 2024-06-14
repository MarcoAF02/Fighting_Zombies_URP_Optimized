using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 游戏成功菜单
/// </summary>
public class GameCompleteMenu : MonoBehaviour
{
	#region 基本组件和变量

	[Header("玩家角色控制器")]
	[SerializeField] private PlayerController playerController;
	[Header("成功界面根物体")]
	[SerializeField] private GameObject gameCompleteMenuRoot;
	[Header("通关时显示完成时间 UI")]
	[SerializeField] private TextMeshProUGUI completeTimeUIText;
	[Header("游戏成功后过多久显示通关 UI")]
	[SerializeField] private float showUIIntervalTime;

	// 协程
	private Coroutine displayGameCompleteMenu_IECor;

	#endregion

	#region 基本生命周期函数

	private void Start()
	{
		GameProgressManager.Instance.eventHandler_GameManager.ChangeGameProgressEvent += DisplayGameCompleteMenu;
	}

	private void OnDestroy()
	{
		try
		{
			GameProgressManager.Instance.eventHandler_GameManager.ChangeGameProgressEvent -= DisplayGameCompleteMenu;
		}
		catch
		{
			Debug.Log("游戏进程结束，无需取消订阅事件");
		}
	}

	#endregion

	#region 菜单显示功能

	private void DisplayGameCompleteMenu(GameProgress _gameProgress)
	{
		displayGameCompleteMenu_IECor = StartCoroutine(DisplayGameCompleteMenu_IE(_gameProgress));
	}

	private IEnumerator DisplayGameCompleteMenu_IE(GameProgress _gameProgress)
	{
		if (_gameProgress == GameProgress.GameComplete)
		{
			playerController.playerTimeManager.GetGamePlayTotalTime(); // 记录游戏时间
		}

		yield return new WaitForSeconds(showUIIntervalTime);

		if (_gameProgress == GameProgress.GameComplete)
		{
			gameCompleteMenuRoot.SetActive(true);

			// 显示关卡完成时间
			DisplayGameCompleteTime(GameBestTimeManager.Instance.GetGameUseTimeString());

			Time.timeScale = 0f;
		}
	}

	#endregion

	#region 游戏完成时，显示完成时间的 UI

	/// <summary>
	/// 游戏完成时，显示完成时间的 UI
	/// </summary>
	public void DisplayGameCompleteTime(string _timeText)
	{
		completeTimeUIText.text = string.Empty; // 先清空文字
		completeTimeUIText.text = _timeText; // 然后赋值
	}

	#endregion
}
