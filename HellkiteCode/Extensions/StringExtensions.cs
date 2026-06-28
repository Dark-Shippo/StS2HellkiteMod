
using Hellkite.HellkiteCode.Utilities;

namespace Hellkite.HellkiteCode.Extensions;

//Mostly utilities to get asset paths.
public static class StringExtensions
{
    public static string ToRes(this string path)
    {
        return Path.Join("res://", path);
    }

    public static string ImagePath(this string path)
    {
        return Path.Join(HellkiteMod.ModId, "images", path);
    }

    public static string CardImagePath(this string path)
    {
        return Path.Join(HellkiteMod.ModId, "images", "card_portraits", path);
    }

    public static string BigCardImagePath(this string path)
    {
        return Path.Join(HellkiteMod.ModId, "images", "card_portraits", "big", path);
    }

    public static string EnchantmentImagePath(this string path)
    {
        return Path.Join(HellkiteMod.ModId, "images", "enchantments", path);
    }

    public static string PowerImagePath(this string path)
    {
        return Path.Join(HellkiteMod.ModId, "images", "powers", path);
    }

    public static string BigPowerImagePath(this string path)
    {
        return Path.Join(HellkiteMod.ModId, "images", "powers", "big", path);
    }

    public static string RelicImagePath(this string path)
    {
        return Path.Join(HellkiteMod.ModId, "images", "relics", path);
    }

    public static string BigRelicImagePath(this string path)
    {
        return Path.Join(HellkiteMod.ModId, "images", "relics", "big", path);
    }

    public static string PotionImagePath(this string path)
    {
        return Path.Join(HellkiteMod.ModId, "images", "potions", path);
    }

    public static string CharacterUiPath(this string path)
    {
        return Path.Join(HellkiteMod.ModId, "images", "charui", path);
    }

    public static string ScenePath(this string path, string dir)
    {
        return Path.Join(HellkiteMod.ModId, "scenes", dir, path + ".tscn");
    }
}