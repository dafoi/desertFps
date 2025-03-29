using Godot;
using System;
[GlobalClass]
public partial class ShootManager : Node
{
	[Signal] public delegate void didHitEventHandler();

	[Export] Camera3D camera;
	[Export] RayCast3D rayCast;
	[Export] PackedScene decal;
	public void shoot(float damage,int amountOfBullets, float accuracy)
	{
		
		
		for (int i = 0; i < amountOfBullets; i++)
		{
			
			rayCast.ForceRaycastUpdate();
			
			rayCast.Rotation = Vector3.Zero;

			float offset_X = (float)(new Random().NextDouble() - 0.5) * accuracy * 0.1f;
			float offset_Y = (float)(new Random().NextDouble() - 0.5) * accuracy * 0.1f;
			
			rayCast.RotateX(offset_X);
			rayCast.RotateY(offset_Y);
			

			if (rayCast.IsColliding())
			{
				Node3D body = rayCast.GetCollider() as Node3D;
				spawnDecal(rayCast.GetCollisionPoint(),rayCast.GetCollisionNormal());
				
				if (body.HasNode("Health"))
				{	
					body.GetNode<Health>("Health").getDamage(damage);
					EmitSignal(SignalName.didHit);
				}
			}
			
		}		
	}
	public void spawnDecal(Vector3 collisionPoint, Vector3 normal)
	{
		Node3D dec = decal.Instantiate() as Node3D;
		dec.GlobalPosition = collisionPoint;
		GetTree().GetFirstNodeInGroup("Decals").AddChild(dec);
		dec.LookAt(collisionPoint + normal, Vector3.Up);
		dec.Transform = dec.Transform.RotatedLocal(Vector3.Right, 3.14f / 2.0f);
	}
	public override void _Ready()
	{
		
	}
	
}
