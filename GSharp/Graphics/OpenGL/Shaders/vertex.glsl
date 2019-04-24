#version 330 core
layout (location = 0) in vec2 v_position;
layout (location = 1) in vec3 v_color;

uniform mat4 projection;

out vec3 f_color;

void main() {
	f_color = v_color;
	gl_Position = projection * vec4(v_position, 0.0, 1.0);
}