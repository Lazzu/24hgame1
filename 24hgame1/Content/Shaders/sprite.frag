#version 330 core

in vec3 tc;

out vec4 color;

uniform sampler2D textureSampler;
 
void main(){
	vec2 texcoord = vec2(tc.x + ((tc.z - tc.x) * gl_PointCoord.x), tc.y + ((tc.z - tc.y) * gl_PointCoord.y));
	color = texture2D( textureSampler, texcoord );
	//color = vec4(texcoord.x,texcoord.y,1,1);
}
