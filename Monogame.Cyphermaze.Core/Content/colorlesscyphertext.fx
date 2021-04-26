#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float2 dstLetterSize;
int numLetters;
texture2D textTex;
texture2D sourceTex;

sampler2D TextTextureSampler = sampler_state
{
    Texture = <textTex>;
    MinFilter = point;
    MagFilter = point;
    MipFilter = point;
    AddressU = wrap;
    AddressV = wrap;
};
sampler2D SourceTextureSampler = sampler_state
{
    Texture = <sourceTex>;
    MinFilter = point;
    MagFilter = point;
    MipFilter = point;
    AddressU = wrap;
    AddressV = wrap;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 TexCoords : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float2 TexCoords : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    output.Position = input.Position;
    output.TexCoords = input.TexCoords;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
     float4 sourceColor = tex2D(SourceTextureSampler,input.TexCoords);

     float2 texCoords = float2(
          input.TexCoords.x - ((int)
               (input.TexCoords.x / dstLetterSize.x) * dstLetterSize.x),
          input.TexCoords.y - ((int)
               (input.TexCoords.y / dstLetterSize.y) * dstLetterSize.y));
     texCoords /= dstLetterSize;
     texCoords.x /= numLetters;

     float lum = (sourceColor.r + sourceColor.g + sourceColor.b) / 3.0;
     int ind = ((numLetters - 1) - max(0, min((numLetters - 1), (int)(lum * numLetters))));
     texCoords.x += ind * (1.0 / numLetters);

     float4 letterColor = tex2D(TextTextureSampler,texCoords);

     return letterColor;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}