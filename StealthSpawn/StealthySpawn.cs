using System.Globalization;
using BepInEx;
using RoR2;

namespace StealthySpawn
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.Timmeh42.StealthySpawn", "StealthySpawn", "1.0.0")]
    public class StealthySpawn : BaseUnityPlugin
    {
        public float ParseFloat(string strInput, float defaultVal = 1.0f, float min = float.MinValue, float max = float.MaxValue)
        {
            if (float.TryParse(strInput, NumberStyles.Any, CultureInfo.InvariantCulture, out float parsedFloat))
            {
                return parsedFloat <= min ? min : parsedFloat >= max ? max : parsedFloat;
            }
            return defaultVal;
        }

        public bool ParseBool(string strInput, bool defaultVal = true)
        {
            return bool.TryParse(strInput, out bool parsedBool) ? parsedBool : defaultVal;
        }

        float stealthTime => ParseFloat(Config.Wrap("StealthySpawn", "Duration", "Duration of the cloak effect on spawn", "4.0").Value, 43.0f, 0f);

        public void Awake()
        {
            Chat.AddMessage(string.Format("Stealthy Spawn duration: {0}s", stealthTime));

            On.RoR2.CharacterMaster.OnBodyStart += (orig, self, body) =>
            {
                orig(self, body);
                if (body.isPlayerControlled)
                {
                    body.AddTimedBuff(BuffIndex.Cloak, stealthTime);
                }
            };
        }
    }
}
