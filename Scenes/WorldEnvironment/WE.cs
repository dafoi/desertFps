using Godot;
using System;

public partial class WE : WorldEnvironment
{
    public override void _Ready()
    {

        Settings.Instance.SettingsChanged += updateSettings;
        Environment.SsaoEnabled = Settings.Instance.SettingsRes.ssao;
        Environment.TonemapExposure = (float)Settings.Instance.SettingsRes.brightness;
        
    }
    public void updateSettings(SettingsRes res)
    {
        Environment.SsaoEnabled = res.ssao;
        Environment.TonemapExposure = (float)res.brightness;
     
    }
    public void update()
    {
        
    }
}
