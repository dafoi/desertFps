using Godot;
using System;

public partial class Playground : Node3D
{
    [Export] string myUID;
    public override void _Ready()
    {
        Global.Instance.setCurrentSceneLevel(myUID);
    }
}
