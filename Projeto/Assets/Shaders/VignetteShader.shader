Shader "Custom/VignetteWithBlurredCircle"
{
    Properties
    {
        _CircleRadius ("Circle Radius", Range(0, 1)) = 1.0     // Tamanho do círculo transparente
        _BlurWidth ("Blur Width", Range(0, 0.5)) = 0.1         // Largura da transição suave
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha                      // Define a transparência
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

            float _CircleRadius;
            float _BlurWidth;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // Calcula a distância do ponto atual ao centro da tela
                float2 center = float2(0.5, 0.5);
                float dist = distance(i.uv, center);

                // Define a transição suave entre o círculo transparente e as bordas pretas
                float edgeStart = _CircleRadius;
                float edgeEnd = _CircleRadius + _BlurWidth;
                float alpha = smoothstep(edgeStart, edgeEnd, dist);

                return half4(0, 0, 0, alpha); // Preto com transição suave
            }
            ENDCG
        }
    }
}
