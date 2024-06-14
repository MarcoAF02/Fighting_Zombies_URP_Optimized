// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Knife/Fire"
{
	Properties
	{
		_Noise("Noise", 2D) = "white" {}
		_Alpha("Alpha", 2D) = "white" {}
		[HDR]_Color0("Color 0", Color) = (1,1,1,1)
		[HDR]_Color1("Color 1", Color) = (1,1,1,1)
		_Opacity("Opacity", Range( 0 , 1)) = 1
		_NoiseSoftness("NoiseSoftness", Range( 0 , 1)) = 0
		_NoiseSpeed("NoiseSpeed", Vector) = (0,1,0,0)
		_DepthFade("DepthFade", Float) = 0
		_Rotation("Rotation", Float) = 1
		_Offset("Offset", Vector) = (1,0,0,0)
		_AlphaSoftness("AlphaSoftness", Range( 0 , 1)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Back
		ColorMask RGBA
		ZWrite Off
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;
			};

			uniform float4 _Color0;
			uniform float4 _Color1;
			uniform float _NoiseSoftness;
			uniform sampler2D _Noise;
			uniform float2 _NoiseSpeed;
			uniform float4 _Noise_ST;
			uniform float2 _Offset;
			uniform float _Rotation;
			uniform float _AlphaSoftness;
			uniform sampler2D _Alpha;
			uniform float4 _Alpha_ST;
			uniform float _Opacity;
			UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
			uniform float4 _CameraDepthTexture_TexelSize;
			uniform float _DepthFade;
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord2 = screenPos;
				
				o.ase_texcoord = v.ase_texcoord;
				o.ase_texcoord1.xy = v.ase_texcoord1.xy;
				o.ase_color = v.color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				float4 uv039 = i.ase_texcoord;
				uv039.xy = i.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float NoiseSoftnessMul60 = uv039.w;
				float2 uv0_Noise = i.ase_texcoord.xy * _Noise_ST.xy + _Noise_ST.zw;
				float2 uv163 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float cos37 = cos( ( uv039.z * _Rotation ) );
				float sin37 = sin( ( uv039.z * _Rotation ) );
				float2 rotator37 = mul( ( uv0_Noise + ( uv163 * _Offset ) ) - float2( 0.5,0.5 ) , float2x2( cos37 , -sin37 , sin37 , cos37 )) + float2( 0.5,0.5 );
				float2 panner24 = ( 1.0 * _Time.y * _NoiseSpeed + rotator37);
				float4 tex2DNode2 = tex2D( _Noise, panner24 );
				float smoothstepResult11 = smoothstep( 0.0 , ( NoiseSoftnessMul60 * _NoiseSoftness ) , tex2DNode2.r);
				float4 lerpResult9 = lerp( _Color0 , _Color1 , smoothstepResult11);
				float2 uv_Alpha = i.ase_texcoord.xy * _Alpha_ST.xy + _Alpha_ST.zw;
				float smoothstepResult69 = smoothstep( 0.0 , _AlphaSoftness , tex2D( _Alpha, uv_Alpha ).r);
				float4 screenPos = i.ase_texcoord2;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth50 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
				float distanceDepth50 = abs( ( screenDepth50 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthFade ) );
				float clampResult52 = clamp( distanceDepth50 , 0.0 , 1.0 );
				float smoothstepResult58 = smoothstep( 0.0 , 1.0 , tex2DNode2.r);
				
				
				finalColor = ( lerpResult9 * ( smoothstepResult69 * _Opacity * clampResult52 * ( smoothstepResult69 - smoothstepResult58 ) ) * i.ase_color );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17000
7;7;1844;1044;1727.302;1206.272;1.6;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;63;-2980.21,-338.5386;Float;False;1;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;65;-2895.64,-232.0716;Float;False;Property;_Offset;Offset;9;0;Create;True;0;0;False;0;1,0;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;39;-2612.864,-169.7409;Float;False;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;38;-2697.164,-501.3408;Float;False;0;2;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-2725.64,-276.0716;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;59;-2595.706,54.99451;Float;False;Property;_Rotation;Rotation;8;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;64;-2462.64,-400.0716;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-2356.764,-81.34088;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;25;-1909.564,-72.24103;Float;False;Property;_NoiseSpeed;NoiseSpeed;6;0;Create;True;0;0;False;0;0,1;0,-0.8;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RotatorNode;37;-2120.164,-246.4408;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;60;-2342.706,27.99451;Float;False;NoiseSoftnessMul;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;24;-1618.364,-180.141;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;2;-1436.3,-246.6;Float;True;Property;_Noise;Noise;0;0;Create;True;0;0;False;0;None;e28dc97a9541e3642a48c0e3886688c5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;67;-1306.498,383.3688;Float;False;Property;_AlphaSoftness;AlphaSoftness;10;0;Create;True;0;0;False;0;1;0.179;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-1326,152;Float;True;Property;_Alpha;Alpha;1;0;Create;True;0;0;False;0;None;0000000000000000f000000000000000;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;-1587.519,148.1208;Float;False;Property;_NoiseSoftness;NoiseSoftness;5;0;Create;True;0;0;False;0;0;0.562;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;61;-1601.706,54.99451;Float;False;60;NoiseSoftnessMul;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-1072.198,657.0425;Float;False;Property;_DepthFade;DepthFade;7;0;Create;True;0;0;False;0;0;0.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-1274.706,74.99451;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;58;-995.1234,-104.3162;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;50;-884.198,589.0425;Float;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;69;-979.498,177.3688;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;55;-641.1234,13.68384;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;52;-576.198,573.0425;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-836,407;Float;False;Property;_Opacity;Opacity;4;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;5;-1153.8,-674.3;Float;False;Property;_Color1;Color 1;3;1;[HDR];Create;True;0;0;False;0;1,1,1,1;0.9716981,0.1240466,0,0.145098;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;11;-979.5187,-352.8792;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;4;-1147.8,-830.3;Float;False;Property;_Color0;Color 0;2;1;[HDR];Create;True;0;0;False;0;1,1,1,1;33.89676,7.631207,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;9;-647.9,-640.7999;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-475,312;Float;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;14;-472.519,131.1208;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;106.481,-47.87921;Float;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;345,-33;Float;False;True;2;Float;ASEMaterialInspector;0;1;Knife/Fire;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;False;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;2;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;2;0;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;66;0;63;0
WireConnection;66;1;65;0
WireConnection;64;0;38;0
WireConnection;64;1;66;0
WireConnection;41;0;39;3
WireConnection;41;1;59;0
WireConnection;37;0;64;0
WireConnection;37;2;41;0
WireConnection;60;0;39;4
WireConnection;24;0;37;0
WireConnection;24;2;25;0
WireConnection;2;1;24;0
WireConnection;62;0;61;0
WireConnection;62;1;12;0
WireConnection;58;0;2;1
WireConnection;50;0;51;0
WireConnection;69;0;3;1
WireConnection;69;2;67;0
WireConnection;55;0;69;0
WireConnection;55;1;58;0
WireConnection;52;0;50;0
WireConnection;11;0;2;1
WireConnection;11;2;62;0
WireConnection;9;0;4;0
WireConnection;9;1;5;0
WireConnection;9;2;11;0
WireConnection;8;0;69;0
WireConnection;8;1;7;0
WireConnection;8;2;52;0
WireConnection;8;3;55;0
WireConnection;13;0;9;0
WireConnection;13;1;8;0
WireConnection;13;2;14;0
WireConnection;1;0;13;0
ASEEND*/
//CHKSM=46CF76F923FE01B0977D2B2ADA96920C1035269D