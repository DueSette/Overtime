/*
====================================================================================================
Structs
====================================================================================================
*/
// The output structure of the tessellation domain shader and the input structure of the fragment shader
struct CustomVertexOutput
{
	float4 clipPos      :SV_POSITION; // Qi times the space under the clipping space
	float4 uv			:TEXCOORD0; // UV coordinates, xy main texture UV, zw, normal texture UV.				 
	float3 posWS		:TEXCOORD2; // coordinates in world space
	float4 normal		:TEXCOORD3;  // normal
	float4 tangent		:TEXCOORD4;  // Tangent
};

// The structure of the surface subdivision factor, the general structure, and the UnityTessellationFactor structure defined in the unity buildIn shader
struct TessellationFactors
{
	float edge[3]:SV_TessFactor; //The subdivision factor of the three sides of the triangle
	float inside : SV_InsideTessFactor; // The subdivision factor inside the triangle
};



/*
====================================================================================================
Hull Shader
====================================================================================================
*/
TessellationFactors HullConstant(InputPatch<CustomVaryingsMeshToDS, 3> input)
{
	// Here the subdivision factor needs to be calculated according to different needs, here directly write the bare number, you can refer to several ways of surface subdivision in the Buildin shader.
	float4 tf = float4(_TessAmount, _TessAmount, _TessAmount, _TessAmount / 2);// GetTessellationFactors(p0,p1,p2,n0,n1,n2);
	TessellationFactors output;
	output.edge[0] = min(tf.x, MAX_TESSELLATION_FACTORS);
	output.edge[1] = min(tf.y, MAX_TESSELLATION_FACTORS);
	output.edge[2] = min(tf.z, MAX_TESSELLATION_FACTORS);
	output.inside = min(tf.w, MAX_TESSELLATION_FACTORS);
	return output;
}

[domain("tri")] // Processing triangle face
[partitioning("integer")]  // The parameter type of the subdivided factor, here integer is used to represent the integer, which can be a floating point number "fractional_odd"
[outputtopology("triangle_cw")] // Clockwise vertex arranged as the front of the triangle
[patchconstantfunc("HullConstant")] // The function that calculates the factor of the triangle facet is not a constant. Different triangle faces can have different values. A constant can be understood as a uniform value for the three vertices inside a triangle face.
[outputcontrolpoints(3)] // explicitly point out that each patch handles three vertex data
CustomVaryingsMeshToDS Hull(InputPatch<CustomVaryingsMeshToDS, 3> input, uint id:SV_OutputControlPointID)
{
	CustomVaryingsMeshToDS v = input[id];
	return v;
}



/*
====================================================================================================
Tessellated Vertex Displacement
====================================================================================================
*/
CustomVertexOutput VertTesselation(CustomVaryingsMeshToDS input, float offset)
{
	CustomVertexOutput output = (CustomVertexOutput)0;

	// Getting vertex position in screen space
	float4 worldPos = float4(input.posWS.xyz, 1.0);
	float4 screenPos = mul(UNITY_MATRIX_VP, float4(input.posWS.xyz, 1.0));

	// Handling glitch effect displacement
	float glitchDisplacement = tex2Dlod(_NoiseTex, float4(0, (worldPos.y / _DispScale), 0, 0)).z;
	glitchDisplacement = (glitchDisplacement * 2) - 1;
	if (glitchDisplacement < _DispRange.y)
	{
		glitchDisplacement = -1;
	}
	else if (glitchDisplacement > _DispRange.z)
	{
		glitchDisplacement = 1;
	}
	else
	{
		glitchDisplacement = 0;
	}

	// Finalising Displacement
	glitchDisplacement *= (_DispStrength);
	glitchDisplacement += (offset);
	glitchDisplacement *= ceil(_Intensity);
	screenPos.x += glitchDisplacement;

	// Returning vertex position to model space
	float4 newWorldPos = mul(inverse(UNITY_MATRIX_VP), screenPos);

	// Finalizing Details
	output.clipPos = screenPos;
	output.posWS = newWorldPos;
	output.uv.xy = input.uv;
	output.normal = float4(input.normal, 1);
	output.tangent = input.tangent;
	return output;
}



