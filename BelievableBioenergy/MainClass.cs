using System.Reflection;
using HarmonyLib;
using QModManager.API.ModLoading;
using Logger = QModManager.Utility.Logger;

namespace BelievableBioenergy
{
    [QModCore]
    public class MainClass
    {
        [QModPatch]
        public static void Patch()
        {            
            Assembly assembly = Assembly.GetExecutingAssembly();
            Harmony harmony = new Harmony(QModManager.API.QModServices.Main.GetMyMod().Id);
            harmony.PatchAll(assembly);
            
        }        
    }
}