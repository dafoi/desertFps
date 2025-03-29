using Godot;
using System;

public partial class Crosshair : Control
{
    [Export] ColorRect[] Horizontal;
    [Export] ColorRect[] Vertical;
    [Export] float width;
    [Export] float length;

    [Export] ColorRect center;
    public float targetSize = 12;
    public float speed = 0.5f;
    public override void _Ready()
    {
        foreach(ColorRect cr in Horizontal)
        {
            cr.CustomMinimumSize = new Vector2(length,width);
        }
        foreach (ColorRect cr in Vertical)
        {
            cr.CustomMinimumSize = new Vector2(width, length);
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        Vector2 size = center.CustomMinimumSize;
        size.X = Mathf.Lerp(size.X, targetSize, speed);
        size.Y = size.X;
        center
            .CustomMinimumSize = size;
  
        
    }
}
