#version 330 core
layout (location = 0) in vec2 v_position;
layout (location = 1) in vec2 v_texCoords;

out vec2 f_texCoords;

uniform mat4 projection;

void main() {
	f_texCoords = v_texCoords;

	gl_Position = projection * vec4(v_position, 0.0, 1.0);
}