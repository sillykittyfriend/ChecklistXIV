using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;

namespace ChecklistXIV.Windows;

public class MainWindow : Window, IDisposable
{
    //
    private readonly Configuration configuration;
    private readonly Plugin plugin;
    private readonly IPluginLog log;
    private string search = string.Empty;
    private List<(uint Id, string Name)> items = new();
    private List<(uint Id, string Name)> SelectedItems;
    
    private uint selectedItemId;
    
    // We give this window a hidden ID using ##.
    // The user will see "My Amazing Window" as window title,
    // but for ImGui the ID is "My Amazing Window##With a hidden ID"
    public MainWindow(Plugin plugin, IDataManager data,IPluginLog log,List<(uint Id, string Name)> selectedItems)
        : base("Checklist", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
        this.plugin = plugin;
        this.log = log;
        this.SelectedItems = selectedItems;
        var sheet = data.GetExcelSheet<Lumina.Excel.Sheets.Item>()!;
        configuration = plugin.Configuration;
        items = sheet.Where(i => i.RowId != 0).Select(i => (i.RowId, i.Name.ToString())).ToList();
        
    }




    public void Dispose()
    {
        
    }

    public override void Draw()
    {
        ImGui.InputText("Item", ref search, 100);
        
if (!string.IsNullOrWhiteSpace(search))
{
    //Creating the child and check if its null
    using var child = ImRaii.Child("##dropdown", new System.Numerics.Vector2(300, 200), true);
    if (child.Success)
    {
            
        
    
//Loop through each item and see if the current search text is contained
        foreach (var item in items.Where(i => i.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).Take(100))
        {
         //If item is found and clicked set search back to blank add it to the list and then save the configuration
            if (ImGui.Selectable(item.Name))
            {
                selectedItemId = item.Id;
                log.Information($"{item.Name}");
               SelectedItems.Add((item.Id,item.Name));
               configuration.Save();
                search = "";
                
            }
        }

    }
}
        
        
        ImGui.Spacing();
        
     //Adds a button and adds all the selected items from the selected item list into a always updating list of ImGui.Text instances with a Button next to them for removal
        using (var child = ImRaii.Child("SomeChildWithAScrollbar", Vector2.Zero, true))
        {
            // Check if this child is drawing
            if (child.Success)
            {
                
                for (int i = SelectedItems.Count - 1; i >= 0; i--)
                {
                    string s = SelectedItems[i].Name;

                    
                    ImGui.Text(s);
                    ImGui.SameLine();
                    ImGui.PushID(i);
                    if (ImGui.Button("X"))
                    {
                        SelectedItems.RemoveAt(i);
                        configuration.Save();
                        
                        

                    }
                    ImGui.PopID();
                }
               
           
            }
        }
    }
}
