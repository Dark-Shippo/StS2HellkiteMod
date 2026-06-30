using Godot;
using HarmonyLib;
using Hellkite.HellkiteCode.Cards;
using Hellkite.HellkiteCode.Utilities;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace Hellkite.HellkiteCode.Patches;

// Option B: render a Hellkite card's FireUp cost on the base game's native star-cost
// pip (the same one Regent's stars use), so no custom cost icon/asset is required.
// Hellkite cards leave their native StarCost at the default (-1 = hidden), so the base
// UpdateStarCostVisuals hides the pip; this postfix re-shows it with the FireUp cost.
[HarmonyPatch(typeof(NCard), nameof(NCard.UpdateStarCostVisuals))]
internal class NCardUpdateStarCostVisualsPatch
{
    [HarmonyPostfix]
    private static void Postfix(NCard __instance, PileType pileType)
    {
        if (__instance.Visibility != ModelVisibility.Visible) return;
        if (__instance.Model is not HellkiteCard card) return;

        if (card.HasFireUpCostX)
        {
            __instance._starLabel.SetTextAutoSize("X");
            __instance._starIcon.Visible = true;
        }
        else
        {
            var cost = card.GetFireUpCostWithModifiers().Total;
            if (cost <= 0) return; // No FireUp cost: keep base behavior (pip stays hidden).
            __instance._starLabel.SetTextAutoSize(cost.ToString());
            __instance._starIcon.Visible = true;
        }

        UpdateFireUpCostColor(__instance, card, pileType);
    }

    // Mirrors NCard.UpdateStarCostColor, but keyed off FireUp cost/affordability.
    private static void UpdateFireUpCostColor(NCard instance, HellkiteCard card, PileType pileType)
    {
        var textColor = StsColors.cream;
        var outlineColor = StsColors.defaultStarCostOutline;

        if (!card.HasFireUpCostX && card.WasFireUpCostJustUpgraded)
        {
            textColor = StsColors.green;
            outlineColor = StsColors.energyGreenOutline;
        }
        else if (pileType == PileType.Hand)
        {
            var costColor = HellkiteCardCostHelper.GetFireUpCostColor(card, card.CombatState);
            textColor = NCard.GetCostTextColorInHand(costColor, instance._pretendCardCanBePlayed, textColor);
            outlineColor = NCard.GetCostOutlineColorInHand(costColor, instance._pretendCardCanBePlayed, outlineColor);
        }

        instance._starLabel.AddThemeColorOverride(ThemeConstants.Label.FontColor, textColor);
        instance._starLabel.AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, outlineColor);
    }
}

// Nudge the enchantment tab down when a Hellkite card shows a FireUp (star) cost,
// so the tab doesn't overlap the cost pip.
[HarmonyPatch(typeof(NCard), nameof(NCard.UpdateEnchantmentVisuals))]
internal class NCardUpdateEnchantmentVisualsPatch
{
    [HarmonyPostfix]
    private static void Postfix(NCard __instance)
    {
        if (__instance.Model is HellkiteCard { BaseFireUpCost.Total: >= 0 })
            __instance._enchantmentTab.Position = __instance._defaultEnchantmentPosition + Vector2.Down * 20f;
    }
}
