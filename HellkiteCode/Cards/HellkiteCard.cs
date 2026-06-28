using BaseLib.Abstracts;
using BaseLib.Utils;
using Hellkite.HellkiteCode.Character;
using Hellkite.HellkiteCode.DynamicVars;
using Hellkite.HellkiteCode.Extensions;
using Hellkite.HellkiteCode.Fire_Up;
using Hellkite.HellkiteCode.Hooks;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace Hellkite.HellkiteCode.Cards;

[Pool(typeof(HellkiteCardPool))]
public abstract class HellkiteCard(int cost, CardType type, CardRarity rarity, TargetType target) : CustomCardModel(cost, type, rarity, target)
{
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    //public override string CustomPortraitPath
    //{
    //    get
    //    {
    //        var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    //        return ResourceLoader.Exists(path) ? path : "card.png".CardImagePath();
    //    }
    //}

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    //public override string PortraitPath
    //{
    //    get
    //    {
    //        var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    //        return ResourceLoader.Exists(path) ? path : "card.png".CardImagePath();
    //    }
    //}

    //public override string BetaPortraitPath
    //{
    //    get
    //    {
    //        var path = $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    //        return ResourceLoader.Exists(path) ? path : "beta/card.png".CardImagePath();
    //    }
    //}

    //protected void WithTip(RunesmithHoverTip runesmithTip)
    //{
    //    switch (runesmithTip)
    //    {
    //        case RunesmithHoverTip.Elements:
    //            WithTip(new TooltipSource(_ => RunesmithHoverTipFactory.CreateElementsHoverTip()));
    //            break;
    //        default:
    //            WithTip(new TooltipSource(_ => RunesmithHoverTipFactory.Static(runesmithTip)));
    //            break;
    //    }
    //}

    //protected void WithRuneTip<T>() where T : RuneModel
    //{
    //    WithTip(new TooltipSource(_ => RunesmithHoverTipFactory.FromRune<T>()));
    //}

    protected override void AddExtraArgsToDescription(LocString description)
    {
        base.AddExtraArgsToDescription(description);
        description.Add("fireup", 0);
    }

    public event Action? FireUpCostChanged;

    public void InvokeFireUpCostChanged()
    {
        FireUpCostChanged?.Invoke();
    }

    private bool _fireUpCostSet;

    public virtual FireUp CanonicalFireUpCost => new(-1);

    public List<TemporaryCardCost> TemporaryFireUpCosts = [];

    public TemporaryCardCost? TemporaryFireUpCost => TemporaryFireUpCosts.LastOrDefault();

    public FireUp BaseFireUpCost
    {
        get
        {
            if (!IsMutable) return CanonicalFireUpCost;

            if (_fireUpCostSet) return field;

            field = CanonicalFireUpCost;
            _fireUpCostSet = true;
            return field;
        }
        private set
        {
            AssertMutable();
            if (HasFireUpCostX) return;
            field = value;
            _fireUpCostSet = true;
        }
    }

    public virtual FireUp CurrentFireUpCost
    {
        get
        {
            var tempCost = TemporaryFireUpCost?.Cost;
            if (!tempCost.HasValue || (tempCost == 0 && BaseFireUpCost.Total < 0)) return BaseFireUpCost;
            return new FireUp(tempCost.Value);
        }
    }
    
    public virtual bool HasFireUpCostX => false;

    public int LastFireUpSpent { get; private set; }
    
    public int ResolveFireUpXValue()
    {
        if (!HasFireUpCostX)
            throw new InvalidOperationException("This card does not have a FireUp X-cost.");
        return LastFireUpSpent;
    }

    // DeepCloneFields
    protected override void DeepCloneFields()
    {
        base.DeepCloneFields();
        TemporaryFireUpCosts = TemporaryFireUpCosts.ToList();
    }

    // AfterCloned
    protected override void AfterCloned()
    {
        base.AfterCloned();
        FireUpCostChanged = null;
    }

