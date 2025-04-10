using Godot;
using System;

public partial class PlayerItemsManager : Node
{
    [Signal] public delegate void UpdatedEventHandler();
    public Godot.Collections.Dictionary<string, int> items = new();
    public bool takeItem(string item_name,int amount)
    {
        if (items.ContainsKey(item_name))
        {
            if (items[item_name] <= amount)
            {
                items[item_name] -= amount;
                return true;
            }
        }
        if (items[item_name] == 0) items.Remove(item_name);
        EmitSignal(SignalName.Updated);
        return false;
    }
    public void addItem(string item_name, int amount) 
    {
        items.Add(item_name, amount);
        EmitSignal(SignalName.Updated);
    }
}
