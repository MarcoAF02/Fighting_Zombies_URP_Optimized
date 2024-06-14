using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GamePlayISaveable
{
	//FIXME: https://docs.microsoft.com/zh-cn/dotnet/csharp/language-reference/keywords/get
	string GUID { get; } // 生成只读的 GUID

	GamePlaySaveDataList SaveGamePlayInToData(); // 将数据保存的方法

	void LoadGamePlaySaveData(GamePlaySaveDataList gamePlaySaveData); // 将保存的数据加载出来
}

public interface GameSettingsISaveable
{
	string GUID { get; }

	GameSettingsToUseList SaveGameSettingsInToData();

	void LoadGameSettingsSaveData(GameSettingsToUseList gameSettingsSaveData);
}
