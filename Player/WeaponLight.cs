using Godot;
using System;

public partial class WeaponLight : WeaponEffect
{
    [Export] float time = 0.1f;

    public override void shot()
    {
        Visible = true;
        GetTree().CreateTimer(time).Timeout += onDisable;
    }
    void onDisable()
    {
        
        Visible = false;
    }

}
