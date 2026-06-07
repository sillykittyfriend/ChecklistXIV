using Dalamud.Configuration;
using System;
using System.Collections.Generic;

namespace ChecklistXIV;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;
    public List<(uint Id, string Name)> SelectedItems { get; set; } = new();
    

   
    // The below exists just to make saving less cumbersome
    public void Save()
    {
      
        Plugin.PluginInterface.SavePluginConfig(this);
        
    }
}
