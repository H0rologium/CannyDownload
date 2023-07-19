extends AnimationPlayer

var isComplete:bool

#func looploadinganim(anim_name):
#	if !isComplete and anim_name == "dip":
#		self.play("dip")
#	return

func _ready():
	DownloadManager.sig_invalid_save_location.connect(ThrowNewDirError)
	DownloadManager.sig_invalid_url.connect(ThrowNewURLError)
	DownloadManager.sig_console_error.connect(ThrowNewConsoleError)
	DownloadManager.sig_dl_start.connect(DropIn)
	DownloadManager.sig_dl_complete.connect(EndDL)
	#self.animation_finished.connect(looploadinganim)
	return

func DropIn():
	self.play("dip_intro")
	self.queue("dip")
	return
	
func EndDL():
	isComplete = true
	self.stop()
	self.play("remove_dip")
	self.queue("dl_complete")
	$"../yay".play()
	return
	
func ThrowNewURLError():
	$"../pan".play()
	$errormsg.text = "[center]ERROR!\nINVALID DOWNLOAD URL"
	self.play("remove_dip")
	self.queue("ShowError")
	return
	
func ThrowNewDirError():
	$"../pan".play()
	$errormsg.text = "[center]ERROR!\nINVALID DOWNLOAD FOLDERPATH"
	self.play("remove_dip")
	self.queue("ShowError")
	return
	
func ThrowNewConsoleError():
	$"../pan".play()
	$errormsg.text = "[center]ERROR!\n%s"%DownloadManager.latestConsoleError
	self.play("remove_dip")
	self.queue("ShowError")
	return
