using Hellkite.HellkiteCode.Fire_Up;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hellkite.HellkiteCode.Powers;

public sealed class ChargedScalesPower : HellkitePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player || !cardPlay.IsLastInSeries)
            return;
        
        await DealDamageToAllEnemies();
    }

    public async Task AfterChargeGained(int amount, Player gainer)
    {
        if (amount <= 0 || gainer != Owner.Player)
            return;
        await DealDamageToAllEnemies();
    }

    public async Task AfterChargeSpent(int amount, Player spender)
    {
        if (amount <= 0 || spender != Owner.Player)
            return;
        await DealDamageToAllEnemies();
    }
    
    private async Task DealDamageToAllEnemies()
    {
        Flash();
        await HellkiteCmd.AttackAll(null, null, DynamicVars.Damage.BaseValue);
    }
}