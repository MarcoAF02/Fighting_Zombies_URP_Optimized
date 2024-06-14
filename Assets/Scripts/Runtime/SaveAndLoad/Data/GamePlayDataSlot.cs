using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayDataSlot
{
	// string 的内容是 GUID，SaveData 是每一个存档系统返回的 SaveData
	public Dictionary<string, GamePlaySaveDataList> gamePlayDataDic = new Dictionary<string, GamePlaySaveDataList>();
}
