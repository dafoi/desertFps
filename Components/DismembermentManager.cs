using Godot;
using System;
[GlobalClass]
public partial class DismembermentManager : Node
{
    [Export] Health mainHealth;
    [Export] Godot.Collections.Dictionary<Health, Members> parts;
    [Export] Godot.Collections.Dictionary<Members, int> partsDamageMultiplier;
    public enum Members
    {
        Head,
        ArmL,
        ArmR,
        LegL,
        LegR
    }

    public override void _Ready()
    {
        foreach (Health member in parts.Keys)
        {
            member.hit += (dmg) =>
            {
                partHit(dmg, parts[member]);
            };
            member.died += () =>
            {
                partDied(parts[member]);
            };
        }
    }
    public void partDied(Members type)
    {
        switch (type)
        {
            case Members.Head:
                mainHealth.getDamage(300);
                break;
        }
        GD.Print("part died", type);
    }
    public void partHit(float damage, Members type)
    {
        GD.Print("Parthit", type);
        
        mainHealth.getDamage(damage);

    }


}
