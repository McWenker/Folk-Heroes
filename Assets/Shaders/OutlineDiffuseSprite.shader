Shader "Sprites/OutlineDiffuse"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
        [PerRendererData] _Outline("Outline", Float) = 0        
        [PerRendererData] _OutlineColor("Outline Color", Color) = (1, 1, 1, 1)
    }
 
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
 
        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha
 
        CGPROGRAM
        #pragma surface surf Lambert vertex:vert nofog nolightmap nodynlightmap keepalpha noinstancing
        #pragma multi_compile_local _ PIXELSNAP_ON
        #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
        #include "UnitySprites.cginc"
 
        struct Input
        {
            float2 uv_MainTex;
            fixed4 color;
            float2 texcoord : TEXCOORD0;
        };
 
        void vert (inout appdata_full v, out Input o)
        {
            v.vertex = UnityFlipSprite(v.vertex, _Flip);
 
            #if defined(PIXELSNAP_ON)
            v.vertex = UnityPixelSnap (v.vertex);
            #endif
 
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.color = v.color * _Color * _RendererColor;
            o.texcoord = v.texcoord;
        }

        float4 _MainTex_TexelSize;
        float _Outline;
        float4 _OutlineColor;
        fixed4 _NullColor;

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
            c.rgb *= c.a;

            fixed4 outlineC = _OutlineColor;
            if(_Outline == 0)
            {
                outlineC = _NullColor;
            }

            outlineC.rgb *= outlineC.a;

            fixed alpha_up = SampleSpriteTexture(IN.texcoord + fixed2(0, _MainTex_TexelSize.y)).a;
            fixed alpha_down = SampleSpriteTexture(IN.texcoord- fixed2(0, _MainTex_TexelSize.y)).a;
            fixed alpha_right = SampleSpriteTexture(IN.texcoord + fixed2(_MainTex_TexelSize.x, 0)).a;
            fixed alpha_left = SampleSpriteTexture(IN.texcoord - fixed2(_MainTex_TexelSize.x, 0)).a;

            fixed4 output = lerp(c, outlineC, ceil( clamp(alpha_down + alpha_up + alpha_left + alpha_right, 0, 1) ) - ceil(c.a));

            o.Albedo = output.rgb;
            o.Alpha = output.a;
        }
        ENDCG
    }
 
Fallback "Diffuse"
}