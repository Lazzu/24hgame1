#version 330 core

in vec2 tc;

out vec4 color;

uniform sampler2D textureSampler;
 
void main(){
	color = texture2D( textureSampler, tc );
	//color = vec4(test.x, test.y, 0,1);
	//color = vec4(1);
}

