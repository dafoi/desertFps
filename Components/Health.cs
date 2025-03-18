using Godot;
using System;
[GlobalClass]
public partial class Health : Node
{
    [Signal]
    public delegate void hitEventHandler();
    [Signal]
    public delegate void changedEventHandler();
    [Signal]
    public delegate void diedEventHandler();
    [Export]
    public float health;
    [Export]
    public float maxHealth = 100;
    [Export]
    public float startHealth = 100;
    [Export]
    public bool restoreHealthOnStart;
    [Export]
    public bool deleteParentIfDead = true;
    [ExportGroup("Additional")]
    [Export] PackedScene bloodParticles;
    [Export] Node3D bloodPosition;
    [Export] AudioStreamPlayer3D hitsound;
    public override void _Ready()
    {
        if(restoreHealthOnStart)
        health = startHealth;
    }
    public void getDamage(float amount)
    {
        health -= amount;
        EmitSignal(SignalName.hit);
        if (hitsound != null)
        {
            hitsound.Play();
        }
        if(bloodParticles != null && bloodPosition != null)
        {
            var blood = bloodParticles.Instantiate() as Node3D;
            Random rand = new();
            Vector3 offset = new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble());
            offset /= 3;
            blood.GlobalPosition = bloodPosition.GlobalPosition + offset;
            GetParent().GetParent().AddChild(blood);
        }
        if(health < 0)
        {
            EmitSignal(SignalName.died);
            GetParent().QueueFree();
        }
    }
    public void restore()
    {
        health = maxHealth;
    }

}
