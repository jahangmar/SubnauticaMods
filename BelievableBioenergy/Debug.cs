using HarmonyLib;
using QModManager.Utility;
using System.Collections.Generic;

namespace BelievableBioenergy
{
    class Debug
    {
        //[HarmonyPatch(typeof(Player), "GetOxygenAvailable")]
        internal class PlayerGetOxygenAvailable
        {
            static float time = 0f;

            public static void Prefix()
            {
                if (UnityEngine.Time.time > time + 1f)
                {
                    time = UnityEngine.Time.time;
                }
                else
                    return;

                string relays = "relays:\n";

                foreach (PowerRelay p in PowerRelay.relayList)
                {
                    System.Reflection.FieldInfo fieldInfo = typeof(PowerRelay).GetField("inboundPowerSources", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    List<IPowerInterface> inboundPowerSources = (List<IPowerInterface>)(fieldInfo.GetValue(p));

                    int num = inboundPowerSources.Count;
                    string sources = "";
                    foreach (IPowerInterface inf in inboundPowerSources)
                    {
                        if (inf is PowerSource s)
                            sources += s.name + "(s), ";
                        else if (inf is PowerRelay r)
                            sources += r.name + "(r), ";
                        else
                            sources += "???, ";
                    }

                    relays += $"#{p.name} has {num} relays/sources: " + sources + "\n";
                }
                ErrorMessage.AddMessage(relays);
                return;
            }
        }
    }
}
