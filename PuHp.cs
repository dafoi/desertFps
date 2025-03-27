using Godot;
using System;

public partial class PuHp : Node3D
{
    [Export] Node3D node;
    [Export] playerResource pr;
    [Export] int hp;
    [Export] Area3D area;
    public override void _Ready()
    {
        area.BodyEntered += enter;
    }
    public override void _PhysicsProcess(double delta)
    {
        
        node.RotateY(0.01f);
        
    }
    public void enter(Node3D body)
    {
        GD.Print("body enterd;");
        if(body is player)
        {
            pr.hp += hp;
            QueueFree();
        }

    }
}
