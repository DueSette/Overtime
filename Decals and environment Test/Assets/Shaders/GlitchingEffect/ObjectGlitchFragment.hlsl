// fragment shader
half4 FragLit(CustomVertexOutput IN) : SV_Target
{
	// Get the color of the generated glitch texture
	float4 glitch = tex2D(_NoiseTex, IN.uv);

	float thresh = 1.001 - _Intensity * 1.001;
	float w_d = step(thresh, pow(glitch.z, 2.5)); // displacement glitch
	float w_c = step(thresh, pow(glitch.z, 3.5)); // color glitch

	// Handling glitch effect displacement
	float glitchDisplacement = tex2D(_NoiseTex, float4(0, (IN.posWS.y / _DispScale), 0, 0)).z;
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
	glitchDisplacement = abs(glitchDisplacement);
	glitchDisplacement *= ceil(_Intensity);

	// Displacement
	float2 uv = frac(IN.uv + glitch.xy * (0.1 * _Intensity * glitch.z));

	float4 source = tex2D(_MainTex, uv);
	source = source * _Color;

	float4 trash = tex2D(_DetailTex, uv) * tex2D(_TrashTex, uv);

	// Albedo comes from a texture tinted by color
	float4 c = tex2D(_MainTex, IN.uv) * _Color;

	// Shuffle color components.
	float4 color = lerp(source, trash, glitchDisplacement);
	return color;
}

// fragment shader
half4 FragRed(CustomVertexOutput IN) : SV_Target
{
	// Shuffle color components.
	float4 color = float4(1.0, 0.0, 0.0, 0.5);
	return color;
}

half4 FragBlue(CustomVertexOutput IN) : SV_Target
{
	// Shuffle color components.
	float4 color = float4(0.0, 0.0, 1.0, 0.5);
	return color;
}

half4 FragPos(CustomVertexOutput IN) : SV_Target
{
	// Shuffle color components.
	float4 pos = float4(IN.posWS.xyz, 1.0);
	float4 color = float4(pos.y, pos.y, pos.y, 1.0);

	return color;
}