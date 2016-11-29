Shader "Custom/DepthGrayscale" {
SubShader{
Tags{ "RenderType" = "Opaque" }

Pass{
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		sampler2D _CameraDepthTexture;

struct v2f {
	float4 pos : SV_POSITION;
	float4 scrPos:TEXCOORD1;
	float depth : TEXCOORD0;
};

struct fragOut
{
	half4 color : SV_Target;
	float depth : SV_Depth;
};

	//Vertex Shader
v2f vert(appdata_base v) {
	v2f o;
	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	o.scrPos = ComputeScreenPos(o.pos);
	UNITY_TRANSFER_DEPTH(o.depth);
	return o;
}

//Fragment Shader
fragOut frag(v2f i){
	fragOut o;

	float depthValue = Linear01Depth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)).r);
	//half depth;

	o.color.g = fmod(depthValue * 256, 1);
	o.color.b = floor(depthValue * 256)/256;
	//UNITY_OUTPUT_DEPTH(i.depth);
	//depth.g = depthValue;
	//depth.b = depthValue;
	o.depth = depthValue;

	//depth.a = 1;
	return o;
}

ENDCG
}
}
FallBack "Diffuse"
}