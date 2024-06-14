using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using Newtonsoft.Json;

public class SaveLoadManager : MonoSingleton<SaveLoadManager>
{
	#region ��������ͱ���

	/// <summary>
	/// ���ڴ洢 GamePlay ������ݣ���Ҫ���ܣ�
	/// </summary>
	public List<GamePlayISaveable> gamePlaySaveableList = new List<GamePlayISaveable>();

	/// <summary>
	/// ���ڴ洢 ��Ϸ���� ������ݣ������ܣ����ָ��ļ���
	/// </summary>
	public List<GameSettingsISaveable> gameSettingsSaveableList = new List<GameSettingsISaveable>();

	// TODO: ����кܶ����浵��DataSlot ����һ���б�
	public GamePlayDataSlot gamePlayDataSlot = new GamePlayDataSlot(); // ��� GamePlay �浵��Ϣ����¼ÿ���ؿ����˶���ʱ��ȣ�
	public GameSettingsDataSlot gameSettingsDataSlot = new GameSettingsDataSlot(); // ��� GameSettings �浵��Ϣ

	// GamePlay �浵�ļ�����
	private string jsonFolder_gamePlay = string.Empty;

	// GameSettings �浵�ļ�����
	private string jsonFolder_gameSettings = string.Empty;

	#endregion

	#region �۲���ģʽ

	/// <summary>
	/// �۲���ģʽ��ע��浵��GamePlay ���ݣ�
	/// </summary>
	/// <param name="saveable"></param>
	public void RegisterGamePlaySaveable(GamePlayISaveable saveable)
	{
		if (!gamePlaySaveableList.Contains(saveable))
		{
			gamePlaySaveableList.Add(saveable);
		}
	}

	/// <summary>
	/// �۲���ģʽ��ע��浵��GameSettings ���ݣ�
	/// </summary>
	/// <param name="saveable"></param>
	public void RegisterGameSettingsSaveable(GameSettingsISaveable saveable)
	{
		if (!gameSettingsSaveableList.Contains(saveable))
		{
			gameSettingsSaveableList.Add(saveable);
		}
	}

	#endregion

	#region �����������ں���

	protected override void Awake()
	{
		base.Awake();
		jsonFolder_gamePlay = Application.persistentDataPath + "/SAVE_DATA/"; // ����һ���浵·��
		jsonFolder_gameSettings = Application.persistentDataPath + "/GAME_SETTINGS/"; // �������ñ���·��
		DontDestroyOnLoad(gameObject);
	}

	#endregion

	#region GamePlay �浵�Ͷ�������ʵ��

	/// <summary>
	/// �浵����Ϸ�������ݣ�
	/// </summary>
	public void SaveGamePlayData()
	{
		GamePlayDataSlot dataSlot = new GamePlayDataSlot(); // һ���´浵

		// ����������������Ҫ�浵�����壨�۲���ģʽ����� ISaveable �ӿڣ�
		for (int i = 0; i < gamePlaySaveableList.Count; i ++)
		{
			dataSlot.gamePlayDataDic.Add(gamePlaySaveableList[i].GUID, gamePlaySaveableList[i].SaveGamePlayInToData());
		}

		gamePlayDataSlot = dataSlot; // ��ʽ��¼�浵

		// ���浵�Ž���Ӧ��Ŀ¼��������ڼ�¼�浵���ڵ�Ŀ¼��
		string resultPath = jsonFolder_gamePlay + "gameplayData.json"; // �ֶ�������չ��
		
		// ���л� JSON
		// TODO: ע��ڶ������������������Ƿ�Ҫ����浵�ı�
		string jsonData = JsonConvert.SerializeObject(dataSlot, Formatting.Indented);

		if (!File.Exists(resultPath))
		{
			Directory.CreateDirectory(jsonFolder_gamePlay); // ���û�д������浵���ʹ���һ���´浵��Json �ļ���
		}

		try
		{
			File.WriteAllText(resultPath, jsonData);
			Debug.Log("�浵�ɹ�");
		}
		catch
		{
			Debug.LogWarning("�浵ʧ��");
		}
	}

