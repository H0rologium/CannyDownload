extends Window

@onready var ROOT = get_tree().get_current_scene().get_node("/root/ROOT")

func _ready():
	$"../ConfigBlink".play("default")
	self.visible = false
	DownloadManager.sig_update_available.connect(ShowUpdateWarn)
	return

var isOpen:bool = false
func ToggleSettingsMenu():
	$"../bong".play()
	self.visible = !isOpen
	isOpen = !isOpen
	if !self.visible:
		ROOT.call("CallSaveConfig")
		return
	$"../ConfigBlink".play("default")
	#Get screen dimensions
	var coords = DisplayServer.screen_get_size()
	self.position.x = coords[0]-360
	self.position.y = coords[1]-640
	return

func ShowUpdateWarn():
	$"../ConfigBlink".play("warning")
	$SETTINGSSUB.EnableUpdateBtn
	return
