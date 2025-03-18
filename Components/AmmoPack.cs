using Godot;
using System;
[GlobalClass]
public partial class AmmoPack : Area3D
{
    [Export] Wep weapon_resource;
    [Export] Node3D model;
    [Export] float rotation_speed;
    [Export] int amount_of_ammo;
    [Export] PackedScene soundScene;
    public override void _Ready()
    {
        BodyEntered += onPickup;
    }
    public void onPickup(Node3D body)
    {
        if(body is player)
        {
            if(weapon_resource.reserve_ammo < weapon_resource.reserve_size)
            {
            weapon_resource.reserve_ammo += amount_of_ammo;
            if(weapon_resource.reserve_ammo > weapon_resource.reserve_size)
            {
                weapon_resource.reserve_ammo = weapon_resource.reserve_size;
            }
            if (soundScene != null)
            {
                model.GetParent().AddChild(soundScene.Instantiate());
            }
            model.QueueFree();

            }
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        if(model != null)
        {
            model.RotateY(rotation_speed);
        }
    }
}
