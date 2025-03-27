using Godot;
using System;

public partial class ShootRayCast : RayCast3D
{
	[Export] Godot.Collections.Array<ShootRayCast> rays;
	public void shoot(float damage)
	{
		if (IsColliding())
		{
			Node3D collider = GetCollider() as Node3D;
			if (collider.HasNode("Health"))
			{
				collider.GetNode<Health>("Health").getDamage(damage);
			}
		}
		if (rays != null)
		{
			foreach (var ray in rays)
			{
				ray.shoot(damage);
			}
		}



	}
}
