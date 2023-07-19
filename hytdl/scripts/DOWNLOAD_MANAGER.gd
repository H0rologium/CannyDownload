extends Node



signal sig_invalid_url
signal sig_invalid_save_location
signal sig_console_error
signal sig_dl_start
signal sig_dl_complete
signal sig_update_available
var latestConsoleError:String = ""

func StartDownload():
	var ROOT = get_tree().get_current_scene().get_node("/root/ROOT")
	var ytlink = ROOT.get_node("LineEdit").text
	var dlPath = ROOT.SelectedPath
	if ValidateLink(ytlink) == false: 
		sig_invalid_url.emit()
		return
	if ValidateDLLoc(dlPath) == false:
		sig_invalid_save_location.emit()
		return
	ROOT.get_node("pan").play()
	sig_dl_start.emit()
	await get_tree().create_timer(2.0).timeout
	ROOT.StartDownload(ytlink,dlPath)
	
	return
	
func DLComplete():
	sig_dl_complete.emit()
	return
	
func EmitConsoleError(exception:String):
	latestConsoleError = exception
	sig_console_error.emit()
	return

func ValidateLink(link:String):
	if len(link) < 15:
		return false
	if "https://" not in link:
		return false
	return true
	
	
func ValidateDLLoc(path:String):
	if path == "none":
		return false
	if !DirAccess.dir_exists_absolute(path):
		return false
	return true
