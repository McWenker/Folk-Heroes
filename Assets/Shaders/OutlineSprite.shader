Shader "Sprites/OutlinedSprite"
{
    Properties
    {
        [PerRendererData] _MainTex("Texture", 2D) = "white" {}
        _NullColor("NullColor", Color) = (0, 0, 0, 0)
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0

        [PerRendererData] _Color("Tint", Color) = (0, 0, 0, 1)

        [PerRendererData] _Outline("Outline", Float) = 0        
        [PerRendererData] _OutlineColor("Outline Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Cull Off
        Blend One OneMinusSrcAlpha
        ZWrite Off
        

        Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

        Pass
        {
            CGPROGRAM

            #pragma vertex vertexFunc
            #pragma fragment fragmentFunc            
			#pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 pos      : SV_POSITION;
                fixed4 color    : COLOR;
                half2 texcoord  : TEXCOORD0;
            };  

            sampler2D _MainTex;
            fixed4 _NullColor;
            fixed4 _Color;
            float _Outline;
            float4 _OutlineColor;
            float4 _MainTex_TexelSize;
            float _AlphaSplitEnabled;

            v2f vertexFunc(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color;
                #ifdef PIXELSNAP_ON
                o.pos = UnityPixelSnap(o.pos);
                #endif

                return o;
            }

            fixed4 SampleSpriteTexture(float2 uv)
            {
                fixed4 color = tex2D(_MainTex, uv);
                #if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
                #endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

                return color;
            }

            fixed4 fragmentFunc(v2f i) : COLOR
            {
                //fixed4 c = SampleSpriteTexture(i.texcoord) * i.color;
                fixed4 c = SampleSpriteTexture(i.texcoord) * i.color;
                c.rgb *= c.a;

                fixed4 outlineC = _OutlineColor;
                if(_Outline == 0)
                {
                    outlineC = _NullColor;
                }
                outlineC.rgb *= outlineC.a;

                fixed myAlpha = c.a;
                fixed upAlpha = SampleSpriteTexture(i.texcoord + fixed2(0, _MainTex_TexelSize.y)).a;
                fixed downAlpha = SampleSpriteTexture(i.texcoord - fixed2(0, _MainTex_TexelSize.y)).a;
                fixed rightAlpha = SampleSpriteTexture(i.texcoord + fixed2(_MainTex_TexelSize.x, 0)).a;
                fixed leftAlpha = SampleSpriteTexture(i.texcoord - fixed2(_MainTex_TexelSize.x, 0)).a;

                return lerp(c, outlineC, ceil( clamp(downAlpha + upAlpha + leftAlpha + rightAlpha, 0, 1) ) - ceil(myAlpha));
            }
            ENDCG

        }
    }
}
