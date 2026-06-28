using System.Reflection;
using BaseLib.Audio;
using BaseLib.Config;
using Godot.Bridge;
using HarmonyLib;
using Hellkite.HellkiteCode.Utilities;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;

namespace Hellkite.HellkiteCode;

[ModInitializer(nameof(Initialize))]
public class HellkiteMod
{
    public const string ModId = "Hellkite"; //Used for resource filepath

    public static Logger Logger { get; } =
        new(ModId, LogType.Generic);

    public static void Initialize()
    {
        Harmony harmony = new(ModId);

        harmony.PatchAll();

        ScriptManagerBridge.LookupScriptsInAssembly(Assembly.GetExecutingAssembly());
        
        ModConfigRegistry.Register(ModId, new HellkiteConfig());
        
        ModSound.SetSoundDefaultVolumeOffset("res://Hellkite/audio/hellkite_character_transition.ogg", 25f);
        ModSound.SetSoundDefaultVolumeOffset("res://Hellkite/audio/hellkite_character_select.ogg", 25f);
    }
}