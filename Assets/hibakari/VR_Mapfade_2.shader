Shader "Unlit/VR_Mapfade_2"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Moji("Moji", 2D) = "white" {}
		_fademin("minimum_distance",Range(0,10))=10
		_fademax("maximum_distance",Range(0,1000)) = 0
	}
	SubShader
	{
		Tags { "RenderType"="transparent" "RenderQueue"="transparent"  "DisableBatching"="True"}
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

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
				float3 orig_vertex : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};


			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _Moji;
			float4 _Moji_ST;
			float _fademin;
			float _fademax;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.orig_vertex = v.vertex.xyz;
				return o;
			}


			

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				
				float camera_distance = distance(_WorldSpaceCameraPos , mul(UNITY_MATRIX_M, float4(0,0,0,1)));
				float hoge = clamp(((camera_distance - _fademin) / _fademax),0,1);
				clip(-i.orig_vertex.y+0.5-hoge);
				fixed4 col = tex2D(_MainTex, mul(float2x2(-1 / sqrt(2),-1 / sqrt(2),1 / sqrt(2),-1 / sqrt(2)),2 * (i.uv + float2(-_Time.x,_Time.x))));
				fixed4 col2 = tex2D(_Moji, i.uv);
				col = (1 - col2.a)*col + col2.a*col2;
				col.a *= clamp((-i.orig_vertex.y+0.4-hoge)*6,0,1);
				
				
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
