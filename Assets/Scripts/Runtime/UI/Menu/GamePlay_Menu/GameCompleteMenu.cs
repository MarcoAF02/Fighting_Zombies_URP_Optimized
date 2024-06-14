using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// ��Ϸ�ɹ��˵�
/// </summary>
public class GameCompleteMenu : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��ҽ�ɫ������")]
	[SerializeField] private PlayerController playerController;
	[Header("�ɹ����������")]
	[SerializeField] private GameObject gameCompleteMenuRoot;
	[Header("ͨ��ʱ��ʾ���ʱ�� UI")]
	[SerializeField] private TextMeshProUGUI completeTimeUIText;
	[Header("��Ϸ�ɹ���������ʾͨ�� UI")]
	[SerializeField] private float showUIIntervalTime;

	// Э��
	private Coroutine displayGameCompleteMenu_IECor;

	#endregion

	#region �����������ں���

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
			Debug.Log("��Ϸ���̽���������ȡ�������¼�");
		}
	}

	#endregion

	#region �˵���ʾ����

	private void DisplayGameCompleteMenu(GameProgress _gameProgress)
	{
		displayGameCompleteMenu_IECor = StartCoroutine(DisplayGameCompleteMenu_IE(_gameProgress));
	}

	private IEnumerator DisplayGameCompleteMenu_IE(GameProgress _gameProgress)
	{
		if (_gameProgress == GameProgress.GameComplete)
		{
			playerController.playerTimeManager.GetGamePlayTotalTime(); // ��¼��Ϸʱ��
		}

		yield return new WaitForSeconds(showUIIntervalTime);

		if (_gameProgress == GameProgress.GameComplete)
		{
			gameCompleteMenuRoot.SetActive(true);

			// ��ʾ�ؿ����ʱ��
			DisplayGameCompleteTime(GameBestTimeManager.Instance.GetGameUseTimeString());

			Time.timeScale = 0f;
		}
	}

	#endregion

	#region ��Ϸ���ʱ����ʾ���ʱ��� UI

	/// <summary>
	/// ��Ϸ���ʱ����ʾ���ʱ��� UI
	/// </summary>
	public void DisplayGameCompleteTime(string _timeText)
	{
		completeTimeUIText.text = string.Empty; // ���������
		completeTimeUIText.text = _timeText; // Ȼ��ֵ
	}

	#endregion
}
