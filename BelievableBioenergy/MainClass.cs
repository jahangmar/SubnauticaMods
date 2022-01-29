using System.Reflection;
using System.Collections.Generic;
using HarmonyLib;
using QModManager.API.ModLoading;
using SMLHelper.V2.Json;
using SMLHelper.V2.Options;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Options.Attributes;
using SMLHelper.V2.Interfaces;

namespace BelievableBioenergy
{
    [QModCore]
    public class MainClass
    {
        [QModPatch]
        public static void Patch()
        {

            ModOptions config = OptionsPanelHandler.Main.RegisterModOptions<ModOptions>();
            GreedyFabricator.FabricatorChangesActive = config.FabricatorChanges;

            Dictionary<TechType, float> charge = (Dictionary<TechType, float>) typeof(BaseBioReactor).GetField("charge", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
            ModPlantValues.SetCharges(charge);

            Assembly assembly = Assembly.GetExecutingAssembly();
            Harmony harmony = new Harmony(QModManager.API.QModServices.Main.GetMyMod().Id);
            harmony.PatchAll(assembly);
        }        
    }

    [Menu("Believable Bioenergy")]
    class ModOptions : ConfigFile
    {
        [Toggle("Fabricator changes"), OnChange(nameof(ModFabricatorToggleEvent)), OnChange(nameof(ModGenericValueChangedEvent))]
        public bool FabricatorChanges = false;

        public void ModFabricatorToggleEvent(ToggleChangedEventArgs e)
        {
            GreedyFabricator.FabricatorChangesActive = e.Value;
        }

        public void ModGenericValueChangedEvent(IModOptionEventArgs e)
        {

        }
    }
}