Shader "SimpleTexturePacker/PackShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PackTex ("Pack Texture", 2D) = "white" {}
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _PackTex;
            float _Scale;
            float2 _Offset;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 puv = i.uv - _Offset;
                puv *= _Scale;

                fixed4 col = tex2D(_MainTex, i.uv);

                if (puv.x < 0.0 || puv.x > 1.0)
                {
                    return float4(1, 0, 0, 1);
                    return col;
                }

                if (puv.y < 0.0 || puv.y > 1.0)
                {
                    return col;
                }

                return tex2D(_PackTex, puv);
            }
            ENDCG
        }
    }
}
