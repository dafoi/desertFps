using Godot;
using System;

public partial class again : Button
{
    [Export] String scene;
    [Export] Wep[] weapons;
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        Pressed += reload;
    }
    public void reload()
    {
        foreach (var weapon in weapons) { 
            weapon.resetAmmo();
        }

        GetTree().ChangeSceneToFile(scene);
        
    }
}
