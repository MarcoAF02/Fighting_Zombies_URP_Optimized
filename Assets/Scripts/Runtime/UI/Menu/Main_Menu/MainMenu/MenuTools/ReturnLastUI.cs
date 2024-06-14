using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������һ�� UI �Ŀ�����
/// </summary>
public class ReturnLastUI : MonoBehaviour
{
	#region ��������ͱ���

	[Header("ģʽ�ؿ�չʾ����")]
	[SerializeField] private GameObject gameShowBGObj;

	private FPS_Play_Action fpsPlayAction;

	#endregion

	#region �����������ں���

	private void Awake()
	{
		fpsPlayAction = new FPS_Play_Action();
		fpsPlayAction.Enable();
	}

	private void Update()
	{
		if (fpsPlayAction.UI_Keyboard_And_Mouse.Mouse_Cancel.WasPressedThisFrame())
		{
			if (MainMenu.Instance.returnCommandStack.Count > 0)
			{
				if (MainMenu.Instance.returnCommandStack.Count == 1) // �����ʾ����һ����Ҫ�������˵������عؿ�չʾ������������û�йؿ�չʾ������
				{
					gameShowBGObj.SetActive(false);
				}

				MainMenu.Instance.gameShowController.ClearShowImage(); // һ��������ؼ����ͻ��������ͼƬ����
				ReturnCommand returnCommand = MainMenu.Instance.returnCommandStack.Pop();
				PerformReturnLastUI(returnCommand);
			}
			else
			{
				Debug.Log("���������˵����Ҽ����ܷ���");
			}
		}
	}

	private void OnDestroy()
	{
		fpsPlayAction.Disable();
	}

	#endregion

	#region ���ع���ʵ��

	/// <summary>
	/// �����ϼ��˵�
	/// </summary>
	private void PerformReturnLastUI(ReturnCommand command)
	{
		MenuOperateSound.Instance.PlayCancelSound();

		if (command._hideObj != null) command._hideObj.SetActive(false);
		if (command._displayObj != null) command._displayObj.SetActive(true);

		if (command._saveSettingsData) // Ӧ�ò���������
		{
			try
			{
				SettingsLoader.Instance.ApplyAllGraphicsSettings(false);
				SettingsLoader.Instance.ApplyAllAudioSettings(false); // Ӧ������һ��������
				SaveLoadManager.Instance.SaveGameSettingsData(); // ��������
			}
			catch
			{
				Debug.LogWarning("û���ҵ����ü�������浵������");
			}
		}
	}

	#endregion
}
