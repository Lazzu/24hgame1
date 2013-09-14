#version 330 core

in vec4 tc;

out vec4 color;

uniform sampler2D textureSampler;
 
void main(){
	//vec2 texcoord = vec2(tc.x + (tc.z * gl_PointCoord.x), tc.y + (tc.w * gl_PointCoord.y));
	color = texture2D( textureSampler, gl_PointCoord );
	//color = vec4(1,1,1,1);
}
