using BaseLib.Abstracts;
using Hellkite.HellkiteCode.Cards.Ancient;
using Hellkite.HellkiteCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Basic;

public sealed class FirePunch() : HellkiteCard(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy), ITranscendenceCard
{
    public CardModel GetTranscendenceTransformedCard() => ModelDb.Card<DragonPunch>();
    
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(6M, ValueProp.Move),
        new PowerVar<ScorchPower>(2M)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        FirePunch cardSource = this;
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        if (play.Target != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
            await PowerCmd.Apply<ScorchPower>(
                choiceContext, 
                play.Target,
                DynamicVars[nameof(ScorchPower)].BaseValue,
                Owner.Creature,
                this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3M);
        DynamicVars[nameof(ScorchPower)].UpgradeValueBy(2M);
    }
}