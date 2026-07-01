using Godot;
using Hellkite.HellkiteCode.Extensions;

namespace Hellkite.HellkiteCode.Utilities;

public static class HellkiteResource
{
    public static Texture2D FireUpIcon =>
        ResourceLoader.Load<Texture2D>("res://Hellkite/images/charui/big_energy.png");
    
    public const string NFireUpCounterPath = "res://Hellkite/scenes/combat/energy_counters/fireup_counter.tscn";
    public const string NFireUpIconPath = "res://Hellkite/scenes/cards/fireup_icon.tscn";
    public const string NCreatureVisualsHellkitePath = "res://Hellkite/scenes/creature_visuals/hellkite.tscn";

    public const string NCharSelectBgHellkitePath =
        "res://Hellkite/scenes/screens/char_select/char_select_bg_hellkite.tscn";

    // These assets will be loaded with PreloadManager.
    // NFireUpCounterPath/NFireUpIconPath are intentionally omitted: the counter is built in
    // code now, and the per-card cost uses the native star pip (no custom cost-icon scene).
    public static readonly IEnumerable<string> AssetPaths =
    [
        NCreatureVisualsHellkitePath,
        NCharSelectBgHellkitePath,
        "big_energy.png".CharacterUiPath().ToRes(),
    ];
}