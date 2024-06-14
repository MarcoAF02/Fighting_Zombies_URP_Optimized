using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��Ϸ��ɵ� UI ����
/// </summary>
public class GameOverBG : MonoBehaviour
{
	#region ��������ͱ���

	[Header("����ֵ����������")]
	[SerializeField] private HealthBGController healthBGController;
	[Header("��ϷĿ�걳��ͼƬ")]
	[SerializeField] private Image gameOverBGImage;
	[Header("��Ϸͨ��ʱ������Ļ����ɫ")]
	[SerializeField] private Color completeBGColor;
	[Header("��Ϸʧ��ʱ������Ļ����ɫ")]
	[SerializeField] private Color failBGColor;
	[Header("������Ļ��ɫ�ı���ٶ�")]
	[SerializeField] private float bgChangeSpeed;

	// Э��
	private Coroutine changeBGWhenFail_IECor;

	#endregion

	#region �����������ں���

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
			Debug.Log("��Ϸ���̽���������ȡ�������¼�");
		}
	}

	#endregion

	#region ��Ϸʧ�ܻ�ͨ��ʱ�ı䱳����Ļ

	/// <summary>
	/// ��Ϸʧ��ʱ������Ļ���
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
		if (_gameProgress == GameProgress.EarlyStage) // ��Ϸ��ʼʱ���ñ�����ɫ
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
