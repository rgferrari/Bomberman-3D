Shader "Unlit/Explosao"
{
    Properties
    {

        _MainTex ("Base (RGB)", 2D) = "white" {}
		_Intensity ("Intensity", float) = 1.0
		_BrightnessFactor ("Brightness Factor", float) = 1.0

        _TintColor("Tint Color", Color) = (1,1,1,1)
        _Transparency("Transparency", Range(0.0,1)) = 0.25
        //_CutoutThresh("Cutout Threshold", Range(0.0,1.0)) = 0.2
        _Distance("Distance", Float) = 1
        _Amplitude("Amplitude", Float) = 1
        _Speed ("Speed", Float) = 1
        _Amount("Amount", Range(0.0,1.0)) = 1
    }

    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        ZWrite on
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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                //fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _Intensity;
			float _BrightnessFactor;
            
            float4 _TintColor;
            //float4 _Color;
            float _Transparency;
            float _CutoutThresh;
            float _Distance;
            float _Amplitude;
            float _Speed;
            float _Amount;

            v2f vert (appdata v)
            {
                v2f o;

                v.vertex.x += sin(_Time.z * _Speed + v.vertex.x * _Amplitude) * _Distance * _Amount * v.normal.x;
                v.vertex.y += sin(_Time.z * _Speed + v.vertex.y * _Amplitude) * _Distance * _Amount * v.normal.y;
                v.vertex.z += sin(_Time.z * _Speed + v.vertex.z * _Amplitude) * _Distance * _Amount * v.normal.z;
                //v.vertex.y += (sin(_Time.z * _Speed + v.vertex.y * _Amplitude) * _Distance * _Amount);
                //v.vertex.z += (sin(_Time.z * _Speed + v.vertex.y * _Amplitude) * _Distance * _Amount);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            /*float4 Grayscale(float4 inputColor)
            {
                float gray = dot(inputColor.rgb, float3(0.2126, 0.7152, 0.0722 ))
                return float4(gray, gray, gray);
            }*/

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv); // + _TintColor para as linhas
                //fixed4 gray = float4(0.3, 0.59, 0.11, 1);
                //fixed4 gray = float4(100/255, 100/255, 100/255, 1);
                //fixed4 red = float4(1, 0, 0, 1);

                //fixed4 l = lerp(red, gray, sin(_Time.z* 2));*/

                //gray.x = 60/255;
                //gray.y = 60/255;
                //gray.z = 60/255;
                //gray.a = 60/255;


                //fixed4 original = tex2D(_MainTex, i.uv);
				//fixed lum = saturate(Luminance(_TintColor.rgb));

				//fixed4 output;
				//output.rgb = lerp(_TintColor.rgb, fixed3(lum,lum,lum), -sin(_Time.z*2));
				//output.a = original.a;
				

                //_Intensity
                //_TintColor.r = sin(_Time.z * 2); sin(_Time.y*2)*0.5+0.5

                /*fixed lo = 0.0;
                fixed hi = 1.0;

                // rescale the range 0.2 .. 0.5 to 0.0 .. 1.0
                fixed desaturation = saturate((col.g - lo) / (hi - lo));

                // change 0.0 .. 0.5 .. 1.0 to 1.0 .. 0.0 .. 1.0
                desaturation = abs((desaturation * 2.0) - 1.0);

                // lerp between grey and colorized grey
                fixed3 main = lerp(col * _TintColor.rgb, fixed3(col.g, col.g, col.g), desaturation);*/
                //o.Albedo = col;

                //o.Specular = 0.001; //lowest possible
                //o.Smoothness = 0;

                //fixed4 pixelColor = tex2D(_MainTex, i.uv);
                col.a = _Transparency;
                //_TintColor.a = _Transparency;
                //clip(col.r - _CutoutThresh);


                return col * _TintColor;
                //return l;
            }
            ENDCG
        }
    }
}
