using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ������Ϣ UI
/// </summary>
public class TipMessageController : MonoBehaviour
{
	#region ��������ͱ���

	// ������ҽ�ѧ��ϢҪ���� GameObject ����ʾ�����أ���Ϊ���Ǵ���ͼƬ UI
	// �Ϸ����·����ֵ�������ʾ��Ϣ�����ָ��¼���

	[Header("�̴̳�����")]
	[SerializeField] private TutorialTipTrigger tutorialTrigger;

	[Header("��ҽ̳���Ϣ")]
	[Header("�ƶ��̳���Ϣ")]
	[SerializeField] private GameObject playerMoveTutorial;
	[Header("���ܺ��¶׽̳���Ϣ")]
	[SerializeField] private GameObject runAndCrouchTutorial;
	[Header("�����Ʒ���̳���Ϣ")]
	[SerializeField] private GameObject equipItemTutorial;
	[Header("ǹеʹ�ý̳���Ϣ")]
	[SerializeField] private GameObject useGunTutorial;
	[Header("ҽ����Ʒʹ�ý̳���Ϣ")]
	[SerializeField] private GameObject useMedicineTutorial;
	[Header("��ʾ���� Tab �����ϷĿ�����Ʒ��")]
	[SerializeField] private GameObject useInventoryTutorial;

	[Header("�����ʾ��Ϣ")]
	[SerializeField] private GameObject tipMessageBG;

	[Header("��Ʒ������Ϣ")]
	[Header("������Ϣ�ı� UI")]
	[SerializeField] private GameObject interactiveTextUI;

	#endregion

	// ��ҽ̳̣���ʾ����Ϣ�ؼ��ʶ�����
	public PlayerTutorialKeyword playerTutorialKeyword;

	// Э��
	private Coroutine showInteractiveMessage_IECor;
	private Coroutine showTutorialMessage_IECor;
	private Coroutine showTipMessage_IECor;

	#region �̳���Ϣ��ʾ����

	/// <summary>
	/// ��ʾ��ҽ̳���Ϣ
	/// </summary>
	/// <param name="_key"></param>
	/// <param name="_time"></param>
	public void ShowTutorialMessage(string _key, float _time)
	{
		ClearLastUpMessage(); // ����ϴεĶ�����Ϣ
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

	#region Ŀ����ʾ��Ϣ��ʾ����

	/// <summary>
	/// ��ʾ��Ϸ��ʾ��Ϣ
	/// </summary>
	/// <param name="_message"></param>
	/// <param name="_time"></param>
	public void ShowTipMessage(string _message, float _time)
	{
		ClearLastUpMessage(); // ����ϴεĶ�����Ϣ
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

	#region ������Ϣ��ʾ����

	/// <summary>
	///  ���ݴ�����ַ�����ʾ������Ϣ�ı��������ʾʱ��С�� 0����ʾ��Ϣ������������ʧ��
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

	#region ������Ϣ����

	/// <summary>
	/// ���´���ʾ��Ϣǰ��������ͷ������Ϣ
	/// </summary>
	private void ClearLastUpMessage()
	{
		if (showTutorialMessage_IECor != null) // ����̳���Ϣ
		{
			playerMoveTutorial.SetActive(false);
			runAndCrouchTutorial.SetActive(false);
			equipItemTutorial.SetActive(false);
			useGunTutorial.SetActive(false);
			useMedicineTutorial.SetActive(false);
			useInventoryTutorial.SetActive(false);

			StopCoroutine(showTutorialMessage_IECor);
		}

		if (showTipMessage_IECor != null) // �����ʾ��Ϣ
		{
			tipMessageBG.SetActive(false);
			tipMessageBG.GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;

			StopCoroutine(showTipMessage_IECor);
		}
	}

	/// <summary>
	/// ֱ�����������ʾ��Ϣ
	/// </summary>
	public void ClearInteractiveMessage()
	{
		interactiveTextUI.GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;
		interactiveTextUI.SetActive(false);
	}

	#endregion
}
