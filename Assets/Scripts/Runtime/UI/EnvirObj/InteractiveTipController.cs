using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveTipController : MonoBehaviour
{
	#region 基本组件和变量

	[Header("提示图标根物体")]
	[SerializeField] private Transform interactiveTipObj;
	[Header("玩家射线检测点")]
	[SerializeField] private Transform checkPointTrans;
	[Header("玩家射线检测范围")]
	[Tooltip("玩家距离多近时显示交互提示")]
	[SerializeField] private float checkRadius;
	[Header("玩家的 Layer")]
	[SerializeField] private int playerLayerIndex;
	[Header("玩家距离多近时隐藏交互提示")]
	[SerializeField] private float hideDistance;
	[Header("检测玩家时抬升的值")]
	[SerializeField] private float lookupValue;

	#endregion

	#region 基本生命周期函数

	private void Update()
	{
		ShowInteractiveTip();

		// 修复异常旋转
		interactiveTipObj.localEulerAngles = new Vector3(0f, interactiveTipObj.localEulerAngles.y, 0f);

		// TODO: 以下几种方式都可以控制旋转，区别在于是使用三维向量还是欧拉角（不能直接修改四元数）
		// transform.localEulerAngles = new Vector3(x, y, z);
		// transform.eulerAngles = new Vector3(x, y, z);

		// transform.localRotation = Quaternion.Eular(x, y, z);
		// transform.rotation = Quaternion.Eular(x, y, z);
	}

	#endregion

	#region 显示和隐藏交互提示

	private void ShowInteractiveTip()
	{
		Collider[] playerArray = Physics.OverlapSphere(checkPointTrans.position, checkRadius, 1 << playerLayerIndex);
		
		if (playerArray.Length == 0)
		{
			interactiveTipObj.gameObject.SetActive(false);
		}

		if (playerArray.Length == 1)
		{
			if (Vector3.Distance(playerArray[0].transform.position, interactiveTipObj.position) < hideDistance + lookupValue)
			{
				interactiveTipObj.gameObject.SetActive(false);
			}
			else
			{
				interactiveTipObj.gameObject.SetActive(true);
			}

			Vector3 lookDir = (playerArray[0].transform.position - interactiveTipObj.position).normalized + new Vector3(0, lookupValue, 0);
			interactiveTipObj.rotation = Quaternion.LookRotation(lookDir);
		}

		if (playerArray.Length > 1)
		{
			Debug.LogWarning("当前是单机游戏，不应该有多名玩家");
		}
	}

	#endregion

}
