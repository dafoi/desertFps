using Godot;
using System;

public partial class MaterialRayCast : RayCast3D
{
    public enum materialEnum
    {
        sand,
        stone
    }

    [Export] AudioStreamPlayer3D audioplayer;
    [Export] player _player;
    public materialEnum material;
    public materialEnum lastMaterial;
    [Export] public Godot.Collections.Array<AudioStream> sounds;
    public override void _PhysicsProcess(double delta)
    {
        if (IsColliding())
        {
            Node3D collider = GetCollider() as Node3D;
            if (collider.IsInGroup("stone")) material = materialEnum.stone;
            if (collider.IsInGroup("sand")) material = materialEnum.sand;

        }
        if (lastMaterial != material)
        {
            audioplayer.Stream = sounds[(int)material];
            audioplayer.Play();
        }
        lastMaterial = material;
        if (_player.footsteps)
        {
            if(audioplayer.StreamPaused)
            {
                audioplayer.StreamPaused = false;

            }
            else if (!audioplayer.Playing)
            {
                audioplayer.Play();
            }
        }
        else
        {
            audioplayer.StreamPaused = true;
        }

    }
    


}
