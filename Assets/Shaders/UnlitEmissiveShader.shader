Shader "Jam/SaveZone"
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "white" {}
		_EmissiveTex ("EmissiveTexture", 2D) = "white" {}
		_Tint("Tint", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }


		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			ZTest Always

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};


			sampler2D _MainTex;
			sampler2D _EmissiveTex;
			float4 _Tint;


			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				float4 emissive = tex2D(_EmissiveTex, i.uv).rgba;
				float4 color = tex2D(_MainTex, i.uv).rgba;

	

				return color * color.a * lerp(1.0f, 0.0f, i.uv.y) * _Tint + float4(emissive.rgb, 0.0f);
			}
			ENDCG
		}
	}
}
