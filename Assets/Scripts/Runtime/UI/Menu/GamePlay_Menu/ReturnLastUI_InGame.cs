using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ϸ�ڵ���ͣ�˵����ؿ����������ڿ�����Ϸ�ڵĲ˵� UI
/// </summary>
public class ReturnLastUI_InGame : MonoBehaviour
{
	#region ��������ͱ���

	private FPS_Play_Action fpsPlayAction; // ����ϵͳ

	[Header("������ü�����")]
	[SerializeField] private PlayerSettingsLoader playerSettingsLoader;
	[Header("�����ͣ�˵�������")]
	public PauseMenu pauseMenu;
	[Header("UI ����ָ��ջ")]
	public Stack<ReturnCommand> returnCommandStackInGame = new Stack<ReturnCommand>();

	#endregion

	#region �����������ں���

	private void Awake()
	{
		fpsPlayAction = new FPS_Play_Action();
		fpsPlayAction.Enable();
	}

	private void OnDestroy()
	{
		fpsPlayAction?.Disable();
	}

	#endregion

	#region �˵����ع���ʵ��

	/// <summary>
	/// �˵����ع��ܣ�����ջ��ָ������ UI ����ʾ�����أ�
	/// </summary>
	public void ReturnLastMenuUI()
	{
		if (fpsPlayAction.UI_Keyboard_And_Mouse.Player_OpenPauseMenu.WasPressedThisFrame() ||
		fpsPlayAction.UI_Keyboard_And_Mouse.Mouse_Cancel.WasPressedThisFrame())
		{
			if (returnCommandStackInGame.Count <= 1) // ����ָ���ջ
			{
				// �˵��������壩�򿪺͹رյ���Ч�� FSM ״̬�����ࣩ��
				pauseMenu.ClosePauseMenu(); // ֱ�ӹرղ˵�
				return;
			}

			ReturnCommand returnCommand = returnCommandStackInGame.Pop();
			PerformReturnLastUI(returnCommand);

			try
			{
				if (returnCommandStackInGame.Count > 1) MenuOperateSound.Instance.PlayCancelSound(); // ���ŷ��ص���Ч
			}
			catch
			{
				Debug.LogWarning("������û����Ч����������������Ϊ�������ǵ������ģ������Ǵ����˵����ؽ����");
			}
		}
	}
	
	// �����ϼ� UI ʵ��ִ��
	public void PerformReturnLastUI(ReturnCommand _returnCommand)
	{
		if (_returnCommand._displayObj != null) _returnCommand._displayObj.SetActive(true);
		if (_returnCommand._hideObj != null) _returnCommand._hideObj.SetActive(false);

		if (_returnCommand._saveSettingsData) // Ӧ�ò���������
		{
			try
			{
				SettingsLoader.Instance.ApplyAllGraphicsSettings(false);
				SettingsLoader.Instance.ApplyAllAudioSettings(false); // Ӧ������һ��������
				SaveLoadManager.Instance.SaveGameSettingsData();

				playerSettingsLoader.LoadGameSettings();
			}
			catch
			{
				Debug.LogWarning("û���ҵ����ü�������浵������");
			}
		}
	}

	#endregion
}
