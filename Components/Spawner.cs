using Godot;
using System;
[GlobalClass]
public partial class Spawner : Node3D
{
    [Export] bool Enabled = true;
    [Export] float interval = 3.0f;
    [Export] PackedScene node;
    [Export] Timer timer;
    public override void _Ready()
    {
        timer.Autostart = Enabled;
        timer.WaitTime = interval;

        timer.Timeout += spawn;
        timer.Start();
        
    }
    public void spawn()
    {
        var instance = node.Instantiate() as Node3D;
        AddChild(instance);
        instance.GlobalPosition = GlobalPosition;
    }

}
