// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Shader_Water"
{
    Properties
    {
        _MainTex ("SpriteSheet", 2D) = "white" {}
		_IntX ("X axis size", Int) = 32
		_IntY ("Y axis size", Int) = 32
    }
    SubShader
    {
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appData
			{
				float vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			v2f vert(appData v) 
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			sampler2D _MainTex

			float4 frag(v2f i) : SV_TARGET
			{
				float4 color = tex2D(_MainTex, i.uv)
				return color;
			}
			ENDCG
		}
    }
}
