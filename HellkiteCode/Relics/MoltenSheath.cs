using Hellkite.HellkiteCode.Hooks;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hellkite.HellkiteCode.Relics;

/// <summary>Uncommon relic. The first time you lose Charge each turn, gain 2 Plating.</summary>
public class MoltenSheath() : HellkiteRelic, IAfterFireUpSpent
{
    private const int Plating = 2;

    private bool _platedThisTurn;

    public override RelicRarity Rarity => RelicRarity.Uncommon;

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner)
            _platedThisTurn = false;

        return Task.CompletedTask;
    }

    public async Task AfterFireUpSpent(FireUp fireUp, Player spender)
    {
        if (_platedThisTurn)
            return;

        _platedThisTurn = true;
        await PowerCmd.Apply<PlatingPower>(null, Owner.Creature, Plating, Owner.Creature, null);
    }
}
