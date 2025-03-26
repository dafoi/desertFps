using Godot;
using System;

public partial class healthBar : ProgressBar
{

    public float targetValue;
    public float speed = 0.5f;
    public override void _PhysicsProcess(double delta)
    {
        Value = Mathf.Lerp(Value, targetValue, speed);
    }
}
