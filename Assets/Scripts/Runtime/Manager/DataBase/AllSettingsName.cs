using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所有设置的名称
/// </summary>
[Serializable]
public class AllSettingsName
{
	[Header("所有设置选项的名称")]

	[Header("游戏设置")]
	[Header("鼠标灵敏度")]
	public string mouseSensitivity = "Mouse_Sensitivity";
	[Header("准星")]
	public string frontSight = "Front_Sight";
	[Header("视野角度")]
	public string fieldOfView = "Field_Of_View";
	[Header("屏幕受伤提示")]
	public string screenInjuryWarning = "Screen_Injury_Warning";
	[Header("视角摇晃")]
	public string shakingPerspective = "Shaking_Perspective";
	[Header("显示教程信息")]
	public string displayTutorialInformation = "Display_Tutorial_Information";

	[Header("图形设置")]
	[Header("显示模式")]
	public string displayMode = "Display_Mode";
	[Header("分辨率")]
	public string resolutionRatio = "Resolution_Ratio";
	[Header("渲染倍数")]
	public string renderScale = "Render_Scale";
	[Header("最大帧率")]
	public string maximumFrameRate = "Maximum_Frame_Rate";
	[Header("垂直同步")]
	public string vSync = "VSync";
	[Header("后期处理抗锯齿")]
	public string postProcessingAntiAliasing = "Post_Processing_Anti_Aliasing";

	[Header("阴影质量")]
	public string shadowQuality = "Shadow_Quality";

	[Obsolete("已停止支持对阴影分辨率的动态调整，阴影质量的调整功能现在在一个选项里")]
	[Header("平行光阴影质量")]
	public string directionalLightShadowQuality = "Directional_Light_Shadow_Quality";

	[Obsolete("已停止支持对阴影分辨率的动态调整，阴影质量的调整功能现在在一个选项里")]
	[Header("场景光阴影质量")]
	public string sceneLightShadowQuality = "Scene_Light_Shadow_Quality";

	[Obsolete("已停止支持对阴影分辨率的动态调整，阴影质量的调整功能现在在一个选项里")]
	[Header("阴影渲染距离")]
	public string shadowRenderDistance = "Shadow_Render_Distance";

	[Header("纹理质量")]
	public string textureQuality = "Texture_Quality";
	[Header("环境光遮蔽")]
	public string ambientOcclusion = "Ambient_Occlusion";
	[Header("纹理过滤")]
	public string textureFiltering = "Texture_Filtering";

	[Header("音频设置")]
	[Header("全局音量")]
	public string globalVolume = "Global_Volume";
	[Header("音乐音量")]
	public string bgmVolume = "Music_Volume";
	[Header("音效音量")]
	public string sfxVolume = "SFX_Volume";
	[Header("系统音量")]
	public string systemSFXVolume = "System_SFX_Volume";

}
