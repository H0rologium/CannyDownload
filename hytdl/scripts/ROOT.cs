using Godot;
using System;
using System.Collections.Generic;

public partial class ROOT : Node
{
	private List<string> saveLocations;
	private protected readonly string CONFIG_PATH = "user://hytdl.config";

	public List<String> SaveLocations
	{
		get {return saveLocations;}
	}

	public override void _Ready()
	{
		saveLocations = new();
		DisplayServer.WindowSetTitle("Canny Downloader");
		//Read config
		if (!FileAccess.FileExists(CONFIG_PATH)) SaveToConfig(true);
		else {LoadConfig();}
	}

	public void SaveToConfig(bool makeNew = false)
	{
		if (makeNew)
		{
			GD.Print("Creating a new config file, none found");
			FileAccess tmp = FileAccess.Open(CONFIG_PATH,FileAccess.ModeFlags.Write);
			string newLine = Json.Stringify(DEFAULT_CONFIG);
			tmp.StoreLine(newLine);
			tmp.Close();
			return;
		}
		Godot.Collections.Dictionary<string,Variant> saveDict = new();
		FileAccess config;
		if (!FileAccess.FileExists(CONFIG_PATH) && !makeNew) config = FileAccess.Open(CONFIG_PATH,FileAccess.ModeFlags.Write);
		
		string aggregator = "";
		foreach(string path in SaveLocations)
		{
			aggregator += $"{path}*";
		}
		saveDict["savehistorylocations"] = aggregator;
		if (OS.IsDebugBuild()) GD.Print(saveDict.ToString());
		
	}

	public void LoadConfig()
	{
		FileAccess config = FileAccess.Open(CONFIG_PATH,FileAccess.ModeFlags.Read);
		if (config != null)
		{
			var data = Json.ParseString(config.GetAsText());
			currentConfig = new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary)data);
			if (currentConfig["version"].AsString() != DEFAULT_CONFIG["version"].AsString()) currentConfig["version"] = DEFAULT_CONFIG["version"];
			string saveHistoryPaths = currentConfig["savehistorylocations"].AsString();
			foreach (string location in saveHistoryPaths.Trim().Split('*'))
			{
				if (DirAccess.DirExistsAbsolute(location))
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
