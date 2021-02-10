Shader "Unlit/reflexo"
{
    Properties {
        _MainTex ("Albedo Texture", 2D) = "white" {}
        _Color ("Diffuse Material Color", Color) = (1,1,1,1) 
        _SpecColor ("Specular Material Color", Color) = (1,1,1,1) 
        _Shininess ("Shininess", Float) = 10
        _Distance("Distance", Float) = 1
        _Amplitude("Amplitude", Float) = 1
        _Speed ("Speed", Float) = 1
        _Amount("Amount", Range(0.0,1.0)) = 1
   }
   SubShader {
        Pass {	
            Tags { "LightMode" = "ForwardBase" } 
            // pass for ambient light and first light source
    
            CGPROGRAM
    
            #pragma vertex vert  
            #pragma fragment frag 
    
            #include "UnityCG.cginc"
            uniform float4 _LightColor0; 
            // color of light source (from "Lighting.cginc")
    
            // User-specified properties
            uniform float4 _Color; 
            uniform float4 _SpecColor; 
            uniform float _Shininess;

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _CutoutThresh;
            float _Distance;
            float _Amplitude;
            float _Speed;
            float _Amount;
    
            struct vertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 uv : TEXCOORD0;
            };
            struct vertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
            };
    
            vertexOutput vert(vertexInput input) 
            {
                vertexOutput output;
    
                float4x4 modelMatrix = unity_ObjectToWorld;
                float4x4 modelMatrixInverse = unity_WorldToObject;

                input.vertex.x += (sin(_Time.z * _Speed + _Amplitude) * _Distance * _Amount * input.normal);
                input.vertex.y += (sin(_Time.z * _Speed + _Amplitude) * _Distance * _Amount * input.normal.y);
                input.vertex.z += (sin(_Time.z * _Speed + _Amplitude) * _Distance * _Amount * input.normal.z);
                
                output.pos = UnityObjectToClipPos(input.vertex);
                output.posWorld = mul(modelMatrix, input.vertex);
                output.normalDir = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
                
                return output;
            }
    
            float4 frag(vertexOutput input) : COLOR
            {
                float3 normalDirection = normalize(input.normalDir);
    
                float3 viewDirection = normalize(
                _WorldSpaceCameraPos - input.posWorld.xyz);
                float3 lightDirection;
                float attenuation;
    
                if (0.0 == _WorldSpaceLightPos0.w) // directional light?
                {
                attenuation = 1.0; // no attenuation
                lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                } 
                else // point or spot light
                {
                float3 vertexToLightSource = 
                    _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
                float distance = length(vertexToLightSource);
                attenuation = 1.0 / distance; // linear attenuation 
                lightDirection = normalize(vertexToLightSource);
                }
                _Color.r = sin(_Time.z * 2);
    
                float3 ambientLighting = 
                UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;
    
                float3 diffuseReflection = 
                attenuation * _LightColor0.rgb * _Color.rgb
                * max(0.0, dot(normalDirection, lightDirection));
    
                float3 specularReflection;
                if (dot(normalDirection, lightDirection) < 0.0) 
                // light source on the wrong side?
                {
                specularReflection = float3(0.0, 0.0, 0.0); 
                    // no specular reflection
                }
                else // light source on the right side
                {
                specularReflection = attenuation * _LightColor0.rgb 
                    * _SpecColor.rgb * pow(max(0.0, dot(
                    reflect(-lightDirection, normalDirection), 
                    viewDirection)), _Shininess);
                }

                return float4(ambientLighting + diffuseReflection 
                + specularReflection, 1.0);
            }
    
            ENDCG
      }
   }
   Fallback "Specular"
}
