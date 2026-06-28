using Hellkite.HellkiteCode.Field;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace Hellkite.HellkiteCode.Extensions;

public static class CardModelExtension
{
    public class HellkiteCardModelModifier(CardModel cardModel)
    {
        public CardModel CardModel { get; set; } = cardModel;

        public bool Stasis
        {
            get;
            set
            {
                CardModel.AssertMutable();
                field = value;
            }
        }

        public HellkiteCardModelModifier Clone(CardModel cardModel)
        {
            var ret = (HellkiteCardModelModifier)MemberwiseClone();
            ret.CardModel = cardModel;
            return ret;
        }
    }

    public static HellkiteCardModelModifier GetCardModelModifier(this CardModel cardModel)
    {
        return HellkiteField.Modifier[cardModel]!;
    }
}