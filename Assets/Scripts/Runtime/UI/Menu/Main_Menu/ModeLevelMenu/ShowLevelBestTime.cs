using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// չʾһ���ؿ���������ʱ��
/// </summary>
public class ShowLevelBestTime : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��λ����")]
	public string minString = "��";
	public string secString = "��";

	[Header("��ť��Ӧ�Ĺؿ����ƣ���Ҫ�ֶ���д��")]
	[SerializeField] private string levelName;
	[Header("չʾ�ı�")]
	[SerializeField] private TextMeshProUGUI bestTimeTMP;

	#endregion

	#region �ؿ����ʱ�� UI ��ʾ����

	/// <summary>
	/// ��ʾ�ؿ�������ʱ��
	/// </summary>
	public void ShowLevelBestTimeOnUI()
	{
		// �ȰѶ�Ӧ�Ĺؿ������ҳ���
		SaveLoadManager.Instance.LoadGamePlayData(); // ����

		if (GameBestTimeManager.Instance.currentLevelPlayData.Count == 0)
		{
			bestTimeTMP.text = string.Empty;
		}
		else
		{
			for (int i = 0; i < GameBestTimeManager.Instance.currentLevelPlayData.Count; i++)
			{
				if (GameBestTimeManager.Instance.currentLevelPlayData[i].sceneName == levelName)
				{
					string _time = GameBestTimeManager.Instance.currentLevelPlayData[i].useMinute.ToString() + " " + minString + " " + GameBestTimeManager.Instance.currentLevelPlayData[i].useSecond + " " + secString;
					bestTimeTMP.text = _time;

					return;
				}
			}

			bestTimeTMP.text = string.Empty;
		}
	}

	#endregion
}
