using Godot;
using System;

public partial class Settings : Control
{
    [Export] Button crosshair_button;
    [Export] ColorPicker crosshair_color;
    public override void _Ready()
    {
        crosshair_button.Pressed += on_crosshair_button_pressed;
    }
    public void on_crosshair_button_pressed()
    {
        crosshair_color.Visible = !crosshair_color.Visible;
    }
}
