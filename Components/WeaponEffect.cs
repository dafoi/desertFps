using Godot;
using System;

public partial class WeaponEffect : Node3D
{
    [Export] public WeaponManager wm;
    public override void _Ready()
    {
        if(wm != null)
        {
            wm.playerShot += shot;
        }
    }
    public virtual void shot()
    {

    }
    public virtual void reload()
    {
        throw new NotImplementedException();
    }
}
