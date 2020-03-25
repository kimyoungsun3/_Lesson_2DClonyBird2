Shader "JK2/SportLight"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_CharacterPosition("Char pos", vector) = (0,0,0,0)
		_CircleRadius("Spotlight size", Range(0, 20)) = 3
		_RingSize("Ring size", Range(0, 5)) = 1
		_ColorTint("Outside of the sportlight color", Color) = (0,0,0,0)
		_MoveRate("Move Rate", Range(0, 5)) = 0		//0f error
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

				float dist : TEXCOORD1;

				//float3 worldPos : TEXCOORD1; // World position of that vertex 
				//float3 mormalMinusWorld : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			//
			float4 _CharacterPosition;
			float _CircleRadius;
			float4 _ColorTint;
			float _RingSize;
			float _MoveRate;

			//
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.dist = distance(worldPos, _CharacterPosition.xyz);

				// 
				if (o.dist > _MoveRate) {
					o.vertex.y += _MoveRate;
				}
				else {
					o.vertex.y += o.dist;
				}
				//o.vertex.y += (o.dist); //+ _MoveRate);

				return o;
			}
			
			//
			fixed4 frag (v2f i) : SV_Target
			{
				//fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 col = _ColorTint; // (0,0,0,0) 
			
				
				// This is the player's spotlight
				if (i.dist < _CircleRadius) {
					col = tex2D(_MainTex, i.uv);
				}
				else if( (i.dist > _CircleRadius) && (i.dist < (_CircleRadius + _RingSize)) ){
					//col = 0.5;
					float blendStrength = i.dist - _CircleRadius;
					col = lerp(tex2D(_MainTex, i.uv), _ColorTint, blendStrength / _RingSize);
				}
				else {
					//col = 0;
				}
				// This is the blending section

				// This is past both the Player's spotlight and the blending section

				//

				return col;
			}
			ENDCG
		}
	}
}
