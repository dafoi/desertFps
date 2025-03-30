using Godot;
using System;
[GlobalClass]
public partial class RandomSound : Resource
{
    [Export] AudioStream[] sounds;
    public AudioStream getRandom()
    {
        int length = sounds.Length;
        int id = new Random().Next(0, length);
        return sounds[id];
    }
}
