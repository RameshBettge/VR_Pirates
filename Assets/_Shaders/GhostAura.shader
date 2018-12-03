Shader "Custom/GhostAura"
{
	Properties
	{
		_NoiseTex ("Noise", 2D) = "white" {}
		_Color ("Color", color) = (1, 1, 1, 1)
		_Color2 ("Color2", color) = (1, 1, 1, 1)
		_Thickness ("Thickness", Range(0.01, 0.2)) = 0.5

		_NoiseScrollX ("NoiseScroll X", Range(0.01, 10)) = 1
		_NoiseScrollY ("NoiseScroll Y", Range(0.01, 10)) = 1

		_BlendScrollX ("BlendScroll X", Range(0.01, 10)) = 1
		_BlendScrollY ("BlendScroll Y", Range(0.01, 10)) = 1

		_ViewWeight("View Weight", Range(0, 1)) = 0.25
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent+1"}
		LOD 100

		Pass
		{
			Cull Front
			ZWrite Off
			blend SrcAlpha OneMinusSrcAlpha // TODO: experiment with kinds of blending

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 localPos : TEXCOORD1;
				float3 viewPos : TEXCOORD2;
			};

			sampler2D _NoiseTex;
			float4 _NoiseTex_ST;
			fixed4 _Color;
			fixed4 _Color2;
			float _Thickness;
			float _NoiseScrollX;
			float _NoiseScrollY;

			float _BlendScrollX;
			float _BlendScrollY;

			float _ViewWeight;
			
			v2f vert (appdata v)
			{
				v2f o;
				v.vertex += (v.normal * _Thickness);
				o.localPos = v.vertex;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.viewPos = UnityObjectToViewPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _NoiseTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 blendTimeOffset = float2(_BlendScrollX, _BlendScrollY) * _Time.x;

				float2 noiseTimeOffset = float2(_NoiseScrollX, _NoiseScrollY) * _Time.x;
				float2 localUV = noiseTimeOffset + float2(i.localPos.x + i.localPos.z, i.localPos.y);
				float2 viewUV = noiseTimeOffset + i.viewPos.xy;


				float localNoiseA = tex2D(_NoiseTex, localUV);
				float viewNoiseA = tex2D(_NoiseTex, viewUV);
				//fixed4 col = tex2D(_NoiseTex, i.uv);
				fixed4 col = _Color;

				//col.a *= (viewNoiseA * _ViewWeight) + (localNoiseA * (1 - _ViewWeight)); // permanent weight method

				float noiseBlending = tex2D(_NoiseTex, i.viewPos.xy + blendTimeOffset);
				col.a *= (viewNoiseA * noiseBlending) + (localNoiseA * (1 - noiseBlending)); 
				col.rgb = _Color * noiseBlending + _Color2*(1-noiseBlending);
				col.a *= 2;


				return col;
				//return col;
			}
			ENDCG
		}
	}
}
