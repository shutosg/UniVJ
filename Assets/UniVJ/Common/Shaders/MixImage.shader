Shader "UniVJ/MixImage"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Tex1 ("Texture", 2D) = "white" {}
        _Tex2 ("Texture", 2D) = "white" {}
        _Tex3 ("Texture", 2D) = "white" {}
        _Tex4 ("Texture", 2D) = "white" {}
        _BlendingFactor1 ("Blending Factor", Range(0.0, 1.0)) = 1.0
        _BlendingFactor2 ("Blending Factor", Range(0.0, 1.0)) = 0.0
        _BlendingFactor3 ("Blending Factor", Range(0.0, 1.0)) = 0.0
        _BlendingFactor4 ("Blending Factor", Range(0.0, 1.0)) = 0.0
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _Tex1;
            float4 _Tex1_ST;
            sampler2D _Tex2;
            sampler2D _Tex3;
            sampler2D _Tex4;
            float _BlendingFactor1;
            float _BlendingFactor2;
            float _BlendingFactor3;
            float _BlendingFactor4;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _Tex1);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col1 = tex2D(_Tex1, i.uv);
                fixed4 col2 = tex2D(_Tex2, i.uv);
                fixed4 col3 = tex2D(_Tex3, i.uv);
                fixed4 col4 = tex2D(_Tex4, i.uv);
                return col1 * _BlendingFactor1 + col2 * _BlendingFactor2 + col3 * _BlendingFactor3 + col4 * _BlendingFactor4;
            }
            ENDCG
        }
    }
}
