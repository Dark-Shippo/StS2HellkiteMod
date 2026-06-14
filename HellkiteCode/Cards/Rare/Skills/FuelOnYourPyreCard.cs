using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Hellkite.HellkiteCode.Cards.Rare.Skills;

public sealed class FuelOnYourPyreCard() : HellkiteCard(1, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<ScorchPower>(6M),
        new ChargeCostVar(2M)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await ChargeHandler.LoseCharge(Owner.Creature, DynamicVars[nameof(ChargeCostVar)].BaseValue);

        if (play.Target != null)
        {
            await PowerCmd.Apply<ScorchPower>(play.Target, DynamicVars[nameof(ScorchPower)].BaseValue, Owner.Creature,
                this);

            int targetScorch = play.Target.GetPowerAmount<ScorchPower>();
            if (targetScorch > 0)
            {
                decimal spreadAmount = Math.Floor(targetScorch / 2M);
                if (spreadAmount > 0)
                {
                    if (CombatState != null)
                        foreach (var enemy in CombatState.HittableEnemies)
                        {
                            if (enemy != play.Target)
                            {
                                await PowerCmd.Apply<ScorchPower>(enemy, spreadAmount, Owner.Creature, this);
                            }
                        }
                }
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars[nameof(ScorchPower)].UpgradeValueBy(4M);
    }
}