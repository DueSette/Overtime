Shader "Custom/ObjectGlitch"
{
    Properties
    {
        // Tessellation Parameters
        _TessAmount("Tessellation Amount", Range(1.0, 64.0)) = 15.0

        // Surface Glitching Parameters
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}

        _NoiseTex("Noise Texture", 2D) = "white" {}
        _TrashTex("Trash Texture", 2D) = "white" {}
        _DetailTex("Trash Detail Texture", 2D) = "white" {}
        _DispStrength("Displacement Strength", float) = 1.0
        _DispScale("Displacement Scale", float) = 1.0
        _DispRange("Displacement Range", vector) = (-1.0, -0.663, -0.334, 1.0)
        _ChromAbberOffset("Chromatic Abberation Offset", Range(0.0, 1.0)) = 0.0
        _Intensity("Intensity", float) = 0.0
    }

    HLSLINCLUDE

    #pragma target 4.5

    #define HAVE_VERTEX_MODIFICATION

    ENDHLSL

    SubShader
    {
        Tags{ "RenderPipeline" = "HDRenderPipeline"}
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "Main Colored"

            ZWrite On

            HLSLPROGRAM

            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"

            #include "ObjectGlitchVertex.hlsl"
            #include "ObjectGlitchTessellation.hlsl"
            #include "ObjectGlitchFragment.hlsl"

            #pragma vertex vert
            #pragma fragment FragLit
            #pragma hull Hull
            #pragma domain DomainDefault

            ENDHLSL
        }

        Pass
        {
            Name "Chromatic Abberation Red"

            ZWrite Off

            HLSLPROGRAM

            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"

            #include "ObjectGlitchVertex.hlsl"
            #include "ObjectGlitchTessellation.hlsl"
            #include "ObjectGlitchFragment.hlsl"

            #pragma vertex vert
            #pragma fragment FragRed
            #pragma hull Hull
            #pragma domain DomainOffsetRed

            ENDHLSL
        }

        Pass
        {
            Name "Chromatic Abberation Blue"

            ZWrite Off

            HLSLPROGRAM

            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"

            #include "ObjectGlitchVertex.hlsl"
            #include "ObjectGlitchTessellation.hlsl"
            #include "ObjectGlitchFragment.hlsl"

            #pragma vertex vert
            #pragma fragment FragBlue
            #pragma hull Hull
            #pragma domain DomainOffsetBlue

            ENDHLSL
        }
    }
}
