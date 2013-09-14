#version 330 core

layout(location = 0) in vec3 TranslateData;
layout(location = 1) in vec3 Texdata;
layout(location = 2) in vec4 Color;

uniform mat4 mP;

out vec4 tc;
out vec4 color;

void main(){
	gl_Position = mP * vec4(TranslateData.x, TranslateData.y, 0, 1.0);
	gl_PointSize = TranslateData.z;
	tc = vec4(Texdata.x, Texdata.y, Texdata.z, Texdata.z);
	color = Color;
}

