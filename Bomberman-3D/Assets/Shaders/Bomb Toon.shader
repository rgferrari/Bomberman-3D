﻿Shader "Unlit/Bomba Toon"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TintColor("Tint Color", Color) = (1,1,1,1)

        _Distance("Distance", Float) = 1
        _Amplitude("Amplitude", Float) = 1
        _Speed ("Speed", Float) = 1
        _Amount("Amount", Range(0.0,1.0)) = 1

        _Brightness("Brightness", Range(0,1)) = 0.3
    }

    SubShader
    {
        Tags 
		{ 
			"RenderType" = "Opaque"
			"LightMode" = "ForwardBase"
		}

        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
        //LOD 100

        //ZWrite on
        //Blend SrcAlpha OneMinusSrcAlpha

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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldNormal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _Brightness;

            float Toon(float3 normal, float3 lightDir)
            {
                float NdotL = max(0.0, dot(normalize(normal), normalize(lightDir)));

                return floor(NdotL/0.49);
            }


            float4 _TintColor;
            //float4 _Color;
            //float _Transparency;
            float _CutoutThresh;
            float _Distance;
            float _Amplitude;
            float _Speed;
            float _Amount;

            v2f vert (appdata v)
            {
                v2f o;

                v.vertex.x += (((sin(_Time.z * _Speed + _Amplitude) + 1) / 2.0) * _Distance * _Amount * v.normal);
                v.vertex.y += (((sin(_Time.z * _Speed + _Amplitude) + 1) / 2.0) * _Distance * _Amount * v.normal.y);
                v.vertex.z += (((sin(_Time.z * _Speed + _Amplitude) + 1) / 2.0) * _Distance * _Amount * v.normal.z);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                o.worldNormal = UnityObjectToWorldNormal(v.normal);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv) + _TintColor; // + _TintColor para as linhas


                fixed4 original = tex2D(_MainTex, i.uv);
				fixed lum = saturate(Luminance(_TintColor.rgb)) - 0.1;

				fixed4 output;
				output.rgb = lerp(_TintColor.rgb, fixed3(lum,lum,lum), -sin(_Time.z*2));
				output.a = original.a;
                if(Toon(i.worldNormal, _WorldSpaceLightPos0.xyz) < 0.5f){
                    return output * 0.5f;
                }
				
                return output * Toon(i.worldNormal, _WorldSpaceLightPos0.xyz);

                //_TintColor.r = sin(_Time.z * 2);

                //col *= Toon(i.worldNormal, _WorldSpaceLightPos0.xyz) + _Brightness;

                //fixed4 pixelColor = tex2D(_MainTex, i.uv);
                //col.a = _Transparency;
                //clip(col.r - _CutoutThresh);
                //return col * _TintColor;
            }
            ENDCG
        }
    } 
}