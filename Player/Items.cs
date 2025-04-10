using Godot;
using Godot.Collections;
using System;

public partial class Items : Control
{
    [Export] PlayerItemsManager pim;
    [Export] TextureRect[] slots;
    [Export] Godot.Collections.Dictionary<Skull.COLOR, Texture2D> skullColors;
    
    public override void _Ready()
    {
        pim.Updated += update;
        Position += new Vector2(200, 0);
    }
    public override void _PhysicsProcess(double delta)
    {
        if (Visible)
        {
            Vector2 pos = Position;
            pos.X = Mathf.Lerp(pos.X, 0, 0.5f);
            Position = pos;
        }
    }
    public void update()
    {

        Visible = true;
        
        if (pim.items.ContainsKey(Skull.COLOR.SKULLRED.ToString())){
            slots[0].Texture = skullColors[Skull.COLOR.SKULLRED];
        }
        if (pim.items.ContainsKey(Skull.COLOR.SKULLBLUE.ToString()))
        {
            slots[1].Texture = skullColors[Skull.COLOR.SKULLBLUE];
        }
        if (pim.items.ContainsKey(Skull.COLOR.SKULLGREEN.ToString()))
        {
            slots[2].Texture = skullColors[Skull.COLOR.SKULLGREEN];
        }
    }

}
