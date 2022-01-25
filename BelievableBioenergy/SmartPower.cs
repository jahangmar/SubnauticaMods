using HarmonyLib;
using QModManager.Utility;
using System.Collections.Generic;

namespace BelievableBioenergy
{
    class SmartPower
    {       
        [HarmonyPatch(typeof(PowerRelay), "AddInboundPower")]
        internal class PowerRelayAddInboundPower
        {
            [HarmonyPostfix]
            public static void Postfix(List<IPowerInterface>  ___inboundPowerSources)
            {                
                ___inboundPowerSources.Sort(PowerSourceComparer.Comparer);
            }
        }
        class PowerSourceComparer : IComparer<IPowerInterface>
        {
            public static PowerSourceComparer Comparer = new PowerSourceComparer();

            public static int Priority(string name)
            {                
                if (name.Contains(nameof(SolarPanel)))
                    return 0;
                if (name.Contains("PowerTransmitter"))
                    return 1;
                if (name.Contains(nameof(ThermalPlant)))
                    return 2;
                if (name.Contains(nameof(BaseBioReactor)))
                    return 3;
                if (name.Contains(nameof(BaseNuclearReactor)))
                    return 4;                
                return 0;
            }

            public int Compare(IPowerInterface x, IPowerInterface y)
            {               
                string xName = x is PowerSource xs ? xs.name : x is PowerRelay xr ? xr.name : "";
                string yName = y is PowerSource ys ? ys.name : y is PowerRelay yr ? yr.name : "";

                return Priority(xName) - Priority(yName);
            }
        }
    }
}
