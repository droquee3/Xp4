Shader "Custom/VignetteShader"
{
    Properties
    {
        _Transparency ("Transparency", Range(0, 1)) = 1.0 // Controle da transparência
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha // Define a transparência
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
                float4 pos : SV_POSITION;
            };

            float _Transparency;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // Cor escura com transparência controlada
                return half4(0, 0, 0, 1 - _Transparency); // Preto com transparência inversa
            }
            ENDCG
        }
    }
}
