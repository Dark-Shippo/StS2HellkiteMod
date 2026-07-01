using Hellkite.HellkiteCode.Commands;
using Hellkite.HellkiteCode.Hooks;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hellkite.HellkiteCode.Relics;

/// <summary>Uncommon relic. The first time Scorch triggers each turn, gain 1 Charge.</summary>
public class DragonsHeart() : HellkiteRelic, IAfterScorchTriggered
{
    private const int Charge = 1;

    private bool _gainedThisTurn;

    public override RelicRarity Rarity => RelicRarity.Uncommon;

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner)
            _gainedThisTurn = false;

        return Task.CompletedTask;
    }

    public async Task AfterScorchTriggered(Creature scorchedTarget)
    {
        if (_gainedThisTurn)
            return;

        _gainedThisTurn = true;
        await HellkitePlayerCmd.GainFireUp(new FireUp(Charge), Owner);
    }
}
