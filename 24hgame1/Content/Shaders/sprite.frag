#version 330 core

in vec2 tc;

out vec4 color;

uniform sampler2D textureSampler;
 
void main(){
	//color = vec4(texture2D( textureSampler, tc ).rgb,1);
	color = vec4(1,1,1,1);
}
