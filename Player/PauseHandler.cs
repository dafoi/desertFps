using Godot;
using System;

public partial class PauseHandler : Node
{
    
    public override void _Input(InputEvent @event)
    {
        if(Input.IsActionJustPressed("ui_cancel"))
        {
            bool paused = GetTree().Paused;
            if (paused)
            {
                GetTree().Paused = false;
                Input.MouseMode = Input.MouseModeEnum.Captured;
            }
            else
            {
                GetTree().Paused = true;
                Input.MouseMode = Input.MouseModeEnum.Visible;
            }
            
        }
    }
}
