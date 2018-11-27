Shader "Custom/Pointer"
{
	// TODO: Check, if fog is needed or unnecessary

	Properties
	{
		_MainTex ("Texture", 2D) = "white" {} //Currently unused but might be used to add a nice effect
		//_Color ("Color", color) = (0, 0, 0 ,0)
		_ScrollSpeed ("Scroll Speed", Float) = 10
		_FadeOutAbruptness ("Fade-Out Abruptness", int) = 2
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent"}
		LOD 100

		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite off
		Cull off

		Pass
		{

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
				float3 localPos : TEXCOORD2;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			//fixed4 _Color;
			float _ScrollSpeed;
			int _FadeOutAbruptness;

			uniform float uMaxPos;
			uniform float uColor [4];

			fixed4 ArrayToCol(float array[4]) 
			{ 
				return fixed4(array[0], array[1], array[2], array[3]); 
			}
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv.y -= _Time.x * _ScrollSpeed;

				o.localPos = v.vertex;

				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 texCol = tex2D(_MainTex, i.uv);
				fixed4 col = ArrayToCol(uColor);
				//fixed4 col = fixed4(1, 1, 1, 1);

				 //apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);

				col.a *= texCol.a;

				float alpha = 1 - pow((i.localPos.y / uMaxPos), _FadeOutAbruptness);


				col.a = min(alpha, col.a);

				return col;
			}
			ENDCG
		}
	}
}
