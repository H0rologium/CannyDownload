extends Node

@onready var pickerNode = $LineEdit
@onready var bg = $Sprite2D
@onready var btn = $Button
@onready var ROOT = self.get_parent()

func PickNew():
	pickerNode.visible = true
	bg.visible = true
	btn.visible = true
	$"../bong".play()
	return


func _on_file_dialog_dir_selected(dir):
	$"../bong".play()
	pickerNode.visible = false
	bg.visible = false
	btn.visible = false
	if len(dir) < 3: return
	if dir == "useVal":
		dir = get_parent().get_node("LineEdit").text
	ROOT.AppendNewSaveLocation(dir,true)
	return


func _on_start_dl_btn_pressed():
	DownloadManager.StartDownload()
	return
