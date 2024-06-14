using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏失败菜单
/// </summary>
public class GameFailMenu : MonoBehaviour
{
	#region 基本组件和变量

	[Header("失败界面根物体")]
	[SerializeField] private GameObject gameFailMenuRoot;
	[Header("游戏失败后过多久显示失败 UI")]
	[SerializeField] private float showUIIntervalTime;

	// 协程
	private Coroutine displayGameFailMenu_IECor;

	#endregion

	#region 基本生命周期函数

	private void Start()
	{
		GameProgressManager.Instance.eventHandler_GameManager.ChangeGameProgressEvent += DisplayGameFailMenu;
	}

	private void OnDestroy()
	{
		try
		{
			GameProgressManager.Instance.eventHandler_GameManager.ChangeGameProgressEvent -= DisplayGameFailMenu;
		}
		catch
		{
			Debug.Log("游戏进程结束，无需取消订阅事件");
		}
	}

	#endregion

	#region 菜单显示功能

	private void DisplayGameFailMenu(GameProgress _gameProgress)
	{
		displayGameFailMenu_IECor = StartCoroutine(DisplayGameFailMenu_IE(_gameProgress));
	}

	private IEnumerator DisplayGameFailMenu_IE(GameProgress _gameProgress)
	{
		yield return new WaitForSeconds(showUIIntervalTime);

		if (_gameProgress == GameProgress.GameFail)
		{
			gameFailMenuRoot.SetActive(true);
			Time.timeScale = 0f;
		}
	}

	#endregion

}
