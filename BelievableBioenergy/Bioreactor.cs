using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace BelievableBioenergy
{
    class Bioreactor
    {
        static public float MaxPlantCharge = BaseBioReactor.GetCharge(TechType.Melon);

        /*
        static public float ModifiedGetCharge(TechType techType)
        {
            return ModPlantValues.charge.GetOrDefault(techType, -1f);            
        }             

        [HarmonyPatch(typeof(BaseBioReactor))]
        [HarmonyPatch(nameof(BaseBioReactor.GetCharge))]
        internal class PatchBioreactorGetCharge
        {
            [HarmonyPrefix]
            public static bool Prefix(TechType techType, ref float __result)
            {
                __result = ModifiedGetCharge(techType);
                return false;                
            }
        }

        [HarmonyPatch(typeof (BaseBioReactor))]
        [HarmonyPatch("ProducePower")]
        internal class PatchBioReactorProducePower
        {

            [HarmonyPrefix]
            public static bool Prefix(BaseBioReactor __instance, ref float __result, float requested, ItemsContainer ____container, List<Pickupable> ___toRemove)
            {
                float power = 0f;
                if (requested > 0f && ____container.count > 0)
                {
                    __instance._toConsume += requested;
                    power = requested;
                    foreach (InventoryItem inventoryItem in ____container)
                    {
                        Pickupable item = inventoryItem.item;
                        TechType techType = item.GetTechType();
                        float value = ModifiedGetCharge(techType);
                        if (value > 0 && __instance._toConsume >= value)
                        {
                            __instance._toConsume -= value;
                            ___toRemove.Add(item);
                        }
                    }

                    for (int i = ___toRemove.Count - 1; i >= 0; i--)
                    {
                        ____container.RemoveItem(___toRemove[i], forced: true);
                        Object.Destroy((Object)(object)((Component)___toRemove[i]).gameObject);
                    }

                    ___toRemove.Clear();
                    if (____container.count == 0)
                    {
                        power -= __instance._toConsume;
                        __instance._toConsume = 0f;
                    }
                }

                __result = power;
                return false;
            }
        }
        
        [HarmonyPatch(typeof(BaseBioReactor))]
        [HarmonyPatch("OnAddItem")]
        internal class PatchBioReactorOnAddItem
        {
            [HarmonyPostfix]
            public static bool Prefix(InventoryItem item)
            {
                if (item != null && item.item != null && BaseBioReactor.GetCharge(item.item.GetTechType()) > 0)
                {
                    //ErrorMessage.AddMessage($"Added {System.Enum.GetName(typeof(TechType), item.item.GetTechType())} with a charge of {BaseBioReactor.GetCharge(item.item.GetTechType())}.");
                }

                return true;
            }
        }        
        */
    }
}
