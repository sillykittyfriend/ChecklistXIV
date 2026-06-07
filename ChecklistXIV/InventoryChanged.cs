using System;
using System.Collections.Generic;

using Dalamud.Plugin.Services;

using Dalamud.Game.Inventory.InventoryEventArgTypes;




namespace ChecklistXIV;

public class InventoryChanged : IDisposable
{
    private readonly Configuration configuration;
    private readonly IGameInventory inv;
    private readonly IPluginLog log;
    private readonly IChatGui chat;
    private List<(uint Id, String Name)> SelectedItems;
    //Initialize all Services
    public InventoryChanged(Plugin plugin ,IGameInventory inv, IPluginLog log,List<(uint Id, String Name)>  selectedItems, IChatGui chat)
    {
        this.inv = inv;
        this.log = log;
        this.inv.ItemAddedExplicit += OnItemAdded;
        this.SelectedItems = selectedItems;
        configuration = plugin.Configuration;
        this.chat = chat;
    }

    private void OnItemAdded(InventoryItemAddedArgs data)
    {
      //  log.Information($"Inventory event: ({data})");
      
      //When an item hits your inventory loop through all selected items and then remove the index from list if item exists!
        for (int i = SelectedItems.Count - 1; i >= 0; i--)
        {
            if (data.Item.ItemId == SelectedItems[i].Id)
            {
                chat.PrintError($"{SelectedItems[i].Name} HAS BEEN COLLECTED CONGRATSSS!!!!");
                log.Information("Removal Reached");
                SelectedItems.RemoveAt(i);
                configuration.Save();
                
            }
        }
    }
    
    
    public void Dispose()
    {
        this.inv.ItemAddedExplicit -= OnItemAdded;
    }
}
