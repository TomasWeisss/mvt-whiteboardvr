Shader "Unlit/NonClearingRenderTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PreviousTex ("Prev Texture", 2D) = "black" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _PreviousTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 newCol = tex2D(_MainTex, i.uv);
                fixed4 prevCol = tex2D(_PreviousTex, i.uv);
                return lerp(prevCol, newCol, newCol.a);
            }
            ENDCG
        }
    }
}
