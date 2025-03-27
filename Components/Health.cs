using Godot;
using System;
[GlobalClass]
public partial class Health : Node
{
	[Signal]
	public delegate void hitEventHandler(float _damage);
	[Signal]
	public delegate void changedEventHandler(float _health);
	[Signal]
	public delegate void diedEventHandler();

	[Export] public bool can_die = true;
	[Export] public bool hitAble = true;

	[Export] public string name = "";

	[Export]
	public float health;
	[Export]
	public float maxHealth = 100;
	[Export]
	public float startHealth = 100;
	[Export]
	public bool restoreHealthOnStart;
	[Export]
	public bool deleteParentOnDeath = true;
	[Export] Node[] deleteNodeOnDeath;

	[ExportGroup("Additional")]
	[Export] PackedScene bloodParticles;
	[Export] Node3D bloodPosition;
	[Export] AudioStreamPlayer3D hitsound;
	public override void _Ready()
	{
		if (restoreHealthOnStart)
			health = startHealth;
	}
	public void getDamage(float amount)
	{
		if(!hitAble)
		{
			return;
		}

		health -= amount;


		EmitSignal(SignalName.changed, health);
		EmitSignal(SignalName.hit, amount);

		checkFX();
		
		if (health < 0 && can_die)
		{
			EmitSignal(SignalName.died);

			if(deleteNodeOnDeath != null) 
			foreach(var i in deleteNodeOnDeath )
			{i.QueueFree();
			};

			if (deleteParentOnDeath)
			{
				GetParent().QueueFree();

			}
		}
	}
	public void checkFX()
	{
		if (hitsound != null) hitsound.Play();

		if (bloodParticles != null && bloodPosition != null) spawnBlood();
	}
	public void restore()
	{
		health = maxHealth;
		EmitSignal(SignalName.changed, health);
	}
	public void spawnBlood()
	{
		var blood = bloodParticles.Instantiate() as Node3D;
		Random rand = new();
		Vector3 offset = new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble());
		offset /= 3;
		blood.GlobalPosition = bloodPosition.GlobalPosition + offset;
		GetTree().GetFirstNodeInGroup("Decals").AddChild(blood);
	}

}
