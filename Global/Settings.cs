using Godot;
using System;

public partial class Settings : Node
{
    private static Settings instance;
    public static Settings Instance => instance;

    private SettingsRes settingsRes;
    public SettingsRes SettingsRes { get { return settingsRes; } set { settingsRes = value; } }
    [Signal] public delegate void SettingsChangedEventHandler(SettingsRes res);
    public static string name = "user://settings.tres";
    public override void _Ready()
    {
        loadSettingsRes();
    }
    public void loadSettingsRes()
    {
        if (ResourceLoader.Exists(name))
        {
            settingsRes = ResourceLoader.Load(name) as SettingsRes;
            EmitSignal(SignalName.SettingsChanged, settingsRes);
            float db = (float)settingsRes.volume;
            AudioServer.SetBusVolumeDb(0, db);
        }
        else
        {
            settingsRes = new SettingsRes();
            saveSettingsRes();
        }
        
    }
    public void saveSettingsRes()
    {
        ResourceSaver.Save(settingsRes, name);
        EmitSignal(SignalName.SettingsChanged,settingsRes);
    }
   
    public override void _EnterTree()
    {
        if (instance != null)
        {
            this.QueueFree(); // The Singletone is already loaded, kill this instance
        }
        instance = this;

    }
}
