using Godot;
using System;
[GlobalClass]
public partial class Hitbox : Area3D
{
    [Signal] public delegate void hitEventHandler();
    [Signal] public delegate void destroyEventHandler(string _name);

    [Export] public string name;

    [Export] Health bodyHealth;

    [Export] float damageToWholeBody = 0;

    [Export] Node3D bodyPart;
    [Export] bool destroyBodyPart = true;

    [Export] PackedScene hitParticles;
    [Export] PackedScene destroyParticles;

    private Health bodyPartHealth;
    public override void _Ready()
    {
        bodyPartHealth = GetNode<Health>("Health");
        bodyPartHealth.died += destroyed;
        bodyPartHealth.hit += partHit;
    }
    public void partHit(float damage)
    {
        bodyPartHealth.health -= damage;
        
    }
    public void destroyed()
    {
        EmitSignal(SignalName.destroy,name);
        bodyPart.QueueFree();
    }


}
