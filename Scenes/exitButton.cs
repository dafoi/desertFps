using Godot;
using System;

public partial class exitButton : Button
{
    public override void _Ready()
    {
        Pressed += () => { GetTree().Quit(); };
    }
}
