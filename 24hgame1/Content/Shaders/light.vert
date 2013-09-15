#version 330 core

layout(location = 0) in vec3 vertex;

uniform mat4 mP;
uniform mat4 mV;
uniform mat4 mM;

uniform vec2 ScreenSize;

out vec2 tc;

void main(){
	gl_Position = mP * mV * mM * vec4(vertex, 1.0);
	tc = vertex.xy;
}


