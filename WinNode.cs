using Godot;
using System;

public partial class WinNode : Node
{
    [Export] EnemyController ec;
    public override void _Ready()
    {
        base._Ready();
        ec.AllEnemyIsDead += won;
    }
    public void won()
    {
        GetTree().ChangeSceneToFile("uid://27021rkyrv3e");
    }
}
