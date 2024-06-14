using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 交互信息 UI
/// </summary>
public class TipMessageController : MonoBehaviour
{
	#region 基本组件和变量

	// 仅有玩家教学信息要控制 GameObject 的显示和隐藏，因为它们带有图片 UI
	// 上方和下方出现的两种提示信息用文字更新即可

	[Header("教程触发器")]
	[SerializeField] private TutorialTipTrigger tutorialTrigger;

	[Header("玩家教程信息")]
	[Header("移动教程信息")]
	[SerializeField] private GameObject playerMoveTutorial;
	[Header("奔跑和下蹲教程信息")]
	[SerializeField] private GameObject runAndCrouchTutorial;
	[Header("快捷物品栏教程信息")]
	[SerializeField] private GameObject equipItemTutorial;
	[Header("枪械使用教程信息")]
	[SerializeField] private GameObject useGunTutorial;
	[Header("医疗物品使用教程信息")]
	[SerializeField] private GameObject useMedicineTutorial;
	[Header("提示按下 Tab 检查游戏目标和物品栏")]
	[SerializeField] private GameObject useInventoryTutorial;

	[Header("玩家提示信息")]
	[SerializeField] private GameObject tipMessageBG;

	[Header("物品交互信息")]
	[Header("交互信息文本 UI")]
	[SerializeField] private GameObject interactiveTextUI;

	#endregion

	// 玩家教程（提示）信息关键词定义类
	public PlayerTutorialKeyword playerTutorialKeyword;

	// 协程
	private Coroutine showInteractiveMessage_IECor;
	private Coroutine showTutorialMessage_IECor;
	private Coroutine showTipMessage_IECor;

	#region 教程信息显示功能

	/// <summary>
	/// 显示玩家教程信息
	/// </summary>
	/// <param name="_key"></param>
	/// <param name="_time"></param>
	public void ShowTutorialMessage(string _key, float _time)
	{
		ClearLastUpMessage(); // 清除上次的顶部信息
		showTutorialMessage_IECor = StartCoroutine(ShowTutorialMessage_IE(_key, _time));
	}

	private IEnumerator ShowTutorialMessage_IE(string _key, float _time)
	{
		if (_key == playerTutorialKeyword.firstMove)
		{
			playerMoveTutorial.SetActive(true);

			if (_time < 0) yield break;

			yield return new WaitForSeconds(_time);
			playerMoveTutorial.SetActive(false);
		}

		if (_key == playerTutorialKeyword.firstRunAndCrouch)
		{
			runAndCrouchTutorial.SetActive(true);

			if (_time < 0) yield break;

			yield return new WaitForSeconds(_time);
			runAndCrouchTutorial.SetActive(false);
		}

		if (_key == playerTutorialKeyword.firstUseEquipItem)
		{
			equipItemTutorial.SetActive(true);

			if (_time < 0) yield break;

			yield return new WaitForSeconds(_time);
			equipItemTutorial.SetActive(false);
		}

		if (_key == playerTutorialKeyword.firstUseGun)
		{
			useGunTutorial.SetActive(true);

			if (_time < 0) yield break;

			yield return new WaitForSeconds(_time);
			useGunTutorial.SetActive(false);
		}

		if (_key == playerTutorialKeyword.firstUseMedicine)
		{
			useMedicineTutorial.SetActive(true);

			if (_time < 0) yield break;

			yield return new WaitForSeconds(_time);
			useMedicineTutorial.SetActive(false);
		}

		if (_key == playerTutorialKeyword.firstUseInventory)
		{
			useInventoryTutorial.SetActive(true);

			if (_time < 0) yield break;

			yield return new WaitForSeconds(_time);
			useInventoryTutorial.SetActive(false);
		}
	}

	#endregion

	#region 目标提示信息显示功能

	/// <summary>
	/// 显示游戏提示信息
	/// </summary>
	/// <param name="_message"></param>
	/// <param name="_time"></param>
	public void ShowTipMessage(string _message, float _time)
	{
		ClearLastUpMessage(); // 清除上次的顶部信息
		showTipMessage_IECor = StartCoroutine(ShowTipMessage_IE(_message, _time));
	}

	private IEnumerator ShowTipMessage_IE(string _message, float _time)
	{
		tipMessageBG.SetActive(true);
		tipMessageBG.GetComponentInChildren<TextMeshProUGUI>().text = _message;

		if (_time < 0) yield break;

		yield return new WaitForSeconds(_time);

		tipMessageBG.SetActive(false);
		tipMessageBG.GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;
	}

	#endregion

	#region 交互信息显示功能

	/// <summary>
	///  根据传入的字符串显示交互信息文本（如果显示时间小于 0，提示信息将不会主动消失）
	/// </summary>
	/// <param name="_message"></param>
	/// <param name="_time"></param>
	/// <param name="_colorBG"></param>
	public void ShowInteractiveMessage(string _message, float _time, Color _colorBG)
	{
		if (showInteractiveMessage_IECor != null)
		{
			StopCoroutine(showInteractiveMessage_IECor);
		}

		showInteractiveMessage_IECor = StartCoroutine(ShowInteractiveMessage_IE(_message, _time, _colorBG));
	}

	private IEnumerator ShowInteractiveMessage_IE(string _message, float _time, Color _colorBG)
	{
		interactiveTextUI.SetActive(true);
		interactiveTextUI.GetComponent<Image>().color = _colorBG;
		interactiveTextUI.GetComponentInChildren<TextMeshProUGUI>().text = _message;

		if (_time < 0) yield break;

		yield return new WaitForSeconds(_time);

		interactiveTextUI.GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;
		interactiveTextUI.SetActive(false);
	}

	#endregion

	#region 残留信息清理

	/// <summary>
	/// 在下次显示信息前，清除玩家头顶的信息
	/// </summary>
	private void ClearLastUpMessage()
	{
		if (showTutorialMessage_IECor != null) // 清除教程信息
		{
			playerMoveTutorial.SetActive(false);
			runAndCrouchTutorial.SetActive(false);
			equipItemTutorial.SetActive(false);
			useGunTutorial.SetActive(false);
			useMedicineTutorial.SetActive(false);
			useInventoryTutorial.SetActive(false);

			StopCoroutine(showTutorialMessage_IECor);
		}

		if (showTipMessage_IECor != null) // 清除提示信息
		{
			tipMessageBG.SetActive(false);
			tipMessageBG.GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;

			StopCoroutine(showTipMessage_IECor);
		}
	}

	/// <summary>
	/// 直接清除交互提示信息
	/// </summary>
	public void ClearInteractiveMessage()
	{
		interactiveTextUI.GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;
		interactiveTextUI.SetActive(false);
	}

	#endregion
}
