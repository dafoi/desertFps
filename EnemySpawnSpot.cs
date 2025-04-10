using Godot;
using System;

public partial class EnemySpawnSpot : Marker3D
{
    [Signal] public delegate void enemySpawnedEventHandler(Node3D enemy);
    [Export] AudioStreamPlayer3D asp;
    [Export] Node[] triggers;
    [Export] PackedScene entity;
    [Export] string signal;
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
        if (used && OneShot) return;
        
        if(msg == signal)
        {
            used = true;
            var instance = entity.Instantiate() as Node3D;
            instance.GlobalPosition = GlobalPosition;
            GetTree().GetFirstNodeInGroup("Enemies").AddChild(instance);
            EmitSignal(SignalName.enemySpawned,instance);
            asp.Play();
        }
    }
}
