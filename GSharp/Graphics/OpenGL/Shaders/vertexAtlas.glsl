#version 330 core
layout (location = 0) in vec2 v_position;
layout (location = 1) in vec2 v_texCoords;
layout (location = 2) in vec4 v_texLocation;

out vec2 f_texCoords;

uniform mat4 projection;
uniform vec2 atlasSize;

void main() {
	vec2 texCoords = vec2(v_texLocation.x, atlasSize.y - v_texLocation.y - v_texLocation.w) / atlasSize;
	vec2 size = vec2(v_texLocation.z, v_texLocation.w) * v_texCoords / atlasSize;
	f_texCoords = texCoords + size;

	gl_Position = projection * vec4(v_position, 0.0, 1.0);
}