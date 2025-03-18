
extends ColorRect

@export var light_path : NodePath
@onready var light : DirectionalLight3D = get_node(light_path)   

@export var camera_path : NodePath
@onready var camera : Camera3D = get_node(camera_path)

func _process(_delta: float) -> void:
	var pos = camera.unproject_position(camera.global_position - (-light.global_basis.z.normalized()))   
	material.set_shader_parameter("light_source_pos", pos)
	material.set_shader_parameter("light_source_dir", -light.global_basis.z)
	material.set_shader_parameter("camera_dir", -camera.global_basis.z)
