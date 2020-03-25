// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "JK2/Flow"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex("Noise texture", 2D) = "grey" {}
		_Mitigation("Distortion mitigation", Range(1, 30)) = 1
		_SpeedX("Speed X", Range(0, 10)) = 0
		_SpeedY("Speed Y", Range(0, 10)) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			
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
			float4 _MainTex_ST;

			sampler2D _NoiseTex;
			float _Mitigation;
			float _SpeedX;
			float _SpeedY;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
	
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				//half noiseVal = tex2D(_NoiseTex, uv).r;
				

				half noiseVal = 1;

				half2 uv = i.uv;
				uv.x = uv.x + noiseVal *  sin(_Time.y * _SpeedX) / _Mitigation;
				uv.y = uv.y + noiseVal *  sin(_Time.y * _SpeedY) / _Mitigation;

				return tex2D(_MainTex, uv);
			}
			ENDCG
		}
	}
}
