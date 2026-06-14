using Hellkite.HellkiteCode.Fire_Up;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Rare.Attacks;

public sealed class HonedTalons() : HellkiteCard(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    private int _currentDamage = 13;
    private int _increasedDamage;
    
    [SavedProperty]
    private int CurrentDamage
    {
        get => this._currentDamage;
        set
        {
            AssertMutable();
            _currentDamage = value;
            DynamicVars.Damage.BaseValue = _currentDamage;
        }
    }
    
    [SavedProperty]
    private int IncreasedDamage
    {
        get => _increasedDamage;
        set
        {
            AssertMutable();
            _increasedDamage = value;
        }
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(CurrentDamage, ValueProp.Move), 
        new IntVar("Increase", 3M)
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target != null)
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        int intValue = DynamicVars["Increase"].IntValue;
        BuffFromPlay(intValue);
        if (!(DeckVersion is HonedTalons deckVersion))
            return;
        deckVersion.BuffFromPlay(intValue);
        if (ChargeHandler.HasSpentChargeThisTurn(Owner.Creature))
        {
            deckVersion.BuffFromPlay(intValue);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Increase"].UpgradeValueBy(1M);
    }
    
    protected override void AfterDowngraded() => this.UpdateDamage();

    private void BuffFromPlay(int extraDamage)
    {
        IncreasedDamage += extraDamage;
        UpdateDamage();
    }

    private void UpdateDamage() => CurrentDamage = 13 + IncreasedDamage;
}