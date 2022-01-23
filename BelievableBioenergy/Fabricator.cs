using HarmonyLib;

namespace BelievableBioenergy
{
    class GreedyFabricator
    {

        private const float additionalEnergyToConsume = 5f;
        private const float additionalEnergyToConsumeBeforeFixed = 20f;
        private const float durationFactor = 2f;
        private const float durationFactorBeforeFixed = 4f;

        private static bool EscapePodBroken()
        {
            if (Player.main.currentEscapePod == null)
            {
                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Error, "BelievableBioenergy Bug: GreedyFabricator.EscapePodBroken called but Player.main.currentEscapePod is null", showOnScreen: true);
                return false;
            }

            return Story.StoryGoalManager.main.IsGoalComplete(Player.main.currentEscapePod.fixPanelGoal.key);
        }

        private static float GetAdditionalEnergy()
        {
            if (EscapePodBroken())
            {
                return additionalEnergyToConsume;
            }
            else
                return additionalEnergyToConsumeBeforeFixed;
        }

        private static float GetDurationFactor()
        {
            if (EscapePodBroken())
            {
                return durationFactor;
            }
            else
                return durationFactorBeforeFixed;
        }

        public static bool IsInEscapePod() => Player.main.currentEscapePod != null;

        [HarmonyPatch(typeof(GhostCrafter))]
        [HarmonyPatch("Craft")]
        internal class PatchFabricatorCraft
        {
            [HarmonyPrefix]
            public static bool Prefix(PowerRelay ___powerRelay)
            {
                if (IsInEscapePod())
                {
                    return CrafterLogic.ConsumeEnergy(___powerRelay, GetAdditionalEnergy());
                }

                return true;                
            }
        }

        [HarmonyPatch(typeof(GhostCrafter))]
        [HarmonyPatch("HasEnoughPower")]
        internal class PathFabricatorHasEnoughPower
        {
            [HarmonyPostfix]
            public static void Postfix(ref bool __result, PowerRelay ___powerRelay)
            {
                __result = __result && (!IsInEscapePod() || ___powerRelay.GetPower() >= 5f + GetAdditionalEnergy());
            }
        }

        [HarmonyPatch(typeof(Crafter))]
        [HarmonyPatch("Craft")]
        internal class CrafterCraft
        {
            [HarmonyPrefix]
            public static bool Prefix(ref float duration)
            {
                duration = IsInEscapePod() ? duration * GetDurationFactor() : duration;
                return true;
            }
        }
    }
}
