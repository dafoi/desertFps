using Godot;
using System;

public partial class PuHp : Area3D
{
    [Export] Node3D node;
    [Export] playerResource pr;
    [Signal] public delegate void PickedUpEventHandler();
    [Export] int hp;
    public override void _Ready()
    {
        BodyEntered += enter;
    }
    public override void _PhysicsProcess(double delta)
    {
        
        node.RotateY(0.01f);
        
    }
    public void enter(Node3D body)
    {
        GD.Print("body enterd;");
        if(body is player p)
        {
            p.playerResource.hp += hp;
            EmitSignal(SignalName.PickedUp);
            QueueFree();
        }

    }
}
