#version 330 core

in vec2 tc;

out vec4 c;

uniform vec4 color;
 
void main(){
	c = color;
}
