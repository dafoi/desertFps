using Godot;
using System;

public partial class Crosshair : Control
{
    [Export] ColorRect center;
    public float targetSize = 12;
    public float speed = 0.5f;
    public override void _PhysicsProcess(double delta)
    {
        Vector2 size = center.CustomMinimumSize;
        size.X = Mathf.Lerp(size.X, targetSize, speed);
        size.Y = size.X;
        center
            .CustomMinimumSize = size;
  
        
    }
}
