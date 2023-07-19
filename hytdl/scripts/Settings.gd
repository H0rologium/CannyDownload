extends Node2D
@onready var ROOT = get_tree().get_current_scene().get_node("/root/ROOT")


func _ready():
	$updatebtn.visible = false
	return


func _on_close_pressed():
	get_tree().get_current_scene().get_node("/root/ROOT/SettingsSubWindow").ToggleSettingsMenu()
	return


func _on_erasavloc_pressed():
	ROOT.EraseSavedPaths()
	for node in get_tree().get_nodes_in_group("HISTORYPATH"):
		node.text = "none"
	return

func EnableUpdateBtn():
	$updatebtn.visible = true
	return


func _on_updatebtn_pressed():
	ROOT.call("RunConsoleEnv","start https://github.com/H0rologium/CannyDownload/releases/latest",false)
	return
