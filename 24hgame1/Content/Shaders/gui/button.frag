#version 330 core

in vec2 tc;

out vec4 c;

uniform vec4 color;
uniform float fade;
 
void main(){
	c = vec4(color.rgb - (color.rgb * (1-tc.y) * fade), color.a);
}
