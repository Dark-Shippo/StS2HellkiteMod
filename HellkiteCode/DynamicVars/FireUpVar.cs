using Hellkite.HellkiteCode.Hooks;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.DynamicVars;

public class FireUpVar : DynamicVar
{
    public const string defaultName = "FireUp";

    public FireUpVar(int amount)
        : this("FireUp", amount)
    {
    }

    public FireUpVar(string name, int amount)
        : base(name, (decimal)amount)
    {
    }

    public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
    {
        decimal previewValue = ((DynamicVar)this).BaseValue;
        if (runGlobalHooks && card.CombatState != null)
        {
            previewValue = HellkiteHook.ModifyFireUpGain(card.CombatState, card.Owner, new FireUp(((DynamicVar)this).IntValue), (ValueProp)8, card, out IEnumerable<AbstractModel> _).Charge;
        }
        ((DynamicVar)this).PreviewValue = previewValue;
    }
}