	/// <summary>
	/// ��������Ϸ�������ݣ�
	/// </summary>
	public void LoadGamePlayData()
	{
		// ע���������·������ʹ浵һ�£�������޷���ȡ�浵
		string resultPath = jsonFolder_gamePlay + "gameplayData.json";

		string stringData = string.Empty;
		try
		{
			// ���������·����ȡ�ļ��ڵ�ֵ�����浽���� stringData ��
			stringData = File.ReadAllText(resultPath);
		}
		catch
		{
			Debug.Log("֮ǰû����Ϸ�浵�������ȡ");
			return;
		}

		GamePlayDataSlot jsonData = JsonConvert.DeserializeObject<GamePlayDataSlot>(stringData); // �����л�Ϊ DataSlot ��

		// ����Ϸ������д��浵������ʹ�� GUID ���õ�ֵ
		try
		{
			// ע��浵��ʱ����һ����д��ȥ�ģ�ȡ�����ݻ�ԭ��Ҫͨ�� GUID �ҵ���Ӧ����Ʒ
			for (int i = 0; i < gamePlaySaveableList.Count; i ++) // ����õ�����Ϣ�����ܺʹ浵����Ϣ����
			{
				gamePlaySaveableList[i].LoadGamePlaySaveData(jsonData.gamePlayDataDic[gamePlaySaveableList[i].GUID]);
			}

			Debug.Log("������ݶ����ɹ�");
		}
		catch
		{
			Debug.LogWarning("������ݶ���ʧ��");
		}
	}

	#endregion

	#region GameSetting �浵�Ͷ�������ʵ��

	/// <summary>
	/// �浵����Ϸ�������ݣ�
	/// </summary>
	public void SaveGameSettingsData()
	{
		GameSettingsDataSlot dataSlot = new GameSettingsDataSlot(); // �Ϲ��

		// ����������Ҫ�浵�������ļ�
		for (int i = 0; i < gameSettingsSaveableList.Count; i ++)
		{
			dataSlot.gameSettingsDataDic.Add(gameSettingsSaveableList[i].GUID, gameSettingsSaveableList[i].SaveGameSettingsInToData());
		}

		gameSettingsDataSlot = dataSlot; // ��¼�浵

		string resultPath = jsonFolder_gameSettings + "gameSettings.json"; // �Ϲ�أ��ֶ�������չ��

		// ���л� JSON
		// TODO: ע��ڶ������������������Ƿ�Ҫ����浵�ı�
		string jsonData = JsonConvert.SerializeObject(dataSlot, Formatting.Indented);

		if (!File.Exists(resultPath))
		{
			Directory.CreateDirectory(jsonFolder_gameSettings); // ���û�д������浵���ʹ���һ���´浵��Json �ļ���
		}

		try
		{
			File.WriteAllText(resultPath, jsonData);
			Debug.Log("��Ϸ���ô浵�ɹ�");
		}
		catch
		{
			Debug.LogWarning("��Ϸ���ô浵ʧ��");
		}
	}

	/// <summary>
	/// ��������Ϸ�������ݣ�
	/// </summary>
	public void LoadGameSettingsData()
	{
		// ������Ϸ�һ��
		string resultPath = jsonFolder_gameSettings + "gameSettings.json";

		string stringData = string.Empty;
		try
		{
			// ���������·����ȡ�ļ��ڵ�ֵ�����浽���� stringData ��
			stringData = File.ReadAllText(resultPath);
		}
		catch
		{
			Debug.Log("֮ǰû����Ϸ�浵�������ȡ");
			return;
		}

		GameSettingsDataSlot jsonData = JsonConvert.DeserializeObject<GameSettingsDataSlot>(stringData); // �����л�Ϊ DataSlot ��

		// ����Ϸ������д��浵������ʹ�� GUID ���õ�ֵ
		try
		{
			// ע��浵��ʱ����һ����д��ȥ�ģ�ȡ�����ݻ�ԭ��Ҫͨ�� GUID �ҵ���Ӧ����Ʒ
			for (int i = 0; i < gameSettingsSaveableList.Count; i++) // ����õ�����Ϣ�����ܺʹ浵����Ϣ����
			{
				gameSettingsSaveableList[i].LoadGameSettingsSaveData(jsonData.gameSettingsDataDic[gameSettingsSaveableList[i].GUID]);
			}

			Debug.Log("��Ϸ���ö����ɹ�");
		}
		catch
		{
			Debug.LogWarning("��Ϸ���ö���ʧ��");
		}
	}

	#endregion

}
