Shader "Unlit/D"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Mask("Mask",2d)="white"{}
        _Width("Width",Range(0,0.3))=0
        _EdgeWidth("Edge",Range(0,0.3))=0
        _Light("Light",Range(0,1))=0
        _X("_X",Range(0,1))=0
        _Y("_Y",Range(0,1))=0
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque" "Queue"="Transparent"
        }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _Mask;
            float4 _MainTex_ST;
            float _Width;
            float _EdgeWidth;
            float _Light;
            float _X;
            float _Y;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed2 center = fixed2(_X, _Y);
                // fixed2 scaleFactor = fixed2(1399.0 / 1513, 1);//图片的缩放比例
                fixed2 scaleFactor = fixed2(1, 1); //图片的缩放比例
                fixed2 uv = i.uv;
                uv -= center;
                fixed factor = 1 - smoothstep(_Width, _Width + _EdgeWidth, length(uv.xy * scaleFactor));
                uv -= uv * _Light * factor;
                uv += center;
                fixed4 col = tex2D(_MainTex, uv);
                fixed4 colMask = tex2D(_Mask, i.uv);
                return fixed4(col.r, col.g, col.b, colMask.a);
            }
            ENDCG
        }
    }
}