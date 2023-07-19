# CannyDownload
A canny cat to help you get your files!
This is a windows-only UI written by me while delirious to wrap [ytdl-patched](https://github.com/ytdl-patched/ytdl-patched). It started as a serious project but I started throwing a bunch of uncanny cats in it.

## Usage

1. Open the program, paste the applicable link into the top box.
2. Select a path from the bottom of the window to save the file to. If every entry reads 'none', you will need to press the "New Location" button to add a **folder** path
3. Ensure the proper path is selected (thinner, highlighted white)
4. Press "Download" and enjoy

Settings can be accessed at the bottom right of the program.

## Installation

**YOU NEED TO HAVE FFMPEG ADDED TO YOUR ENVIRONMENT PATH**
### To install ffmpeg:
[You should use this specific version of FFMPEG](https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip)
If you already have a version installed it should be fine but [as per ytdl team ffmpeg versions can occaisionally break stuff](https://github.com/ytdl-patched/ytdl-patched/blob/ytdlp/README.md)

Extract the FFMPEG folder wherever you please

To modify the Windows PATH environment variable (WIN-10)
<ol>
<li>Open the start menu and search for "Edit the System Environment Variables"</li>
<li>In the System Properties dialog box, click the Advanced tab if not open already.</li>
<li>Click Environment Variables at the bottom.</li>
<li>In the top list, scroll down to the PATH variable, select it, and click Edit. Note: If the PATH variable does not exist, click New and enter PATH for the Variable Name (Case sensitive).</li>
<li>In the Variable Value box, Press New -> Browse. Browse to the extraced FFMPEG folder, select the 'bin' folder inside and press OK.</li>
<li>Click OK to close each dialog box.</li>
</ol>

If downloads start failing without errors try adding this PATH to the system PATH rather than the user PATH.
