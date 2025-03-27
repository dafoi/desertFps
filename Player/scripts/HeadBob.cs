using Godot;
using System;

public partial class HeadBob : Node3D
{
	public bool bob = false;
	public bool tilt = true;
	public Vector2 input = Vector2.Zero;
	private float temp;
	private float tilt_x = 0f;
	private float tilt_y = 0f;
	private Vector3 rotation;
	[Export] float amplitude = .25f;
	[Export] float freq = .5f;
	[Export] float tilt_y_speed = 0.1f;
	[Export] float tilt_y_amplitude = 1f;
	[Export] float tilt_x_speed = 0.1f;
	[Export] float tilt_x_amplitude = 1f;
	public override void _PhysicsProcess(double delta)
	{
		if(bob) bbob(delta);
		if (tilt) ttilt();

		
	}
	void bbob(double delta)
	{
	   temp += (float)delta;
		Position = new Vector3(0, Mathf.Sin(temp * freq) * amplitude+ 0.472f , 0) ;
	}
	void ttilt()
	{
		rotation = RotationDegrees;
		//TILT X
		if (input.X != 0)
		{
			tilt_x = Mathf.Lerp(tilt_x, -tilt_x_amplitude * input.X, tilt_x_speed);
		}
		else
		{
			tilt_x = Mathf.Lerp(tilt_x, 0, tilt_x_speed);
		}
		//TILT Y
		if (input.Y != 0)
		{
			tilt_y = Mathf.Lerp(tilt_y, tilt_y_amplitude * input.Y, tilt_y_speed);
		}
		else
		{
			tilt_y = Mathf.Lerp(tilt_y, 0, tilt_y_speed);
		}

		rotation.Z = tilt_x;
		rotation.X = tilt_y;
		RotationDegrees = rotation;
	}
}
