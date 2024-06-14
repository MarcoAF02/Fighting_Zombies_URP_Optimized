using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ϸʧ�ܲ˵�
/// </summary>
public class GameFailMenu : MonoBehaviour
{
	#region ��������ͱ���

	[Header("ʧ�ܽ��������")]
	[SerializeField] private GameObject gameFailMenuRoot;
	[Header("��Ϸʧ�ܺ�������ʾʧ�� UI")]
	[SerializeField] private float showUIIntervalTime;

	// Э��
	private Coroutine displayGameFailMenu_IECor;

	#endregion

	#region �����������ں���

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
			Debug.Log("��Ϸ���̽���������ȡ�������¼�");
		}
	}

	#endregion

	#region �˵���ʾ����

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
