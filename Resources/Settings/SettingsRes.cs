using Godot;
using System;
[GlobalClass]
public partial class SettingsRes : Resource
{
    [Export] public bool ssao = true;
    [Export] public double brightness = 1.0f;
    [Export] public double volume = 1.0f;
    [Export] public bool decals = true;

}
