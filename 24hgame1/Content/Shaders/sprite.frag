#version 330 core

in vec3 tc;

out vec4 color;

uniform sampler2D textureSampler;
 
void main(){
	vec2 texcoord = vec2(tc.x + ((tc.z - gl_PointCoord.x) * tc.z), tc.y + ((tc.z - gl_PointCoord.y) * tc.z));
	color = texture2D( textureSampler, gl_PointCoord );
	//color = vec4(gl_PointCoord.x,gl_PointCoord.y,1,1);
}
