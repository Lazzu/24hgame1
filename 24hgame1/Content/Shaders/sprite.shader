<?xml version="1.0" encoding="utf-8"?>
<ShaderProgram xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" Name="SpriteShader">
  <Shaders>
    <Shader>
      <Data>sprite.vert</Data>
      <Inline>false</Inline>
      <Type>VertexShader</Type>
    </Shader>
    <Shader>
      <Data>sprite.frag</Data>
      <Inline>false</Inline>
      <Type>FragmentShader</Type>
    </Shader>
  </Shaders>
  <Uniforms>
    <Uniform>mP</Uniform>
    <Uniform>textureSampler</Uniform>
  </Uniforms>
</ShaderProgram>