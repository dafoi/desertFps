using Godot;
using System;

[GlobalClass]
public partial class Wep : Resource
{

    [Export] public string name;
    [Export] public int mag_size;
    [Export] public int reserve_size;
    [Export] public int reserve_ammo;
    [Export] public int ammo;
    [Export] int default_reserve_ammo;
    [Export] int default_ammo;
    [ExportGroup("Animation Names")]
    [ExportCategory("Weapon")]
    [Export] public string Shoot_anim;
    [Export] public string Reload_anim;
    [Export] public string Activate_anim;
    [Export] public string Deactivate_anim;
    [ExportCategory("Rig")]
    [Export] public string r_Shoot_anim;
    [Export] public string r_Reload_anim;
    [Export] public string r_Activate_anim;
    [Export] public string r_Deactivate_anim;
    [ExportGroup("Sounds")]
    [Export] public AudioStream s_Activate;
    [Export] public AudioStream s_Deactivate;
    [Export] public AudioStream s_Shoot;
    [Export] public AudioStream s_Reload;
    [ExportGroup("Speed")]
    [Export] public float Activate_speed = 1f;
    [Export] public float Deactivate_speed = 1f;
    [Export] public float Shoot_speed = 1f;
    [ExportGroup("ShootingParams")]
    [Export] public float damage = 10f;
    [Export] public int amountOfBullets = 1;
    [Export] public float accuracy = 1.0f;
    [Export] public int crosshairSize = 12;
    
    public void reload()
    {
        int ammoneed = mag_size - ammo;
        if(reserve_ammo >= ammoneed)
        {
            reserve_ammo-= ammoneed;
            ammo += ammoneed;
            
        }
        else {
            ammo += reserve_ammo;
            reserve_ammo = 0;
        }
    }
    public void resetAmmo()
    {
        ammo = default_ammo;
        reserve_ammo = default_reserve_ammo;
    }



}
