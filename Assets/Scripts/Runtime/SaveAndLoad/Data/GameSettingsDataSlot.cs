using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsDataSlot
{
	// string 的内容是 GUID，SaveData 是每一个存档系统返回的 SaveData
	public Dictionary<string, GameSettingsToUseList> gameSettingsDataDic = new Dictionary<string, GameSettingsToUseList>();
}
