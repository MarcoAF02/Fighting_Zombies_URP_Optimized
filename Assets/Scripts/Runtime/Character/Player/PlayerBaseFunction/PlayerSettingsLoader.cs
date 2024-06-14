using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ö�ȡ����
/// </summary>
public class PlayerSettingsLoader : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��ҿ�����")]
	[SerializeField] private PlayerController playerController;
	[Header("�̳���Ϣ������")]
	[SerializeField] private TutorialTipTrigger tutorialTipTrigger;
	[Header("������������������")]
	[SerializeField] private PlayerCameraAnim playerCameraAnim;

	// TODO: δ���ĸ��£�׼���� ��״����̬ʮ�֣���̬ʮ��
	[Header("���׼�ǣ��� UI �ϣ�")]
	[SerializeField] private GameObject frontSight;

	// TODO: δ���ĸ��£�����Ѫ��
	[Header("�������ֵ��ʾ UI")]
	[SerializeField] private HealthBGController healthBGController;

	// Э��
	private Coroutine applyAllGraphicsSettingsWhenStart_IECor;

	#endregion

	#region �����������ں���

	private void Start()
	{
		LoadGameSettings();
	}

	#endregion

	#region ��ȡ��Ϸ����

	// ��ȡ��Ϸ���ã���Щ���ò���ȫ�ֵģ�ֻ������Լ��й�
	public void LoadGameSettings()
	{
		if (SettingsLoader.Instance == null)
		{
			Debug.LogWarning("������û�����ü���������������Ϊ�������ǵ������ģ������Ǵ����˵����ؽ����");
			return;
		}

		// �����
		playerController.playerCameraController.mouseSpeed = SettingsLoader.Instance.gameSettingsManager.LoadMouseSensitivity();
		playerController.playerCameraController.normalFOV = SettingsLoader.Instance.gameSettingsManager.LoadCameraFOV();
		playerController.playerCameraController.CalculateCameraFOV(); // ��������� FOV

		// ���������
		playerCameraAnim.turnOnThePerspectiveShake = SettingsLoader.Instance.gameSettingsManager.LoadPerspectiveShake();

		// ���׼��
		frontSight.SetActive(SettingsLoader.Instance.gameSettingsManager.LoadFrontSight());

		// TODO: �����ֵĿǰ��д���ģ�δ�����Կ�������㷨����������ֵ...
		if (SettingsLoader.Instance.gameSettingsManager.LoadScreenInjuryWarning() == 0)
		{
			healthBGController.takeDamageBGMaxOpacity = 0.4f;
			healthBGController.takeDamageBGChangeProportion = 0.013f;
		}
		if (SettingsLoader.Instance.gameSettingsManager.LoadScreenInjuryWarning() == 1)
		{
			healthBGController.takeDamageBGMaxOpacity = 0.2f;
			healthBGController.takeDamageBGChangeProportion = 0.017f;
		}

		// �������������Ļ��ʾ
		healthBGController.FixTakeDamageBG();

		// �Ƿ���ʾ�̳���Ϣ
		tutorialTipTrigger.openTutorial = SettingsLoader.Instance.gameSettingsManager.LoadDisplayTutorialInformation();
	}

	#endregion

}
