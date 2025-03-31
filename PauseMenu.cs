using Godot;
using System;

public partial class PauseMenu : Panel
{
    [Export] Button continueButton;
    [Export] Button settingsButton;
    [Export] Button exitButton;
    [Export] Control settingsMenu;
    public enum States
    {
        Playing,
        Paused,
        Settings
    }
    public States state = States.Playing;

    public override void _Ready()
    {
        continueButton.Pressed += onContinue;
        settingsButton.Pressed += onSettings;
        exitButton.Pressed += onExit;

    }
    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            switch (state)
            {
                case States.Playing:

                    Visible = true;
                    Input.MouseMode = Input.MouseModeEnum.Visible;
                    GetTree().Paused = true;
                    state = States.Paused;

                    break;
                case States.Paused:

                    Visible = false;
                    Input.MouseMode = Input.MouseModeEnum.Captured;
                    GetTree().Paused = false;
                    state = States.Playing;
                    break;
                case States.Settings:
                    settingsMenu.Visible = false;
                    Visible = true;
                    state = States.Paused;


                    break;
            }
        }
    }
    public void onContinue()
    {

        Visible = false;
        GetTree().Paused = false;
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }
    public void onSettings()
    {
        state = States.Settings;
        Visible = false;
        settingsMenu.Visible = true;
    }
    public void onExit()
    {

        GetTree().Quit();
    }
}
