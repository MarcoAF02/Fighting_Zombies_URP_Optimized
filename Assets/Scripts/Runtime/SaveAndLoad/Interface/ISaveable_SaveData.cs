using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GamePlayISaveable
{
	//FIXME: https://docs.microsoft.com/zh-cn/dotnet/csharp/language-reference/keywords/get
	string GUID { get; } // ����ֻ���� GUID

	GamePlaySaveDataList SaveGamePlayInToData(); // �����ݱ���ķ���

	void LoadGamePlaySaveData(GamePlaySaveDataList gamePlaySaveData); // ����������ݼ��س���
}

public interface GameSettingsISaveable
{
	string GUID { get; }

	GameSettingsToUseList SaveGameSettingsInToData();

	void LoadGameSettingsSaveData(GameSettingsToUseList gameSettingsSaveData);
}
