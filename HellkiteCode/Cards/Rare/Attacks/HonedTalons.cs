using Hellkite.HellkiteCode.Cards;
using Hellkite.HellkiteCode.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Cards.Rare.Attacks;

public sealed class HonedTalons() : HellkiteCard(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    private const int BaseDamageUnupgraded = 12;
    private const int BaseDamageUpgraded = 16;

    private int _currentDamage = BaseDamageUnupgraded;
    private int _increasedDamage;

    // Upgrade-aware base: 12 normally, 16 once upgraded.
    private int BaseDamage => IsUpgraded ? BaseDamageUpgraded : BaseDamageUnupgraded;

    [SavedProperty]
    private int CurrentDamage
    {
        get => _currentDamage;
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

        int increase = DynamicVars["Increase"].IntValue;
        int totalIncrease = increase;
        
        if (Owner.PlayerCombatState?.Hellkite()?.HasSpentFireUpThisTurn() == true)
            totalIncrease += increase;

        BuffFromPlay(totalIncrease);
        if (DeckVersion is HonedTalons deckVersion)
            deckVersion.BuffFromPlay(totalIncrease);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Increase"].UpgradeValueBy(1M);
        UpdateDamage();
    }

    protected override void AfterDowngraded() => UpdateDamage();

    private void BuffFromPlay(int extraDamage)
    {
        IncreasedDamage += extraDamage;
        UpdateDamage();
    }

    private void UpdateDamage() => CurrentDamage = BaseDamage + IncreasedDamage;
}