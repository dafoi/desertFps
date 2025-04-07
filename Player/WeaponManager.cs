using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class WeaponManager : Node3D
{
	[Signal] public delegate void playerShotEventHandler();
	[Export] player gamer;
	[Export] ShootManager shootManager;
	[Export] Timer reloadTimer;
	[Export] AnimationPlayer player;
	[Export] AnimationPlayer rig_player;
	[Export] Godot.Collections.Dictionary<string, Wep> wep_resources;
	[Export] Godot.Collections.Dictionary<string, string> wep_paths;
	[Export] string[] wep_have;
	
	[Export] DynamicCrosshair crosshair;
	[ExportGroup("Labels")]
	[Export] Label label;
	[Export] Label ui_ammo;
	[ExportGroup("Sound player's")]
	[Export] AudioStreamPlayer wep_activate_sound; 
	[Export] AudioStreamPlayer wep_shoot_sound;
	
	public Wep wep_current;
	public int id = 0;
	public string next_weapon = "";
	public string weapon = "";
	public int last_id = 0;
	public bool can_switch = true;
	public bool isShooting = false;
	public bool isReloading = false;

	public override void _Process(double delta)
	{
		if (isReloading) can_switch = false;

		label.Text = "fps:" + Engine.GetFramesPerSecond() + " toggle v:global illumination, b:voletric fog";//$"{id} , {wep_current.name} , {can_switch} , next: {next_weapon} , res_ammo : {wep_current.reserve_ammo}";
		
		if (can_switch)
		{
			if (Input.IsActionJustPressed("invnext"))
			{
				if (id + 1 <= wep_have.Length - 1)
				{
					id++;
				}
				else
				{
					id = 0;
				}
				next_weapon = wep_have[id];
				can_switch = false;

				exit();

			}
			if (Input.IsActionJustPressed("invprev"))
			{
				if (id-- <= 0)
				{
					id = wep_have.Length - 1;
				}
				next_weapon = wep_have[id];
				can_switch = false;
				exit();

			}
			if (Input.IsActionJustPressed("reload") && wep_current.ammo != wep_current.mag_size && wep_current.reserve_ammo > 0) 
			{
				wep_current.reload();
				isReloading = true;
				can_switch = false;
				//reloadTimer.Start();
				rig_player.Play(wep_current.r_Reload_anim);
				player.Play(wep_current.Reload_anim);
				rig_player.SpeedScale = wep_current.Reload_speed;
				player.SpeedScale = wep_current.Reload_speed;
			}
		}
		last_id = id;

		if (Input.IsActionPressed("shoot"))
		{
			if (can_switch && !isShooting && wep_current.ammo > 0)
			{
				if(wep_current.s_Shoot != null)
				{
					wep_shoot_sound.Stream = wep_current.s_Shoot;
				}
				wep_shoot_sound.Play();
				player.SpeedScale = wep_current.Shoot_speed;
				player.Play(wep_current.Shoot_anim);
				rig_player.SpeedScale = wep_current.Shoot_speed;
				rig_player.Play(wep_current.r_Shoot_anim);
				isShooting = true;
				can_switch = false;
				wep_current.ammo--;
				shootManager.shoot(wep_current.damage, wep_current.amountOfBullets, wep_current.accuracy);
				EmitSignal(SignalName.playerShot);

			}
		}
		ui_ammo.Text = wep_current.ammo + "/" + wep_current.reserve_ammo;
	}
	public override void _Ready()
	{
		Initialize();
	}

	void Initialize()
	{
		wep_current = wep_resources[wep_have[id]];
		player.RootNode = wep_paths[wep_have[id]];
		player.Queue(wep_current.Activate_anim);
		weapon = wep_current.name;
		crosshair.target_size = wep_current.crosshairSize;


	}
	void enter()
	{
		updatePath();
		if(wep_current.s_Activate != null)
		{
			wep_activate_sound.Stream = wep_current.s_Activate;
		}
		player.SpeedScale = wep_current.Activate_speed;
		player.Queue(wep_current.Activate_anim);
		rig_player.SpeedScale = wep_current.Activate_speed;
		rig_player.Play(wep_current.r_Activate_anim);
		next_weapon = "";
		if(crosshair != null)
		{
			crosshair.target_size = wep_current.crosshairSize;
		}
	}
	void exit()
	{
		if(wep_current.s_Deactivate != null)
		{
			//wep_activate_sound.Stream = wep_current.s_Deactivate;
		}
		player.SpeedScale = wep_current.Deactivate_speed;
		player.Play(wep_current.Deactivate_anim);
		rig_player.SpeedScale = wep_current.Deactivate_speed;
		rig_player.Play(wep_current.r_Deactivate_anim);


	}
	void changeWeapon(string wep_name)
	{
		wep_current = wep_resources[wep_name];

		enter();
	}
	public void finished(string name)
	{
		if (name == wep_current.Deactivate_anim)
		{
			changeWeapon(next_weapon);
		}

		if (name == wep_current.Activate_anim)
		{
			can_switch = true;
			player.SpeedScale = 1f;
			rig_player.SpeedScale = 1f;
		}
		if (name == wep_current.Shoot_anim)
		{
			player.SpeedScale = 1f;
			rig_player.SpeedScale = 1f;
			can_switch = true;
			isShooting = false;

		}
		if(name == wep_current.Reload_anim)
		{
            can_switch = true;
            isReloading = false;
            player.SpeedScale = 1f;
            rig_player.SpeedScale = 1f;
        }
	}
	void updatePath()
	{
		player.RootNode = wep_paths[wep_have[id]];
	}
	public void onreloadTimer()
	{
		can_switch = true;
		isReloading = false;
	}

}
