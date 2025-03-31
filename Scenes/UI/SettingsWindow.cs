using Godot;
using System;

public partial class SettingsWindow : Control
{
    [Export] Button save;
    [Export] Button exit;
    [Export] CheckButton ssao;
    [Export] CheckButton decals;
    [Export] HSlider volume;
    [Export] HSlider brightness;
    SettingsRes settings;
    [Export] PauseMenu pauseMenu;

    public override void _Ready()
    {
        save.Pressed += uploadSettings;
        loadSettings();
        updateSettingsUI();
        exit.Pressed += exitT;
        brightness.Changed += uploadSettings;
    }
    public void exitT()
    {
        Visible = false;
        pauseMenu.Visible = true;
        pauseMenu.state = PauseMenu.States.Paused;
    }
    public void loadSettings()
    {
        settings = Settings.Instance.SettingsRes;
    }
    void updateSettingsUI()
    {
        ssao.ButtonPressed = settings.ssao;
        decals.ButtonPressed = settings.decals;
        volume.Value = settings.volume;
        brightness.Value = settings.brightness;
        
    }
    void uploadSettings()
    {
        settings.volume = volume.Value;
        settings.brightness = brightness.Value;
        settings.ssao = ssao.ButtonPressed;
        settings.decals = decals.ButtonPressed;

        Settings.Instance.SettingsRes = settings;
        Settings.Instance.saveSettingsRes();
    }

    
}
