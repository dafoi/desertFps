using Godot;
using System;

public partial class EnemySpawnSpot : Marker3D
{
    [Export] Node[] triggers;
    [Export] PackedScene entity;
    [Export] bool OneShot = true;
    private bool used = false;
    public override void _Ready()
    {
        foreach (var trigger in triggers)
        {
            if (trigger.HasNode("Trigger"))
            {
                Trigger trg = trigger.GetNode<Trigger>("Trigger");
                trg.triggered += spawn;
            }
        }
    }

    public void spawn(string msg)
    {
        if (used) return;

        if(msg == "spawn")
        {
            used = true;
            var instance = entity.Instantiate() as Node3D;
            instance.GlobalPosition = GlobalPosition;
            GetTree().GetFirstNodeInGroup("Enemies").AddChild(instance);
        }
    }
}
