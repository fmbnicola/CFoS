Shader "Supershape2DShader"
{
	Properties
	{
		_Scale("Scale", Float) = 1
		_Antialias("Anti-aliasing", Float) = 0.1
		_Color("Color", Color) = (1., 1., 1., 1.)

		_A ("A",  Range(0, 1 )) = 1
		_B ("B",  Range(0, 1 )) = 1
		_N1("N1", Range(0, 2 )) = 1
		_N2("N2", Range(0, 2 )) = 1
		_N3("N3", Range(0, 2 )) = 1
		_M ("M",  Range(0, 50)) = 1
    }
    SubShader
    {
        Tags {"RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "Queue" = "Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		ZTest LEqual
		Cull off
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog // make fog work
			#pragma multi_compile_instancing // instancing

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };


			float _Antialias;

			UNITY_INSTANCING_BUFFER_START(Props)
				UNITY_DEFINE_INSTANCED_PROP(float, _Scale)
				UNITY_DEFINE_INSTANCED_PROP(float4, _Color)

				UNITY_DEFINE_INSTANCED_PROP(float, _A)
				UNITY_DEFINE_INSTANCED_PROP(float, _B)
				UNITY_DEFINE_INSTANCED_PROP(float, _N1)
				UNITY_DEFINE_INSTANCED_PROP(float, _N2)
				UNITY_DEFINE_INSTANCED_PROP(float, _N3)
				UNITY_DEFINE_INSTANCED_PROP(float, _M)
			UNITY_INSTANCING_BUFFER_END(Props)


            v2f vert (appdata v)
            {
                v2f o;

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }


			float super(float phi, float a, float b, float n1, float n2, float n3, float m)
			{
				return pow(pow(abs(cos(m*phi/4.0)/a), n2) + pow(abs(sin(m*phi/4.0)/b), n3), -1.0 / n1);
			}


			float outline(float distance, float linewidth, float antialias)
			{
				float t = linewidth / 2.0 - antialias;
				float border_distance = abs(distance) - t;
				float alpha = border_distance / antialias;
				alpha = exp(-alpha * alpha);
				
				if (border_distance < 0.0)
					return 1;

				else if (distance < 0.0)
					return lerp(0, 1, sqrt(alpha));
				
				else
					return 1 * alpha;
			}


            fixed4 frag (v2f i) : SV_Target
            {
				UNITY_SETUP_INSTANCE_ID(i);

				// Get instanced props
				float  scale = UNITY_ACCESS_INSTANCED_PROP(Props, _Scale);
				float4 color = UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
			
				float b  = UNITY_ACCESS_INSTANCED_PROP(Props, _B);
				float a  = UNITY_ACCESS_INSTANCED_PROP(Props, _A);
				float n1 = UNITY_ACCESS_INSTANCED_PROP(Props, _N1);
				float n2 = UNITY_ACCESS_INSTANCED_PROP(Props, _N2);
				float n3 = UNITY_ACCESS_INSTANCED_PROP(Props, _N3);
				float m  = UNITY_ACCESS_INSTANCED_PROP(Props, _M);

				// center + convert to polar coordinates (x,y) -> (angle, dist)
				float2 uv = i.uv;
				uv = 2 * (uv - 0.5);
				uv = float2(atan2(uv.y, uv.x), length(uv));

				// Calculate r
				float r = super(uv.x, a, b, n1, n2, n3, m);
				
				// Scaling
				r = r * scale;

				// signed distance
				float d = (uv.y - r);

				// outline
				// float val = outline(d, 0.02, _Antialias * 0.01);

				// fill
				float val = smoothstep(-_Antialias, _Antialias, -d / fwidth(-d));

				// Set color and alpha
				float4 col = float4(color.r * val, color.g * val, color.b * val, val);
                
				// apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
