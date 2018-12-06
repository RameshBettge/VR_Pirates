Shader "Custom/GhostAura"
{
	Properties
	{
		[NoScaleOffset]
		_NoiseTex ("Noise", 2D) = "white" {}

		[Space(10)]
		_Color ("Color", color) = (1, 1, 1, 1)
		_Color2 ("Color2", color) = (1, 1, 1, 1)
		[Space(10)]

		// NOTE: this could be used to create a spawn effect. Slide this from 0 to 100 and then put in the actual texture of the enemy behind it. Then slide it back to 0.
		_Thickness ("Thickness", float) = 0.5
		_SideFadeStrength("SideFadeStrength", float) = 3
		[Space(10)]
		_PatchSize("Patch Size", range(0.1, 10)) = 2
		_MaxOpacity("Maximal Opacity", Range(0.01, 1)) = 0.8
		[Space(10)]
		_BlendStrength("Blend Strength", Range(0, 1)) = 0.2
		_BlendSpeed("Blend Speed", Range(0.1, 3)) = 2

		[Space(20)]

		_NoiseSize("Noise Size Multiplier", Range(0.01, 1)) = 0.1
		_blenUVMultiplier("BlendUV Multiplier", float) = 1

		[Space(20)]

		[Header(UV Scroll speed)]

		_NoiseScrollX ("NoiseScroll X", Range(-10, 10)) = 1
		_NoiseScrollY ("NoiseScroll Y", Range(-10, 10)) = 1
		_NoiseScrollX2 ("NoiseScroll X 2", Range(-10, 10)) = 1
		_NoiseScrollY2 ("NoiseScroll Y 2", Range(-10, 10)) = 1


		[Space(10)]

		_BlendScrollX ("BlendScroll X", Range(-10, 10)) = 1
		_BlendScrollY ("BlendScroll Y", Range(-10, 10)) = 1


	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent+1"}
		LOD 100

		Pass
		{
			Cull Back
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

				float3 normal : TEXCOORD3;
				float3 worldPos : TEXCOORD4;
			};

			sampler2D _NoiseTex;
			float4 _NoiseTex_ST;
			fixed4 _Color;
			fixed4 _Color2;
			float _MaxOpacity;
			float _PatchSize;
			float _Thickness;

			float _NoiseScrollX;
			float _NoiseScrollY;
			float _NoiseScrollX2;
			float _NoiseScrollY2;


			float _BlendScrollX;
			float _BlendScrollY;

			float _NoiseSize;
			float _SideFadeStrength;
			float _blenUVMultiplier;

			float _BlendStrength;
			float _BlendSpeed;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
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
				float2 noiseTimeOffset2 = float2(_NoiseScrollX2, _NoiseScrollY2) * _Time.x;
				float2 localUV = noiseTimeOffset + float2(i.localPos.x, i.localPos.y);
				float2 localUV2 = noiseTimeOffset2 + float2(i.localPos.x, i.localPos.y);

				localUV *= _NoiseSize;

				float localNoiseA = (tex2D(_NoiseTex, localUV) + tex2D(_NoiseTex, localUV2)) * _PatchSize;
				



				fixed4 col = _Color;

				// Effect based on world position
				float2 blendUV1 = float2(i.localPos.x + i.localPos.y, i.localPos.z);
				float2 blendUV2 = float2(i.localPos.x * 0.5 + i.localPos.z + i.localPos.y, i.localPos.x * 0.5 + i.localPos.z * 0.5);
				float2 blendUV = lerp(blendUV1, blendUV2, sin(_Time.w * _BlendSpeed) * _BlendStrength);
				//float2 blendUV = blendUV1;

				blendUV *= _NoiseSize * _blenUVMultiplier;
				float worldNoiseA = tex2D(_NoiseTex, blendUV * _NoiseSize);
				float noiseBlending = tex2D(_NoiseTex, blendUV + blendTimeOffset);

				col.a *= (worldNoiseA * noiseBlending) * (localNoiseA * (1 - noiseBlending) * 10); 


				col.rgb = _Color * noiseBlending + _Color2*(1-noiseBlending);
				col.a *= 2;


				// Adding fade-out to the sides

				float3 viewDir = normalize(_WorldSpaceCameraPos.xyz -
				i.worldPos.xyz);
				float NdotV = max(0,dot(i.normal, viewDir));
				float alpha = ((0.5 * NdotV + 0.5));

				col.a *= pow(alpha, _SideFadeStrength);

				col.a *= col.a;
				//col.rgb = float3(0, 0, 0);

				col.a = min(col.a, _MaxOpacity);

				return col;
				//return col;
			}
			ENDCG
		}
	}
}