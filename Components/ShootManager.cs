using Godot;
using System;
[GlobalClass]
public partial class ShootManager : Node
{
    [Export] Camera3D camera;
    public void shoot()
    {
        PhysicsDirectSpaceState3D spaceState = camera.GetWorld3D().DirectSpaceState;
        var screenCenter = GetViewport().GetVisibleRect().Size / 2;
        var origin = camera.ProjectRayOrigin(screenCenter);
        var end = origin + camera.ProjectLocalRayNormal(screenCenter) * 1000;
        var query = PhysicsRayQueryParameters3D.Create(origin, end);
        query.CollideWithBodies = true;
        query.CollideWithAreas = true;
        var result = spaceState.IntersectRay(query);
        if(result != null)
        {

        }
        GD.Print(result);
            
    }
    public override void _Ready()
    {
        shoot();
    }
}
