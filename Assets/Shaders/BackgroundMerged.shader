Shader "Jam/Background"
{
	Properties
	{
		_TopTex("Top Tex", 2D) = "white" {}
		_MidTex("Mid Tex", 2D) = "white" {}
		_BotTex("Bottom Tex", 2D) = "white" {}

		_Offset("Offset", Float) = 0
		_Scale("Scale", Float) = 1



	}
	
	SubShader
	{
		Tags{ "RenderType" = "Transparent" }
		LOD 100

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
					// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};


			float4 _BckBottomColor;
			float4 _BckMidColor;
			float4 _BckTopColor;
			float4 _BckBaseColor;

			sampler2D _BotTex;
			sampler2D _MidTex;
			sampler2D _TopTex;

			float _Dim;

			v2f vert(appdata v) {
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			float _Scale;
			float _Offset;

			float4 frag(v2f i) : SV_Target
			{
				// sample the texture
				float blendBot = tex2D(_BotTex, float2(i.uv.x, (i.uv.y + _Offset) * _Scale)).a;
				float blendMid = tex2D(_MidTex, float2(i.uv.x, (i.uv.y + _Offset) * _Scale)).a;
				float blendTop = tex2D(_TopTex, float2(i.uv.x, (i.uv.y + _Offset) * _Scale)).a;
				
				float4 color = blendBot * _BckBottomColor + blendMid * _BckMidColor + blendTop * _BckTopColor + _BckBaseColor;

				return color * _Dim;
			}
		ENDCG
		}
	}
}
