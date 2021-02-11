// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Chessboard"
{
    Properties
    {
        _MagnitudeX ("Magnitude X", Float) = 10 // chessboard size
        _MagnitudeZ ("Magnitude Z", Float) = 10
        _ColorA ("Color A", COLOR) = (1,1,1,1)
        _ColorB ("Color B", COLOR) = (0,0,0,1)
    }

    SubShader {
        Tags { "LightMode" = "ForwardBase" }

        Pass {
            
            CGPROGRAM
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_fwdbase

            #include "AutoLight.cginc"

            float _MagnitudeX; // chessboard size
            float _MagnitudeZ; // chessboard size
            float4 _ColorA, _ColorB;

            struct vertexInput {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
            };

            struct fragmentInput{
                float4 pos : SV_POSITION;
                float4 texcoord0 : TEXCOORD0;
                LIGHTING_COORDS(1,2)
            };

            fragmentInput vert(vertexInput i){
                fragmentInput o;
                o.pos = UnityObjectToClipPos (i.vertex);
                o.texcoord0 = i.texcoord0;
                TRANSFER_VERTEX_TO_FRAGMENT(o);
                return o;
            }

            fixed4 frag(fragmentInput i) : SV_Target {
                fixed4 color;
                if ( fmod(i.texcoord0.x*_MagnitudeX,2.0) < 1.0 ){
                    if ( fmod(i.texcoord0.y*_MagnitudeZ,2.0) < 1.0 )
                    {
                        color = _ColorA;
                    } else {
                        color = _ColorB;
                    }
                } else {
                    if ( fmod(i.texcoord0.y*_MagnitudeZ,2.0) > 1.0 )
                    {
                        color = _ColorA;
                    } else {
                        color = _ColorB;}
                    }

                float attenuation = LIGHT_ATTENUATION(i);

                return color * attenuation;
            }
            ENDCG
        }
    }
    Fallback "VertexLit"
}
