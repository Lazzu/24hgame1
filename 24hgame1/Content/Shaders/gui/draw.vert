#version 330 core

layout(location = 0) in vec3 vertex;

uniform mat4 mP;
uniform mat4 mM;

out vec2 tc;

void main(){
	gl_Position = mP * mM * vec4(vertex, 1.0);
	tc = vertex.xy;
}