/*
====================================================================================================
Domain Shader
====================================================================================================
*/
[domain("tri")] // specified to handle the triangle face triangle
CustomVertexOutput DomainDefault(TessellationFactors tessFactors, const OutputPatch<CustomVaryingsMeshToDS, 3> input, float3 baryCoords:SV_DomainLocation)
{
	CustomVaryingsMeshToDS data;

	// This uses a macro definition to reduce the writing of duplicate code
#define MY_DOMAIN_PROGRAM_INTERPOLATE(fieldName) data.fieldName = \
					input[0].fieldName * baryCoords.x + \
					input[1].fieldName * baryCoords.y + \
					input[2].fieldName * baryCoords.z;

	MY_DOMAIN_PROGRAM_INTERPOLATE(posWS) // Interpolation calculates vertex coordinates
		MY_DOMAIN_PROGRAM_INTERPOLATE(normal) // Interpolation calculation normal
		MY_DOMAIN_PROGRAM_INTERPOLATE(tangent) // Interpolation calculation tangent
		MY_DOMAIN_PROGRAM_INTERPOLATE(uv) // Interpolation calculation UV
		return VertTesselation(data, 0); // Process the interpolation results, prepare the data needed in the rasterization phase
}

[domain("tri")] // specified to handle the triangle face triangle
CustomVertexOutput DomainOffsetRed(TessellationFactors tessFactors, const OutputPatch<CustomVaryingsMeshToDS, 3> input, float3 baryCoords:SV_DomainLocation)
{
	CustomVaryingsMeshToDS data;

	// This uses a macro definition to reduce the writing of duplicate code
#define MY_DOMAIN_PROGRAM_INTERPOLATE(fieldName) data.fieldName = \
					input[0].fieldName * baryCoords.x + \
					input[1].fieldName * baryCoords.y + \
					input[2].fieldName * baryCoords.z;

	MY_DOMAIN_PROGRAM_INTERPOLATE(posWS) // Interpolation calculates vertex coordinates
		MY_DOMAIN_PROGRAM_INTERPOLATE(normal) // Interpolation calculation normal
		MY_DOMAIN_PROGRAM_INTERPOLATE(tangent) // Interpolation calculation tangent
		MY_DOMAIN_PROGRAM_INTERPOLATE(uv) // Interpolation calculation UV
		return VertTesselation(data, -_ChromAbberOffset); // Process the interpolation results, prepare the data needed in the rasterization phase
}

[domain("tri")] // specified to handle the triangle face triangle
CustomVertexOutput DomainOffsetBlue(TessellationFactors tessFactors, const OutputPatch<CustomVaryingsMeshToDS, 3> input, float3 baryCoords:SV_DomainLocation)
{
	CustomVaryingsMeshToDS data;

	// This uses a macro definition to reduce the writing of duplicate code
#define MY_DOMAIN_PROGRAM_INTERPOLATE(fieldName) data.fieldName = \
					input[0].fieldName * baryCoords.x + \
					input[1].fieldName * baryCoords.y + \
					input[2].fieldName * baryCoords.z;

	MY_DOMAIN_PROGRAM_INTERPOLATE(posWS) // Interpolation calculates vertex coordinates
		MY_DOMAIN_PROGRAM_INTERPOLATE(normal) // Interpolation calculation normal
		MY_DOMAIN_PROGRAM_INTERPOLATE(tangent) // Interpolation calculation tangent
		MY_DOMAIN_PROGRAM_INTERPOLATE(uv) // Interpolation calculation UV
		return VertTesselation(data, _ChromAbberOffset); // Process the interpolation results, prepare the data needed in the rasterization phase
}