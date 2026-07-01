using Hellkite.HellkiteCode.Commands;
using Hellkite.HellkiteCode.Extensions;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hellkite.HellkiteCode.Potions;

/// <summary>Gain 12 Charge. This turn, entering Overcharge does not cause you to take double damage.</summary>
public sealed class Overcharger : HellkitePotion
{
    private const int Charge = 12;

    public override PotionRarity Rarity => PotionRarity.Rare;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        // Waive the Overcharge Drained penalty for this turn, then gain Charge
        // (which may immediately push the meter into Overcharge this same turn).
        Owner.PlayerCombatState?.Hellkite()?.SuppressOverchargeDrainThisTurn();
        await HellkitePlayerCmd.GainFireUp(new FireUp(Charge), Owner);
    }
}
