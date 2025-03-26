using Godot;
using System;
using System.Collections.Generic;

public partial class Hitboxtest : Node3D
{
    [Export] public Godot.Collections.Dictionary<Health,Boxes> hitBoxes;
    
    public enum Boxes
    {
        Head,
        ArmR,
        ArmL,
        Torso,
        LegR,
        LegL          
    }
    public override void _Ready()
    {
        foreach (var healthComponent in hitBoxes.Keys)
        {
            healthComponent.hit += (hp)=> { hit(hp, hitBoxes[healthComponent]); };
        }
    }
    public void hit(float hp,Boxes type)
    {
        GD.Print(hp,type);

    }
}
