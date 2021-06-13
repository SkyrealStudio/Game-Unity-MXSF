Shader "Custom/Shader001"
{
	Properties
	{
		_MainTex("MainTex",2D) = ""{}
		_Color("DelColor",Color) = (1,1,1,1)
		_Color2("DelColor2",Color) = (1,1,1,1)
	}

	SubShader
	{
		pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "unitycg.cginc"
			
			sampler2D _MainTex;
			float4 _Color;
			float4 _Color2;

			struct v2f
			{
				float4 pos:POSITION;
				float2 uv:TEXCOORD0;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;
				return o;
			}

			fixed4 frag(v2f IN) : COLOR
			{
				fixed4 final = tex2D(_MainTex,IN.uv);
				
				fixed finalR = final.r;
				fixed finalG = final.g;
				fixed finalB = final.b;
				fixed finalA = final.a;

				/*

				if (
					finalR == 0 &&
					finalG > 0.5 &&
					finalB == 0 &&
					finalA == 1
					)
				{
					if(finalG < 0.8)
					{ discard; }
				}*/

				if (
					_Color.r == final.r&&
					_Color.g == final.g&&
					_Color.b == final.b&&
					_Color.a == final.a
					) 
				{
					discard;
				}

				if (
					_Color2.r == final.r&&
					_Color2.g == final.g&&
					_Color2.b == final.b&&
					_Color2.a == final.a
					)
				{
					discard;
				}

				return final;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}