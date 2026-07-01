using Hellkite.HellkiteCode.Commands;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hellkite.HellkiteCode.Potions;

/// <summary>Gain 5 Charge.</summary>
public sealed class ChargeWater : HellkitePotion
{
    private const int Charge = 5;

    public override PotionRarity Rarity => PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        await HellkitePlayerCmd.GainFireUp(new FireUp(Charge), Owner);
    }
}
