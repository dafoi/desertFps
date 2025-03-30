using Godot;
using System;
[GlobalClass]
public partial class DiedSound : AudioStreamPlayer3D
{
    [Export] RandomSound randomSound;
    [Export] Health[] healths;
    public override void _Ready()
    {
        foreach (var health in healths)
        {
            health.died += () =>
            {
                Stream = randomSound.getRandom();
                Play();
            };
        }
    }
}
