using Godot;
using System;
[GlobalClass]
public partial class EnemyController : Node
{
    [Export] EnemySpawnSpot[] ess;
    [Signal] public delegate void EnemyDiedEventHandler();
    [Signal] public delegate void AllEnemyIsDeadEventHandler();
    [Export] Trigger trigger;
    public int enemies = 0;
    public override void _Ready()
    {
        foreach (EnemySpawnSpot sp in ess) 
        {
            sp.enemySpawned+=addEnemy;
        }
    }
    public void addEnemy(Node3D enemy)
    {
        GD.Print("added enemy");
        if (enemy.HasNode("Health")){
            enemy.GetNode<Health>("Health").died += enemyDied;
        }
        enemies++;
    }
    public void enemyDied()
    {
        GD.Print("enemies died");
        enemies--;
        EmitSignal(SignalName.EnemyDied);
        if(enemies == 0)
        {
            EmitSignal(SignalName.AllEnemyIsDead);
            
            trigger.msg = "alldead";
            trigger.emit();
        }
    }
}
