using Godot;
using System;

public partial class MouseHandler : Node
{
    public bool captured = true;
    public override void _PhysicsProcess(double delta)
    {
        if(Input.IsActionJustPressed("quit"))
        {
            if(captured)
            {
                Input.MouseMode = Input.MouseModeEnum.Visible;
            }
            else
            {
                Input.MouseMode = Input.MouseModeEnum.Captured;
            }
            captured = !captured;
        }
    }
}
