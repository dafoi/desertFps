shader_type canvas_item;
render_mode unshaded, blend_add;

uniform sampler2D subviewport_tex : filter_linear;
uniform sampler2D light_color : hint_default_white;

uniform float ray_length : hint_range(0.0, 1.0) = 1.0;
uniform float ray_intensity : hint_range(0.0, 1.0) = 1.0;
uniform float light_source_scale = 1.0;
uniform float light_source_feather = 2.0;
uniform float noise_strength : hint_range(0.0, 1.0) = 0.2;

uniform vec2 light_source_pos = vec2(0.0, 0.0);
uniform vec3 light_source_dir = vec3(0.5, -1.0, 0.25);
uniform vec3 camera_dir = vec3(-0.5, 1.0, -0.25);

const int SAMPLE_COUNT = 200;

float random (vec2 uv) {
    return fract(sin(dot(uv.xy, vec2(12.9898,78.233))) * 43758.5453123);
}

void fragment() {
	// divide light source pos by screen size for correct UV positioning
	vec2 light_pos = light_source_pos / (1.0 / SCREEN_PIXEL_SIZE);
	vec2 dir = UV - light_pos;

	// light source uv coords with correct aspect ratio for drawing circle
	vec2 ratio = vec2(SCREEN_PIXEL_SIZE.x / SCREEN_PIXEL_SIZE.y, 1.0);
	vec2 dir2 = UV / ratio - light_pos / ratio;

	float light_rays = 0.0;
	vec2 uv2;
	float scale, l;
	for (int i = 0; i < SAMPLE_COUNT; i++){
		scale = 1.0f - ray_length * (float(i) / float(SAMPLE_COUNT - 1));
		l = (1.0 - texture(subviewport_tex, dir * scale + light_pos).a);
		uv2 = dir2 * scale * pow(light_source_scale, 2.0);
		l *= smoothstep(1.0, 0.999 - light_source_feather, dot(uv2, uv2) * 4.0);
		light_rays += l / float(SAMPLE_COUNT);
	}

	// multiply with noise to reduce color banding
	float n = 1.0 - random(UV) * noise_strength * smoothstep(0.999 - 1.25, 1.0, dot(dir2, dir2) * 2.0);
	light_rays *= n;

	// multiply with camera/light dot to fade based on view angle
	float d = clamp(-dot(normalize(camera_dir), normalize(light_source_dir)), 0.0, 1.0);
	light_rays *= d;

	COLOR.rgb = vec3(light_rays * ray_intensity) * texture(light_color, UV).rgb;
}