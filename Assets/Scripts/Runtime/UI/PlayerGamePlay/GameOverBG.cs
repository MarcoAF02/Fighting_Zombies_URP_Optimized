using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏完成的 UI 背景
/// </summary>
public class GameOverBG : MonoBehaviour
{
	#region 基本组件和变量

	[Header("生命值背景控制器")]
	[SerializeField] private HealthBGController healthBGController;
	[Header("游戏目标背景图片")]
	[SerializeField] private Image gameOverBGImage;
	[Header("游戏通关时背景屏幕的颜色")]
	[SerializeField] private Color completeBGColor;
	[Header("游戏失败时背景屏幕的颜色")]
	[SerializeField] private Color failBGColor;
	[Header("背景屏幕颜色改变的速度")]
	[SerializeField] private float bgChangeSpeed;

	// 协程
	private Coroutine changeBGWhenFail_IECor;

	#endregion

	#region 基本生命周期函数

	private void Start()
	{
		GameProgressManager.Instance.eventHandler_GameManager.ChangeGameProgressEvent += ChangeBGColor;
	}

	private void OnDestroy()
	{
		try
		{
			GameProgressManager.Instance.eventHandler_GameManager.ChangeGameProgressEvent -= ChangeBGColor;
		}
		catch
		{
			Debug.Log("游戏进程结束，无需取消订阅事件");
		}
	}

	#endregion

	#region 游戏失败或通关时改变背景屏幕

	/// <summary>
	/// 游戏失败时背景屏幕变黑
	/// </summary>
	public void ChangeBGColor(GameProgress _gameProgress)
	{
		if (changeBGWhenFail_IECor != null)
		{
			StopCoroutine(changeBGWhenFail_IECor);
			gameOverBGImage.color = new Color(0f, 0f, 0f, 0f);
		}

		changeBGWhenFail_IECor = StartCoroutine(ChangeBGWhenFail_IE(_gameProgress));
	}

	private IEnumerator ChangeBGWhenFail_IE(GameProgress _gameProgress)
	{
		if (_gameProgress == GameProgress.EarlyStage) // 游戏开始时重置背景颜色
		{
			gameOverBGImage.color = new Color(0f, 0f, 0f, 0f);
			yield break;
		}

		if (_gameProgress == GameProgress.LaterStage)
		{
			gameOverBGImage.color = new Color(0f, 0f, 0f, 0f);
			yield break;
		}

		if (_gameProgress == GameProgress.GameComplete)
		{
			gameOverBGImage.color = new Color(completeBGColor.r, completeBGColor.g, completeBGColor.b, 0f);
		}

		if (_gameProgress == GameProgress.GameFail)
		{
			gameOverBGImage.color = new Color(failBGColor.r, failBGColor.g, failBGColor.b, 0f);
		}

		while (true)
		{
			float bgAlpha = Mathf.MoveTowards(gameOverBGImage.color.a, 1f, bgChangeSpeed * Time.deltaTime);
			gameOverBGImage.color = new Color(gameOverBGImage.color.r , gameOverBGImage.color.g, gameOverBGImage.color.b, bgAlpha);

			// Debug.Log(bgAlpha);

			if (bgAlpha >= 1f)
			{
				yield break;
			}

			yield return null;
		}
	}

	#endregion
}
