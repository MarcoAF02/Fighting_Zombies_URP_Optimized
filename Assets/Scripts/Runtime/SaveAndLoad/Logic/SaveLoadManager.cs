using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using Newtonsoft.Json;

public class SaveLoadManager : MonoSingleton<SaveLoadManager>
{
	#region 基本组件和变量

	/// <summary>
	/// 用于存储 GamePlay 相关数据（需要加密）
	/// </summary>
	public List<GamePlayISaveable> gamePlaySaveableList = new List<GamePlayISaveable>();

	/// <summary>
	/// 用于存储 游戏设置 相关数据（不加密，可手改文件）
	/// </summary>
	public List<GameSettingsISaveable> gameSettingsSaveableList = new List<GameSettingsISaveable>();

	// TODO: 如果有很多条存档，DataSlot 就是一个列表
	public GamePlayDataSlot gamePlayDataSlot = new GamePlayDataSlot(); // 玩家 GamePlay 存档信息（记录每个关卡用了多少时间等）
	public GameSettingsDataSlot gameSettingsDataSlot = new GameSettingsDataSlot(); // 玩家 GameSettings 存档信息

	// GamePlay 存档文件名称
	private string jsonFolder_gamePlay = string.Empty;

	// GameSettings 存档文件名称
	private string jsonFolder_gameSettings = string.Empty;

	#endregion

	#region 观察者模式

	/// <summary>
	/// 观察者模式：注册存档（GamePlay 数据）
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
	/// 观察者模式：注册存档（GameSettings 数据）
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

	#region 基本生命周期函数

	protected override void Awake()
	{
		base.Awake();
		jsonFolder_gamePlay = Application.persistentDataPath + "/SAVE_DATA/"; // 生成一个存档路径
		jsonFolder_gameSettings = Application.persistentDataPath + "/GAME_SETTINGS/"; // 生成设置保存路径
		DontDestroyOnLoad(gameObject);
	}

	#endregion

	#region GamePlay 存档和读档功能实现

	/// <summary>
	/// 存档（游戏游玩内容）
	/// </summary>
	public void SaveGamePlayData()
	{
		GamePlayDataSlot dataSlot = new GamePlayDataSlot(); // 一条新存档

		// 遍历场景中所有需要存档的物体（观察者模式存入的 ISaveable 接口）
		for (int i = 0; i < gamePlaySaveableList.Count; i ++)
		{
			dataSlot.gamePlayDataDic.Add(gamePlaySaveableList[i].GUID, gamePlaySaveableList[i].SaveGamePlayInToData());
		}

		gamePlayDataSlot = dataSlot; // 正式记录存档

		// 将存档放进对应的目录（这个用于记录存档所在的目录）
		string resultPath = jsonFolder_gamePlay + "gameplayData.json"; // 手动补上拓展名
		
		// 序列化 JSON
		// TODO: 注意第二个参数，它代表着是否要乱序存档文本
		string jsonData = JsonConvert.SerializeObject(dataSlot, Formatting.Indented);

		if (!File.Exists(resultPath))
		{
			Directory.CreateDirectory(jsonFolder_gamePlay); // 如果没有创建过存档，就创建一个新存档（Json 文件）
		}

		try
		{
			File.WriteAllText(resultPath, jsonData);
			Debug.Log("存档成功");
		}
		catch
		{
			Debug.LogWarning("存档失败");
		}
	}

	/// <summary>
	/// 读档（游戏游玩内容）
	/// </summary>
	public void LoadGamePlayData()
	{
		// 注意这个最终路径必须和存档一致，否则会无法读取存档
		string resultPath = jsonFolder_gamePlay + "gameplayData.json";

		string stringData = string.Empty;
		try
		{
			// 根据上面的路径读取文件内的值并储存到变量 stringData 内
			stringData = File.ReadAllText(resultPath);
		}
		catch
		{
			Debug.Log("之前没有游戏存档，无需读取");
			return;
		}

		GamePlayDataSlot jsonData = JsonConvert.DeserializeObject<GamePlayDataSlot>(stringData); // 反序列化为 DataSlot 类

		// 向游戏中重新写入存档，这里使用 GUID 键得到值
		try
		{
			// 注册存档的时候是一股脑写进去的，取出数据还原就要通过 GUID 找到对应的物品
			for (int i = 0; i < gamePlaySaveableList.Count; i ++) // 这里得到的信息必须能和存档的信息对上
			{
				gamePlaySaveableList[i].LoadGamePlaySaveData(jsonData.gamePlayDataDic[gamePlaySaveableList[i].GUID]);
			}

			Debug.Log("玩家数据读档成功");
		}
		catch
		{
			Debug.LogWarning("玩家数据读档失败");
		}
	}

	#endregion

	#region GameSetting 存档和读档功能实现

	/// <summary>
	/// 存档（游戏设置内容）
	/// </summary>
	public void SaveGameSettingsData()
	{
		GameSettingsDataSlot dataSlot = new GameSettingsDataSlot(); // 老规矩

		// 遍历所有需要存档的设置文件
		for (int i = 0; i < gameSettingsSaveableList.Count; i ++)
		{
			dataSlot.gameSettingsDataDic.Add(gameSettingsSaveableList[i].GUID, gameSettingsSaveableList[i].SaveGameSettingsInToData());
		}

		gameSettingsDataSlot = dataSlot; // 记录存档

		string resultPath = jsonFolder_gameSettings + "gameSettings.json"; // 老规矩，手动补上拓展名

		// 序列化 JSON
		// TODO: 注意第二个参数，它代表着是否要乱序存档文本
		string jsonData = JsonConvert.SerializeObject(dataSlot, Formatting.Indented);

		if (!File.Exists(resultPath))
		{
			Directory.CreateDirectory(jsonFolder_gameSettings); // 如果没有创建过存档，就创建一个新存档（Json 文件）
		}

		try
		{
			File.WriteAllText(resultPath, jsonData);
			Debug.Log("游戏设置存档成功");
		}
		catch
		{
			Debug.LogWarning("游戏设置存档失败");
		}
	}

	/// <summary>
	/// 读档（游戏设置内容）
	/// </summary>
	public void LoadGameSettingsData()
	{
		// 必须和上方一致
		string resultPath = jsonFolder_gameSettings + "gameSettings.json";

		string stringData = string.Empty;
		try
		{
			// 根据上面的路径读取文件内的值并储存到变量 stringData 内
			stringData = File.ReadAllText(resultPath);
		}
		catch
		{
			Debug.Log("之前没有游戏存档，无需读取");
			return;
		}

		GameSettingsDataSlot jsonData = JsonConvert.DeserializeObject<GameSettingsDataSlot>(stringData); // 反序列化为 DataSlot 类

		// 向游戏中重新写入存档，这里使用 GUID 键得到值
		try
		{
			// 注册存档的时候是一股脑写进去的，取出数据还原就要通过 GUID 找到对应的物品
			for (int i = 0; i < gameSettingsSaveableList.Count; i++) // 这里得到的信息必须能和存档的信息对上
			{
				gameSettingsSaveableList[i].LoadGameSettingsSaveData(jsonData.gameSettingsDataDic[gameSettingsSaveableList[i].GUID]);
			}

			Debug.Log("游戏设置读档成功");
		}
		catch
		{
			Debug.LogWarning("游戏设置读档失败");
		}
	}

	#endregion

}
