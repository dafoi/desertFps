extends Label
func _process(delta):
	text = "fps: " +  str(Engine.get_frames_per_second())
