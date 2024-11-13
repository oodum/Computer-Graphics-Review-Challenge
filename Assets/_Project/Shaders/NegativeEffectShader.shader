Shader "Sid/NegativeEffectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Tint ("Tint", Color) = (1, 0, 1, 1)
        _TintStrength ("Strength", Range(0, 1)) = 0.5
    }
    SubShader
    {
        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        float3 _PrevColor;
        float3 _CurrColor;
        float3 _Tint;
        float _TintStrength;
        
        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            _PrevColor = tex2D(_MainTex, IN.uv_MainTex);
            _CurrColor.r = abs(1 - _PrevColor.r);
            _CurrColor.g = abs(1 - _PrevColor.g);
            _CurrColor.b = abs(1 - _PrevColor.b);
            o.Albedo = (_CurrColor + (_Tint*_TintStrength)).rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
