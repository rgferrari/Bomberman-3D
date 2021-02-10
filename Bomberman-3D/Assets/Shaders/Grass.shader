Shader "Custom/Grass"
{
    Properties
    {
        _ColorA ("Color A", COLOR) = (1,1,1,1)
        _ColorB ("Color B", COLOR) = (0,0,0,1)
        _Mask ("Mask", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            float4 _ColorA;
            float4 _ColorB;
            sampler2D _Mask;
            float4 _Mask_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _Mask);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            { 
                float mask = tex2D(_Mask, i.uv).r; 
                fixed4 finalColor = _ColorB;
                
                if(mask < 0.5) 
                    finalColor = _ColorA;
                return finalColor;
            }
            ENDCG
        }
    }
}
