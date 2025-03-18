using Godot;
using System;

public partial class CheckForObstacles : RayCast3D
{
    [Export] playerResource playerResource;
    public override void _PhysicsProcess(double delta)
    {

    }
    public bool isThereObstacle()
    {
        TargetPosition = playerResource.Player.GlobalPosition;
        if (IsColliding())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
