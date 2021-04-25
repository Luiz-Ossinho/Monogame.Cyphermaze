#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// size of each letter on screen
// in % of screen, not pixels
float2 dstLetterSize;

// How many letters are in the textTex
int numLetters;

// The source game render
texture2D sourceTex;
sampler2D SourceTextureSampler = sampler_state
{
    Texture = <sourceTex>;
    // note that we use POINT filtering to avoid
    // blending and mag-filtering
    MinFilter = point;
    MagFilter = point;
    MipFilter = point;
    AddressU = wrap;
    AddressV = wrap;
};

// The ASCII characters actually drawn
texture2D textTex;
sampler2D TextTextureSampler = sampler_state
{
    Texture = <textTex>;
    MinFilter = point;
    MagFilter = point;
    MipFilter = point;
    AddressU = wrap;
    AddressV = wrap;
};

// The incoming vertex data
struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 TexCoords : TEXCOORD0;
};

// The outgoing vertex data
struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float2 TexCoords : TEXCOORD0;
};

// The vertex shader... we dont do anything here
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    
    output.Position = input.Position;
    output.TexCoords = input.TexCoords;

    return output;
}

// The pixel shader... here we go!
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	// Transform the screen-space texCoords to Letter-Space
	float2 texCoords = float2(input.TexCoords.x-((int)(input.TexCoords.x/dstLetterSize.x)*dstLetterSize.x),
	                          input.TexCoords.y-((int)(input.TexCoords.y/dstLetterSize.y)*dstLetterSize.y));
	texCoords /= dstLetterSize;
	// now its in textTex space, make it one letter
	texCoords.x /= numLetters;
	
	// Get the color of the pixel at this position
	float4 fullValueColor = tex2D(SourceTextureSampler,input.TexCoords);
	// find the HSV-Value of this color
	float maxV = max(fullValueColor.r,max(fullValueColor.g,fullValueColor.b));
	// find the HSL-Luminosity of this color
	float lum = (fullValueColor.r+fullValueColor.g+fullValueColor.b)/3.0;
	// Find the inverse operand to make the Value of the color 1.0
	float inv = 1.0/maxV;
	// Multiply by the inv so the color is now full bright
	fullValueColor.rgb *= inv;
	
	// determine which ASCII letter to use
	int ind = ((numLetters-1)-max(0,min((numLetters-1),(int)(lum*numLetters))));
	// offset the ASCII texCoords by that index
	texCoords.x += ind*(1.0/numLetters);
	
	// And get the value of that piece of the letter and multiply
	// by the full-bright color
	return fullValueColor * tex2D(TextTextureSampler,texCoords);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}
