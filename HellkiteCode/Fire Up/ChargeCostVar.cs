using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Hellkite.HellkiteCode.Fire_Up;

public sealed class ChargeCostVar: DynamicVar
{
    public const string DefaultName = "Charge";

    public ChargeCostVar(decimal amount)
        : base(DefaultName, amount)
    {
    }

    public ChargeCostVar(string name, decimal amount)
        : base(name, amount)
    {
    }
    
    public override void UpdateCardPreview(
        CardModel card,
        CardPreviewMode previewMode,
        Creature? target,
        bool runGlobalHooks)
    {
        EnchantedValue = BaseValue;
        PreviewValue = BaseValue;
    }
    
}