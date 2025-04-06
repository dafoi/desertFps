shader_type spatial;
uniform vec3 albedo : source_color;
uniform vec3 fresnel_albedo : source_color;
uniform float time_scale = 0.025;

uniform vec2 wave_dir1;
uniform vec2 wave_dir2;
uniform float metallic;
uniform float roughness;
uniform sampler2D texture_normal;
uniform sampler2D texture_normal2;

uniform vec4 color_deep : source_color;
uniform vec4 color_shallow : source_color;
uniform float beers_law = 2.0;
uniform float depth_offset = -0.75;
uniform sampler2D DEPTH_TEXTURE : hint_depth_texture;
uniform sampler2D SCREEN_TEXTURE : hint_screen_texture;
float fresnel(float amount, vec3 normal, vec3 view){
	return pow((1.0-clamp(dot(normalize(normal),normalize(view)),0.0,1.0)),amount);
}
void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	float depth_texture = texture(DEPTH_TEXTURE, SCREEN_UV).x;
	vec3 ndc = vec3(SCREEN_UV * 2.0 - 1.0, depth_texture);
	vec4 view = INV_PROJECTION_MATRIX * vec4(ndc, 1.0); 
	view.xyz /= view.w;
	float depth_blend = exp((-view.z+VERTEX.z + depth_offset) * -beers_law);
	depth_blend = clamp(1.0 - depth_blend, 0.0, 1.0);
	float depth_blend_power = clamp(pow(depth_blend, 2.5), 0.0, 1.0);
	
	
	
	
	vec2 uv1 = UV;
	uv1+=TIME*time_scale*wave_dir1;
	
	vec2 uv2 = UV;
	uv2+=TIME*time_scale*wave_dir2;
	
	vec3 normal1 = texture(texture_normal,uv1).rgb;
	vec3 normal2 = texture(texture_normal2,uv2).rgb;
	vec3 normal = mix(normal1,normal2,.5);
	float fresnel = fresnel(5.0,NORMAL,VIEW);
	vec3 screen_color = textureLod(SCREEN_TEXTURE,SCREEN_UV,depth_blend_power * 2.5).rgb;
	vec3 depth_color = mix(color_shallow.rgb,color_deep.rgb,depth_blend_power);
	vec3 color = mix(screen_color * depth_color,depth_color * 0.25, depth_blend_power * 0.5);
	
	vec3 surface_color = mix(albedo,fresnel_albedo,fresnel);
	
	
	ALBEDO = mix(surface_color,color,0.75);
	ROUGHNESS = roughness;
	METALLIC = metallic;
	NORMAL_MAP = normal;
	
	// Called for every pixel the material is visible on.
}


//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
