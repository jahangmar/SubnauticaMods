using HarmonyLib;

namespace BelievableBioenergy
{
    class PlanterEnergy
    {

        const float maxDepth = 200f;


        public static float DepthFactor(Planter planter)
        {
            return UnityEngine.Mathf.Clamp01((maxDepth - Ocean.main.GetDepthOf(planter.gameObject)) / maxDepth);
        }

        static public float GetEnergyForPlanting(TechType seedTechType)
        {
            float modCharge = BaseBioReactor.GetCharge(ModPlantValues.GetFruit(seedTechType));
            if (seedTechType == TechType.PurpleVegetable)
                return modCharge * ModPlantValues.purpleVegetableYield;
            if (seedTechType == TechType.HangingFruit)
                return modCharge * ModPlantValues.hangingFruitYield;
            if (seedTechType == TechType.BulboTreePiece)
                return modCharge * ModPlantValues.bulboTreeYield;
            return modCharge;
        }

        [HarmonyPatch(typeof(Planter))]
        [HarmonyPatch("IsAllowedToAdd")]
        internal class PlanterIsAllowedToAdd
        {
            [HarmonyPostfix]
            public static void Postfix(Pickupable pickupable, ref bool __result, Planter __instance)
            {
                if (__result)
                {
                    PowerRelay powerRelay = __instance.gameObject.GetComponentInParent<PowerRelay>();                    

                    //ErrorMessage.AddMessage("Depth factor is " + DepthFactor(__instance));

                    if (ModPlantValues.IsSeed(pickupable.GetTechType()) && __instance.isIndoor)
                    {
                        float needs = GetEnergyForPlanting(pickupable.GetTechType());
                        float dneeds = needs * (1 - DepthFactor(__instance));
                        
                        //ErrorMessage.AddMessage($"needs is {dneeds} ({needs} without depth).");
                        bool enoughPower = powerRelay != null && (dneeds == 0 || powerRelay.GetPower() > dneeds);
                        //ErrorMessage.AddMessage($"enoughPower is {enoughPower}.");
                        if (!enoughPower && dneeds > 0)
                            ErrorMessage.AddMessage($"not enough power (needs {dneeds})");
                        else if (dneeds > 0)
                        {
                            float consumed;
                            powerRelay.ConsumeEnergy(dneeds, out consumed);
                            //ErrorMessage.AddMessage($"requested {dneeds} power and {consumed} was consumed.");
                        }
                        else
                        {
                            //ErrorMessage.AddMessage("needs no power (needs is " + dneeds + ")");
                        }

                        __result = enoughPower;
                    }
                }                
            }
        }
    }
}