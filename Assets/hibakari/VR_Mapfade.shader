Shader "Unlit/VR_Mapfade"
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
		Tags { "RenderType"="transparent" "DisableBatching"="True"}
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma geometry geom
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2g
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			struct g2f
			{
				float4 vertex : SV_POSITION;
				float2 uv :TEXCOORD0;
				UNITY_FOG_COORDS(1)

			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _Moji;
			float4 _Moji_ST;
			float _fademin;
			float _fademax;
			
			v2g vert (appdata v)
			{
				v2g o;
				o.vertex = mul(UNITY_MATRIX_M, (v.vertex));
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			float3 rand(float3 seed)
			{
				return 2*float3(0.5-frac(sin(dot(seed.xy, float2(12.9898, 78.233)))*43758.5453), -2*frac(sin(dot(seed.yz, float2(12.9898, 78.233)))*43758.5453), 0.5-frac(sin(dot(seed.zx, float2(12.9898, 78.233)))*43758.5453));
			}


			[maxvertexcount(12)]
			void geom(triangle v2g input[3], inout TriangleStream<g2f> outstream)
			{
				float camera_distance = distance(_WorldSpaceCameraPos , mul(UNITY_MATRIX_M, float4(0,0,0,1)));
				float hoge = clamp(((camera_distance - _fademin) / _fademax),0,1);
				g2f output0;
				g2f output1;
				g2f output2;

				output0.vertex = mul(UNITY_MATRIX_VP,float4((input[0].vertex.xyz + rand(input[0].vertex.xyz+input[1].vertex.xyz)*hoge),1));
				output0.uv =input[0].uv;
				UNITY_TRANSFER_FOG(output0, output0.vertex);
				
				output1.vertex = mul(UNITY_MATRIX_VP, float4(((input[0].vertex.xyz+input[1].vertex.xyz)/2 + rand(input[0].vertex.xyz + input[1].vertex.xyz) * hoge), 1));
				output1.uv = (input[0].uv + input[1].uv)/2;
				UNITY_TRANSFER_FOG(output1,output1.vertex);
				
				output2.vertex = mul(UNITY_MATRIX_VP, float4(((input[0].vertex.xyz + input[2].vertex.xyz) / 2 + rand(input[0].vertex.xyz + input[1].vertex.xyz) * hoge), 1));
				output2.uv = (input[0].uv + input[2].uv)/2;
				UNITY_TRANSFER_FOG(output2, output2.vertex);
						
				outstream.Append(output0);
				outstream.Append(output1);
				outstream.Append(output2);
				outstream.RestartStrip();

				output0.vertex = mul(UNITY_MATRIX_VP, float4((input[1].vertex.xyz + rand(input[1].vertex.xyz+ input[2].vertex.xyz)*hoge), 1));
				output0.uv = input[1].uv;
				UNITY_TRANSFER_FOG(output0, output0.vertex);

				output1.vertex = mul(UNITY_MATRIX_VP, float4(((input[0].vertex.xyz + input[1].vertex.xyz) / 2 + rand(input[1].vertex.xyz + input[2].vertex.xyz) * hoge), 1));
				output1.uv = (input[0].uv + input[1].uv)/2;
				UNITY_TRANSFER_FOG(output1, output1.vertex);

				output2.vertex = mul(UNITY_MATRIX_VP, float4(((input[1].vertex.xyz + input[2].vertex.xyz) / 2 + rand(input[1].vertex.xyz + input[2].vertex.xyz) * hoge), 1));
				output2.uv = (input[1].uv + input[2].uv)/2;
				UNITY_TRANSFER_FOG(output2, output2.vertex);

				outstream.Append(output1);
				outstream.Append(output0);
				outstream.Append(output2);
				outstream.RestartStrip();

				output0.vertex = mul(UNITY_MATRIX_VP, float4((input[2].vertex.xyz + rand(input[2].vertex.xyz + input[0].vertex.xyz)*hoge), 1));
				output0.uv = input[2].uv;
				UNITY_TRANSFER_FOG(output0, output0.vertex);

				output1.vertex = mul(UNITY_MATRIX_VP, float4(((input[2].vertex.xyz + input[1].vertex.xyz) / 2 + rand(input[2].vertex.xyz + input[0].vertex.xyz) * hoge), 1));
				output1.uv = (input[1].uv + input[2].uv)/2;
				UNITY_TRANSFER_FOG(output1, output1.vertex);

				output2.vertex = mul(UNITY_MATRIX_VP, float4(((input[0].vertex.xyz + input[2].vertex.xyz) / 2 + rand(input[2].vertex.xyz + input[0].vertex.xyz) * hoge), 1));
				output2.uv = (input[0].uv + input[2].uv)/2;
				UNITY_TRANSFER_FOG(output2, output2.vertex);

				outstream.Append(output1);
				outstream.Append(output0);
				outstream.Append(output2);
				outstream.RestartStrip();

				output0.vertex = mul(UNITY_MATRIX_VP, float4(((input[0].vertex.xyz + input[1].vertex.xyz) / 2 + rand(input[0].vertex.xyz + input[1].vertex.xyz + input[2].vertex.xyz)*hoge), 1));
				output0.uv = (input[0].uv + input[1].uv)/2;
				UNITY_TRANSFER_FOG(output0, output0.vertex);

				output1.vertex = mul(UNITY_MATRIX_VP, float4(((input[2].vertex.xyz + input[1].vertex.xyz) / 2 + rand(input[0].vertex.xyz + input[1].vertex.xyz + input[2].vertex.xyz) * hoge), 1));
				output1.uv = (input[2].uv + input[1].uv)/2;
				UNITY_TRANSFER_FOG(output1, output1.vertex);

				output2.vertex = mul(UNITY_MATRIX_VP, float4(((input[0].vertex.xyz + input[2].vertex.xyz) / 2 + rand(input[0].vertex.xyz + input[1].vertex.xyz + input[2].vertex.xyz) * hoge), 1));
				output2.uv = (input[0].uv + input[2].uv)/2;
				UNITY_TRANSFER_FOG(output2, output2.vertex);

				outstream.Append(output0);
				outstream.Append(output1);
				outstream.Append(output2);
				outstream.RestartStrip();

			}
			


			fixed4 frag (g2f i) : SV_Target
			{
				// sample the texture
				float camera_distance = distance(_WorldSpaceCameraPos , mul(UNITY_MATRIX_M, float4(0,0,0,1)));
				float hoge = clamp(((camera_distance - _fademin) / _fademax),0,1);
				fixed4 col = tex2D(_MainTex, mul(float2x2(-1 / sqrt(2),-1 / sqrt(2),1 / sqrt(2),-1 / sqrt(2)),2 * (i.uv + float2(-_Time.x,_Time.x))));
				fixed4 col2 = tex2D(_Moji, i.uv);
				col = (1 - col2.a)*col + col2.a*col2;
				col.a *= 1 - hoge;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
fallback"Unlit/Texture"
}
