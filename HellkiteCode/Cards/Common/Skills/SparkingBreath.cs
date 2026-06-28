using BaseLib.Extensions;
using Hellkite.HellkiteCode.Commands;
using Hellkite.HellkiteCode.DynamicVars;
using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Powers;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Hellkite.HellkiteCode.Cards.Common.Skills;

public sealed class SparkingBreath() : HellkiteCard(0, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<ScorchPower>(2M),
        new FireUpVar(1).WithUpgrade(1)
        //new ChargeCostVar(1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target != null)
            await HellkiteCmd.ApplyScorch(play.Target, DynamicVars[nameof(ScorchPower)].BaseValue, Owner.Creature,
                this, choiceContext);
        await HellkitePlayerCmd.GainFireUp(new FireUp(this), Owner, play);
        //await ChargeHandler.GainCharge(Owner.Creature, DynamicVars[ChargeCostVar.DefaultName].IntValue, choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars[nameof(ScorchPower)].UpgradeValueBy(1M); 
        //DynamicVars[ChargeCostVar.DefaultName].UpgradeValueBy(1M);
    }
}