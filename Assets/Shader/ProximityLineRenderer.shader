Shader "Custom/ProximityLineRenderer"
{
    Properties {
        _MainTex("Texture", 2D) = "white" {}
        _StartColor("Start Color", Color) = (1, 0, 0, 1)
        _EndColor("End Color", Color) = (0, 0, 1, 1)
        _BlendFactor("Blend Factor", Range(0, 1)) = 0.5
        _WorldPosA("World Position A", Vector) = (0, 0, 0, 1)
        _WorldPosB("World Position B", Vector) = (0, 0, 0, 1)
        _MainTex_ST("Texture Tiling and Offset", Vector) = (1, 1, 0, 0)
    }

    SubShader {
        Tags { "Queue"="Transparent" }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _StartColor;
            float4 _EndColor;
            float _BlendFactor;
            float4 _WorldPosA;
            float4 _WorldPosB;
            float4 _MainTex_ST;

            v2f vert(appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw; // Apply tiling and offset to UV
                float distanceToA = distance(v.vertex, _WorldPosA.xyz);
                float distanceToB = distance(v.vertex, _WorldPosB.xyz);
                float t = saturate(distanceToA / (distanceToA + distanceToB));
                o.color = lerp(_StartColor, _EndColor, t);
                return o;
            }

            half4 frag(v2f i) : SV_Target {
                half4 texColor = tex2D(_MainTex, i.uv);
                half4 resultColor = texColor;
                resultColor.rgb *= i.color.rgb;
                resultColor.a *= i.color.a;
                return resultColor;
            }
            ENDCG
        }
    }
}