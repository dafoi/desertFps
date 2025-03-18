extends VoxelGI
func _physics_process(delta):
	if(Input.is_action_just_pressed("v")):
		visible = !visible
