Shader "Custom/GhostAura"
{
	Properties
	{

		_NoiseTex ("Noise", 2D) = "white" {}

		[Space(10)]
		_Color ("Color", color) = (1, 1, 1, 1)
		_Color2 ("Color2", color) = (1, 1, 1, 1)
		_MaxVisibility("Maximal visibility", Range(0.01, 1)) = 0.8

		// NOTE: this could be used to create a spawn effect. Slide this from 0 to 100 and then put in the actual texture of the enemy behind it. Then slide it back to 0.
		_PatchSize("Patch Size", range(0.1, 10)) = 2

		_Thickness ("Thickness", float) = 0.5

		[Space(20)]
		_NoiseScrollX ("NoiseScroll X", Range(-10, 10)) = 1
		_NoiseScrollY ("NoiseScroll Y", Range(-10, 10)) = 1
		_NoiseScrollX2 ("NoiseScroll X 2", Range(-10, 10)) = 1
		_NoiseScrollY2 ("NoiseScroll Y 2", Range(-10, 10)) = 1


		[Space(10)]

		_BlendScrollX ("BlendScroll X", Range(-10, 10)) = 1
		_BlendScrollY ("BlendScroll Y", Range(-10, 10)) = 1

		[Space(20)]

		_MaxSecondaryWeight("Max Secondary Weight", Range(0, 1)) = 0.25

		_NoiseSize("Noise Size Multiplier", Range(0.01, 1)) = 0.1
		_NoiseWorldUVMultiplier("NoiseWorldUVMultiplier", float) = 1

		_Smoothness("Smoothness", float) = 3
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
			float _MaxVisibility;
			float _PatchSize;
			float _Thickness;

			float _NoiseScrollX;
			float _NoiseScrollY;
			float _NoiseScrollX2;
			float _NoiseScrollY2;


			float _BlendScrollX;
			float _BlendScrollY;

			float _MaxSecondaryWeight;
			float _NoiseSize;
			float _Smoothness;
			float _NoiseWorldUVMultiplier;
			
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

				// Effect based on fragment position in view - commented out because worldCoords are used.
				//viewUV *= _NoiseSize;
				//float2 viewUV = noiseTimeOffset + i.viewPos.xy;
				//float viewNoiseA = tex2D(_NoiseTex, viewUV);
				//float noiseBlending = tex2D(_NoiseTex, i.viewPos.xy + blendTimeOffset);
				//col.a *= (viewNoiseA * noiseBlending) + (localNoiseA * (1 - noiseBlending)); 

				// Effect based on world position
				float2 worldUV = float2(i.worldPos.x + i.worldPos.z, i.worldPos.y);
				worldUV *= _NoiseSize * _NoiseWorldUVMultiplier;
				float worldNoiseA = tex2D(_NoiseTex, worldUV * _NoiseSize);
				float noiseBlending = tex2D(_NoiseTex, worldUV + blendTimeOffset);
				noiseBlending = min(noiseBlending, _MaxSecondaryWeight);

				col.a *= (worldNoiseA * noiseBlending) * (localNoiseA * (1 - noiseBlending) * 10); 


				col.rgb = _Color * noiseBlending + _Color2*(1-noiseBlending);
				col.a *= 2;


				// ADDING FADE-OUT TO THE SIDES (UNFUNCTIONAL)

				float3 viewDir = normalize(_WorldSpaceCameraPos.xyz -
				i.worldPos.xyz);
				float NdotV = max(0,dot(i.normal, viewDir));
				float alpha = ((0.5 * NdotV + 0.5));

				col.a *= pow(alpha, _Smoothness);

				col.a *= col.a;
				//col.rgb = float3(0, 0, 0);

				col.a = min(col.a, _MaxVisibility);

				return col;
				//return col;
			}
			ENDCG
		}
	}
}
