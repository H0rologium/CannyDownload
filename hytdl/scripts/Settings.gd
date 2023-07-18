extends Node2D


func _on_close_pressed():
	get_tree().get_current_scene().get_node("/root/ROOT/SettingsSubWindow").ToggleSettingsMenu()
	return
