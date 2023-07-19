using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

public partial class ROOT : Node2D
{
	private List<string> saveLocations;
	private string version;
	public string SelectedPath = "none";
	private protected readonly string CONFIG_PATH = "user://hytdl.config";

	public List<String> SaveLocations
	{
		get {return saveLocations;}
	}

	public void StartDownload(string url, string path)
	{
		string command = BuildConsoleCommand(url,path);
		RunConsoleEnv(command);
	}
	
	private string BuildConsoleCommand(string url, string path)
	{
		string retVal = "ytdl";
		retVal += $" {url} ";
		retVal += " --ignore-config ";
		retVal += " --windows-filenames ";
		retVal += " --remux-video mp4 ";
		retVal += $" -P {path}";

		return retVal;
	}

	private async void RunConsoleEnv(string command,bool silent = false)
	{
		Node global = (Node) GetNode("/root/DownloadManager");
		try {
		var p = Process.Start(new ProcessStartInfo("cmd", $"/c {command}"));
		await p.WaitForExitAsync();
		if (!silent) global.Call("DLComplete");
		} catch (Exception e) {
			
			if(!silent)global.Call("EmitConsoleError",e.ToString());
		}
	}

	public void SelectKnownPath(int path)
	{
		foreach (RichTextLabel label in GetTree().GetNodesInGroup("HISTORYPATH"))
		{
			label.AddThemeColorOverride("font_shadow_color",new Godot.Color("#000000"));
		}
		GetNode<RichTextLabel>($"histPath{path}").AddThemeColorOverride("font_shadow_color",new Godot.Color("#FFFFFF"));
		SelectedPath = GetNode<RichTextLabel>($"histPath{path}").Text;
	}


	public override void _Ready()
	{
		GD.Print($"Current operating directory: {System.Environment.CurrentDirectory}");
		GD.Print($"Setting to: {AppDomain.CurrentDomain.BaseDirectory}");
		System.Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
		saveLocations = new();
		DisplayServer.WindowSetTitle("Canny Downloader");
		//Read config
		if (!Godot.FileAccess.FileExists(CONFIG_PATH)) SaveToConfig(true);
		else {LoadConfig();}
		//Populate history paths
		if (SaveLocations.Count > 0)
		{
			foreach (string loc in SaveLocations) AppendNewSaveLocation(loc);
		}
		//Grab ytdl if not present
		if (!System.IO.File.Exists($"{System.Environment.CurrentDirectory}/ytdl.exe") && !System.Environment.UserName.ToLower().Equals("horologium"))
		{
			using (var client = new System.Net.Http.HttpClient())
			{
                using (var s = client.GetStreamAsync("https://github.com/ytdl-patched/ytdl-patched/releases/latest/download/ytdl-patched-red.exe"))
                {
                    using (var fs = new FileStream($"ytdl.exe", FileMode.OpenOrCreate))
                    {
                        s.Result.CopyTo(fs);
                    }
                }
            }
		}
		if (!System.IO.File.Exists($"{System.Environment.CurrentDirectory}/ytdl.exe")) GD.PrintErr("Unable to retrieve latest version of ytdl! do not report this to ytdl");
		else RunConsoleEnv("ytdl -U",true);
	}

	private void AppendNewSaveLocation(string path,bool addToCollection = false)
	{
		path = path.Replace('\\','/');
		GD.Print($"Appending new path for {path}");
		if (SaveLocations.Count >= 6)
		{
			SaveLocations.RemoveAt(0);
		}
		int i = 1;
		if (addToCollection)SaveLocations.Add(path);
		foreach (string location in SaveLocations)
		{
			if (location.Length < 3) continue;
			GetNode<RichTextLabel>($"histPath{i}").Text = location;
			i++;
		}
		if (OS.IsDebugBuild())foreach(string it in SaveLocations) GD.Print(it);
		SaveToConfig();
	}

	//Thanks godot...NOT
	public void CallSaveConfig(){
		 SaveToConfig();
	}

	public void EraseSavedPaths()
	{
		SaveLocations.Clear();
		GetNode<AudioStreamPlayer>("/root/ROOT/pan").Play();
	}

	public void UpgradeVersion()
	{
		GD.Print($"Upgrading config version to {DEFAULT_CONFIG["version"]}");

	}

	public void SaveToConfig(bool makeNew = false)
	{
		if (makeNew)
		{
			GD.Print("Creating a new config file, none found");
			Godot.FileAccess tmp = Godot.FileAccess.Open(CONFIG_PATH,Godot.FileAccess.ModeFlags.Write);
			string newLine = Json.Stringify(DEFAULT_CONFIG);
			tmp.StoreLine(newLine);
			tmp.Close();
			return;
		}
		Godot.Collections.Dictionary<string,Variant> saveDict = new();
		Godot.FileAccess config = Godot.FileAccess.Open(CONFIG_PATH,Godot.FileAccess.ModeFlags.Write);
		saveDict["version"] = version;
		string aggregator = "";
		foreach(string path in SaveLocations)
		{
			if (path.Length < 3)continue;
			aggregator += $"{path}*";
		}
		saveDict["savehistorylocations"] = aggregator;
		if (OS.IsDebugBuild()) GD.Print(saveDict.ToString());
		string newLineTwo = Json.Stringify(saveDict);
		config.StoreLine(newLineTwo);
		config.Close();
	}

	public void LoadConfig()
	{
		Godot.FileAccess config = Godot.FileAccess.Open(CONFIG_PATH,Godot.FileAccess.ModeFlags.Read);
		if (config != null)
		{
			var data = Json.ParseString(config.GetAsText());
			currentConfig = new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary)data);
			if (currentConfig["version"].AsString() != DEFAULT_CONFIG["version"].AsString()) 
			{
				version = DEFAULT_CONFIG["version"].AsString();
				UpgradeVersion();
			}
			else version = currentConfig["version"].AsString();
			string saveHistoryPaths = currentConfig["savehistorylocations"].AsString();
			foreach (string location in saveHistoryPaths.Trim().Split('*'))
			{
				if (DirAccess.DirExistsAbsolute(location) && location.Length > 3)
				{
					GD.Print($"Adding path to {location}");
					SaveLocations.Add(location);
				}
				else 
				{
					GD.Print($"Invalid path to {location} will be removed");
				}
			}
			
		}
		config.Close();
	}

	private Godot.Collections.Dictionary<string,Variant> currentConfig;

	private readonly Godot.Collections.Dictionary<string,Variant> DEFAULT_CONFIG = new Godot.Collections.Dictionary<string, Variant>()
	{
		{"savehistorylocations",""},
		{"version","0.0.1"}
	};
}
