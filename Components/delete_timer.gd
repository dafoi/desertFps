extends Timer
@export var spawnNext : PackedScene
@export var spawnNextPosition : Node3D
func _on_timeout():
	if(spawnNext):
		
		var node = spawnNext.instantiate() as Node3D
		get_parent().get_parent().add_child(node)
		if(spawnNextPosition):			
			node.global_position = spawnNextPosition.global_position
	get_parent().queue_free()
	pass # Replace with function body.
