using Godot;
using System;

public partial class BallProjectile : Node3D
{
    [Export] Area3D attackArea;
    [Export] playerResource playerRes;
    public Vector3 direction = Vector3.Forward;
    [Export] public float speed = 10;
    [Export] float damage = 10;
    public override void _Ready()
    {
        attackArea.BodyEntered += bodyEntered;
    }
    public override void _PhysicsProcess(double delta)
    {
        //Translate(direction * speed * (float)delta);
        Vector3 pos = GlobalPosition;
        pos += direction * speed * (float)delta;
        GlobalPosition = pos;
    }
    public void bodyEntered(Node body)
    {
        if (body is player)
        {
            playerRes.hp -= (int)damage;
        }
        QueueFree();
    }
}
