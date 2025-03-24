using Godot;
using System;

public partial class Playground : Node3D
{
    [Export] public bool enemies_run_at_start = true;
    public override void _Ready()
    {
        if (enemies_run_at_start)
        {
        var enemies = GetTree().GetNodesInGroup("Enemy");
        
        foreach(Enemy enemy in enemies)
        {
            enemy.st = Enemy.S.Running;
        }

        }
    }
}
