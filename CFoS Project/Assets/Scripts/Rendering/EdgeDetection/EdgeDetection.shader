/* 
Template Addapted from this project by @yetmania: https://github.com/yahiaetman/URPCustomPostProcessingStack
Shader Technique described here by : https://halisavakis.com/my-take-on-shaders-edge-detection-image-effect/ 
*/


Shader "/CustomPostProcess/EdgeDetection"
{
	Properties
	{
		_MainTex("Texture", 2D)					= "white" {}
		_Color("Edge color", Color)				= (0,0,0,1)
		_Thickness("Thickness", float)			= 1.0
		_Intensity("Intensity", float)			= 1.0
		_ThresholdMin("Threshold Min", float)	= 0.0
		_ThresholdMax("Threshold Max", float)	= 0.2
	}

    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
	
	TEXTURE2D_X(_MainTex);
	SAMPLER(sampler_linear_clamp);
    float4 _MainTex_TexelSize;

    float3 _Color;
    float _Thickness;
    float _Intensity;
	float _ThresholdMin;
	float _ThresholdMax;
 
	struct VertexAttributes
	{
		uint vertexID : SV_VertexID;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct Varyings
	{
		float4 positionCS : SV_POSITION;
		float2 texcoord   : TEXCOORD0;
		UNITY_VERTEX_OUTPUT_STEREO
	};


    // Sample Scene Depth
    float SampleDepth(float2 uv)
	{
		float depth = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_linear_clamp, UnityStereoTransformScreenSpaceTex(uv)).r;
        float depthEye = LinearEyeDepth(depth, _ZBufferParams);
        
		return depthEye;
    }

    // Read the 8 surrounding samples and average them
    float SampleNeighborhood(float2 uv, float thickness)
	{    
		// The surrounding pixel offsets
        const float2 offsets[8] = {
            float2(-1, -1),
            float2(-1, 0),
            float2(-1, 1),
            float2(0, -1),
            float2(0, 1),
            float2(1, -1),
            float2(1, 0),
            float2(1, 1)
        };
        
		// The sum is multiplied by weight for perspecive correction
        float2 delta = _MainTex_TexelSize.xy * thickness;
        float weight = 0;
        for(int i=0; i<8; i++){
            float sample = SampleDepth(uv + delta * offsets[i]);
			weight += 1 / sample;
        }
		return 8/weight;
    }


	// VERTEX SHADER 
	Varyings FullScreenVertexProgram(VertexAttributes input)
	{
		Varyings output;
		UNITY_SETUP_INSTANCE_ID(input);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

		// Convert vertexID (0,1,2) to a clip-space postion
		output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
		output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);
		return output;
	}

	// FRAGMENT SHADER
    float4 EdgeDetectionFragmentProgram (Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

		// Get UVs and Main Tex
        float2 uv = UnityStereoTransformScreenSpaceTex(input.texcoord);
		float4 color = LOAD_TEXTURE2D_X(_MainTex, input.positionCS.xy);

		// All samples
		float depth = SampleDepth(uv);
		float neighborhood = SampleNeighborhood(uv, _Thickness);

		// Depth similarity
		float depthSame = smoothstep(_ThresholdMax * depth, _ThresholdMin * depth, abs(depth - neighborhood));
		float edge = 1 - depthSame;

		color.rgb = lerp(color.rgb, _Color, edge * _Intensity);

        return color;
    }
    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            HLSLPROGRAM
            #pragma vertex FullScreenVertexProgram
            #pragma fragment EdgeDetectionFragmentProgram
            ENDHLSL
        }
    }
    Fallback Off
}
