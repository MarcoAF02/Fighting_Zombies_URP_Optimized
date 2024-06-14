using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 展示一个关卡的最佳完成时间
/// </summary>
public class ShowLevelBestTime : MonoBehaviour
{
	#region 基本组件和变量

	[Header("单位语言")]
	public string minString = "分";
	public string secString = "秒";

	[Header("按钮对应的关卡名称（需要手动填写）")]
	[SerializeField] private string levelName;
	[Header("展示文本")]
	[SerializeField] private TextMeshProUGUI bestTimeTMP;

	#endregion

	#region 关卡完成时间 UI 显示功能

	/// <summary>
	/// 显示关卡最佳完成时间
	/// </summary>
	public void ShowLevelBestTimeOnUI()
	{
		// 先把对应的关卡名称找出来
		SaveLoadManager.Instance.LoadGamePlayData(); // 读档

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
