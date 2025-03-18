using Godot;
using System;

public partial class WeaponSway : Node3D
{
    [Export] bool enabled = true;

    [Export] player Player;

    //[Export] float y_speed = 2f;

    [Export] float y_amplitude =.02f;
    [Export] float x_speed =9f;
    [Export] float x_amplitude =.1f;

    private float time = 0f;
    
    public override void _PhysicsProcess(double delta)
    {
        Vector3 pos = Position;
        if(enabled && Player.weapon_sway)
        {
            time += (float)delta;
            pos.Y = Mathf.Sin(-time * x_speed * 2f) * y_amplitude;
            pos.X = Mathf.Cos(time * x_speed) * x_amplitude;
            Position = pos;
        }

    }

}
