extends WorldEnvironment
func _input(event):
	if(event.is_action_pressed("b")):
		environment.volumetric_fog_enabled = !environment.volumetric_fog_enabled
