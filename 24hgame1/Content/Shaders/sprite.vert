#version 330 core

layout(location = 0) in vec3 TranslateData;
layout(location = 1) in vec3 Texdata;
layout(location = 2) in vec4 Color;

uniform mat4 mP;
uniform mat4 mV;

out vec3 tc;
out vec4 color;

void main(){
	gl_Position = mP * mV * vec4(TranslateData.x, TranslateData.y, 0, 1.0);
	gl_PointSize = Texdata.z ;
	tc = vec3(Texdata.x, Texdata.y, Texdata.z);
	color = Color;
}

