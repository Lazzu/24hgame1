#version 330 core

in vec2 tc;

out vec4 color;

uniform sampler2D Texture;
 
void main(){
	vec4 c = texture( Texture, tc );

	if(c.a == 0) discard;

	color = c;
}
