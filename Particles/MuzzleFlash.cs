using Godot;
using System;

public partial class MuzzleFlash : WeaponEffect
{
    [Export] GpuParticles3D particles3D;
    public override void shot()
    {
        particles3D.Restart();
    }
}
