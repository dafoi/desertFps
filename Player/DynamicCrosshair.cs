using Godot;
using System;

public partial class DynamicCrosshair : CenterContainer
{
    [Export] float dot_radius = 1.0f;
    [Export] int width = 4;
    [Export] Color dot_color = Color.Color8(255, 255, 255);
    [Export] Color hit_color;
    [Export] float hit_color_time = 0.1f;
    Color color;
    [Export] ShootManager shootManager;
    [Export] Line2D[] lines;
    public float target_size = 12f;
    public float speed = 0.5f;
    public override void _Ready()
    {
        color = dot_color;
        shootManager.didHit += changeColor;
    }
    public override void _PhysicsProcess(double delta)
    {
        lines[1].Position = new Vector2(Mathf.Lerp(lines[1].Position.X, target_size, speed), lines[1].Position.Y);
        lines[3].Position = new Vector2(Mathf.Lerp(lines[3].Position.X, -target_size, speed), lines[3].Position.Y);
        lines[0].Position = new Vector2(lines[0].Position.X, Mathf.Lerp(lines[0].Position.Y, -target_size, speed));
        lines[2].Position = new Vector2(lines[2].Position.X, Mathf.Lerp(lines[2].Position.Y, target_size, speed));
        foreach(var i in lines)
        {
            i.Width = width;
            i.DefaultColor = color;
        }
        
        QueueRedraw();

    }

    public override void _Draw()
    {
        DrawCircle(new Vector2(0, 0), dot_radius, color);
    }
    public void changeColor()
    {
        GetTree().CreateTimer(hit_color_time).Timeout += returnColor;
        color = hit_color;
        
    }
    public void returnColor()
    {
        color = dot_color;
    }

}