    // SetToFreeThisTurn - patch done
    // SetToFreeThisCombat - patch done
    // SetStarCostUntilPlayed - unused

    // SetStarCostThisTurn
    public void SetFireUpCostThisTurn(int cost)
    {
        AddTemporaryFireUpCost(TemporaryCardCost.ThisTurn(cost));
    }

    // SetStarCostThisCombat
    public void SetFireUpCostThisCombat(int cost)
    {
        AddTemporaryFireUpCost(TemporaryCardCost.ThisCombat(cost));
    }

    // GetStarCostThisCombat - unused

    // AddTemporaryStarCost
    private void AddTemporaryFireUpCost(TemporaryCardCost cost)
    {
        AssertMutable();
        TemporaryFireUpCosts.Add(cost);
        FireUpCostChanged?.Invoke();
    }

    // UpgradeStarCostBy - unused

    // GetStarCostWithModifiers - done
    public FireUp GetFireUpCostWithModifiers()
    {
        if (HasFireUpCostX)
            return Owner.PlayerCombatState?.GetFireUp() ?? new FireUp();

        if (Pile is { IsCombatPile: true } && CombatState != null)
            return HellkiteHook.ModifyFireUpCost(CombatState, this, CurrentFireUpCost);

        return CurrentFireUpCost;
    }

    // CostsEnergyOrStars - patch done

    // EndOfTurnCleanup - patch done

    // SpendResources - patch done

    // SpendStars
    public async Task SpendFireUp(FireUp amount)
    {
        LastFireUpSpent = amount.Total;
        if (amount.Total > 0 && Owner.PlayerCombatState != null)
        {
            var hellkiteCombatState = Owner.PlayerCombatState.Hellkite();
            if (hellkiteCombatState != null && Owner.Creature.CombatState != null)
            {
                hellkiteCombatState.LoseFireUp(amount);
                await HellkiteHook.AfterFireUpSpent(Owner.Creature.CombatState, amount, Owner);
            }
        }
    }

    public bool WasFireUpCostJustUpgraded { set; get; }

    public void UpgradeFireUpCostBy(FireUp addend)
    {
        if (addend.Total == 0)
            return;
        var baseFireUpCost = BaseFireUpCost;
        BaseFireUpCost += addend;
        WasFireUpCostJustUpgraded = false;
        if (BaseFireUpCost.Total >= baseFireUpCost.Total)
            return;
        TemporaryFireUpCosts.RemoveAll((Predicate<TemporaryCardCost>)(c => c.Cost > BaseFireUpCost.Total));
    }

    protected override void AfterDowngraded()
    {
        var cardModel = (HellkiteCard)ModelDb.GetById<CardModel>(Id).ToMutable();
        BaseFireUpCost = cardModel.CanonicalFireUpCost;
    }

    public bool IsPlayedWithoutFireUp;

    // OnPlayWrapper - patch done

    // DowngradeInternal - patch set base cost (not really needed)

    public bool HasFireUp()
    {
        if (!IsInCombat) return true;
        var fireUp = Owner.PlayerCombatState?.GetFireUp() ?? new FireUp();
        return fireUp.CanSpend(GetFireUpCostWithModifiers().ClampZero());
    }
    
    protected bool FirstAttack = true;
    
    public override Task AfterCardPlayed(PlayerChoiceContext playerChoiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type == CardType.Attack) FirstAttack = false;
        return Task.CompletedTask;
    }
    
    public async Task<int> SpendUpToX(int maxAmount)
    {
        if (maxAmount <= 0)
            return 0;

        var available = Owner.PlayerCombatState?.GetFireUp() ?? new FireUp();

        int chosen = Math.Min(maxAmount, available.Total);
        if (chosen <= 0)
            return 0;

        await SpendFireUp(new FireUp(chosen));
        return chosen;
    }

    public Task<int> SpendUpToX()
    {
        int max = DynamicVars[FireUpVar.defaultName].IntValue;
        return SpendUpToX(max);
    }
}