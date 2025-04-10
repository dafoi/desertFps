using Godot;
using System;

public partial class PlayerItemsManager : Node
{
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
        return false;
    }
    public void addItem(string item_name, int amount) 
    {
        items.Add(item_name, amount);
    }
}
