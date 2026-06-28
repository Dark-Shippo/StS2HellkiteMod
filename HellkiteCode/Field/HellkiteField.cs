using BaseLib.Utils;
using Hellkite.HellkiteCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using static Hellkite.HellkiteCode.Extensions.PlayerCombatStateExtension;

namespace Hellkite.HellkiteCode.Field;

public static class HellkiteField
{
    public static readonly SpireField<CardModel, CardModelExtension.HellkiteCardModelModifier> Modifier =
        new(card => new CardModelExtension.HellkiteCardModelModifier(card));
    
    public static readonly SpireField<PlayerCombatState, HellkiteCombatState> HellkiteCombatState = new(() => null);
}