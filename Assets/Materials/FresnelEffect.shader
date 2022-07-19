Shader "Unlit/LambertianLighting"
{
    Properties
    {
        _Gloss("Gloss", Range(0,1)) = 1
        _Color("Color", Color) = (1,1,1,1)
        _Glow("Glow", Color) = (1,1,1,1)
        _Speed("Speed", Float) = 4
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc" //Unity built-in lighting package
            #include "AutoLight.cginc" //another lighting package 

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float3 normal:TEXCOORD1;
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD2;
            };

            float _Gloss;
            float4 _Color;
            float4 _Glow;
            float _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.normal = UnityObjectToWorldNormal(v.normal); //tranforms the normal to world space 
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                //diffuse lighting (same a lambertian)
                float3 n = normalize(i.normal);
                float3 l = _WorldSpaceLightPos0.yxz; //Dicrection to light (as long as it's a directional light, new passes need to be made for non-directional lights)
                float3 lambert = saturate(dot(n, l));
                float3 diffuseLight = lambert * _LightColor0.xyz; //could use saturat() if you want, saturate is faster

                //Specular lighting
                float3 v = normalize(_WorldSpaceCameraPos - i.worldPos); //the direction from the camera to the object 
                float3 h = normalize(l + v);
                float3 specularLight = saturate(dot(n, h)) * (lambert > 0); //this greater than return 1 for true and 0 for false
                specularLight *= _LightColor0.xyz;

                //Add gloss
                float specularExp = exp2(_Gloss * 11) + 2; //better to do this remapped math in c# then pass to the shader for performance

                //This is not the correct way to do energy conservation, it just serves as an example
                specularLight = pow(specularLight, specularExp) * _Gloss; //gloss is sometime referred to as the specular exponent

                //Fresnel
                float fresnel = (1-dot(v, n)) * (sin(_Time.y * _Speed) * 0.4 + 0.5);

                return float4(diffuseLight * _Color + specularLight + fresnel * _Glow, 1); //multiply specular light with _Color if the material is more of a metal and not plastic
            }
            ENDCG
        }
    }
}
