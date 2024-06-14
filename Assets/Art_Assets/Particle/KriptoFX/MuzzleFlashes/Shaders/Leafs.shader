Shader "KriptoFX/FPS_Pack/Leafs" {
	Properties{
		[HDR]_TintColor("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex("Particle Texture", 2D) = "white" {}
		_InvFade("Soft Particles Factor", Range(0.01,5)) = 1.0
	}




		SubShader{
			Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "LightMode" = "ForwardBase"}
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
			ZWrite On
		Pass{

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define FORWARD_BASE_PASS
#pragma multi_compile_fog

#include "UnityCG.cginc"
#include "Lighting.cginc"


		sampler2D _MainTex;
	fixed4 _TintColor;

	struct appdata_t {
		float4 vertex : POSITION;
		fixed4 color : COLOR;
		float2 texcoord : TEXCOORD0;
		half3 normal:NORMAL;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f {
		float4 vertex : SV_POSITION;
		fixed4 color : COLOR;
		float2 texcoord : TEXCOORD0;

		UNITY_FOG_COORDS(3)

		UNITY_VERTEX_OUTPUT_STEREO
	};

	float4 _MainTex_ST;

	half3 ShadeTranslucentLights(float4 vertex)
	{
		float3 normal = float3(0, 1, 0);
		half3 otherLights = ShadeSH9(float4(normal, 1.0));

		//#ifdef VERTEXLIGHT_ON
		float3 worldPos = mul(unity_ObjectToWorld, vertex).xyz;
		otherLights += Shade4PointLights(
			unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
			unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,
			unity_4LightAtten0, worldPos, normal);
		//#endif

		return saturate(otherLights * 1.5 + _LightColor0.rgb);
	}

	v2f vert(appdata_t v)
	{
		v2f o;
		UNITY_SETUP_INSTANCE_ID(v); //Insert
		UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert
		o.vertex = UnityObjectToClipPos(v.vertex);

		o.color = v.color * _TintColor;
		o.color.rgb *= ShadeTranslucentLights(v.vertex);

		o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
		UNITY_TRANSFER_FOG(o,o.vertex);
		return o;
	}

	sampler2D_float _CameraDepthTexture;
	float _InvFade;

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 col = tex2D(_MainTex, i.texcoord);
		col *= 2.0f * i.color;
		UNITY_APPLY_FOG(i.fogCoord, col);
		col.a = saturate(col.a);
		return col;
	}
		ENDCG
	}

			Pass
	{
		Tags{ "Queue" = "Transparent" "LightMode" = "ShadowCaster" }

		CGPROGRAM

#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_shadowcaster
#pragma fragmentoption ARB_precision_hint_fastest

#include "UnityCG.cginc"

	sampler2D _MainTex;
	float4 _MainTex_ST;

	struct appdata
	{
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		half3 normal : NORMAL;
	};


	struct v2f
	{
		float2 texcoord : TEXCOORD3;
		V2F_SHADOW_CASTER;
	};

	v2f vert(appdata v)
	{
		v2f o;
		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
		TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
		return o;
	}

	float4 frag(v2f i) : COLOR
	{
		fixed col = tex2D(_MainTex, i.texcoord).a * 2;
		if (col < 0.01) discard;
		SHADOW_CASTER_FRAGMENT(i)
	}

		ENDCG
	}


	}

}
