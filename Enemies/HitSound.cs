using Godot;
using System;
[GlobalClass]
public partial class HitSound : AudioStreamPlayer3D
{
    [Export] RandomSound randomSound;
    [Export] Health[] healths;
    public override void _Ready()
    {
        foreach (var health in healths)
        {
            health.hit += (d) =>
                    {
                        Stream = randomSound.getRandom();
                        Play();
                    };
        }

    }
}
