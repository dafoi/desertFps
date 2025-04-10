using Godot;
using System;

public partial class Skull : Node3D
{
    [Export] COLOR color;
    [Export] AudioStreamPlayer asp;
    public enum COLOR
    {
        SKULLRED,
        SKULLBLUE,
        SKULLGREEN
    }
    public override void _Ready()
    {
        GetNode<Area3D>("Area3D").BodyEntered += entered;
    }
    public void entered(Node3D body)
    {
        if(body is player)
        {
            body.GetNode<PlayerItemsManager>("PlayerItemsManager").addItem(color.ToString(), 1);
            GD.Print(color.ToString());
            asp.Play();
            Visible = false;
            asp.Finished += QueueFree;
            
        }
            
    }
}
