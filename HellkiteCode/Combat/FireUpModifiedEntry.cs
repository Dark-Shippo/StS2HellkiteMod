using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace Hellkite.HellkiteCode.Combat;

public class FireUpModifiedEntry(
    FireUp amount,
    Player player,
    int roundNumber,
    CombatSide currentSide,
    CombatHistory history,
    IEnumerable<Player> players) : CombatHistoryEntry(player.Creature, roundNumber, currentSide, history, players)
{
    public FireUp Amount { get; } = amount;

    public Player Player { get; } = player;

    public override string Description
    {
        get
        {
            Player player = ((CombatHistoryEntry)this).Actor.Player;
            string text = ((player != null) ? ((AbstractModel)player.Character).Id.Entry : null) + " " +
                          ((Amount.Total < 0) ? "lost" : "gained") + " ";
            string[] source = new string[1]
            {
                ((Amount.Charge != 0) ? $"{Amount.Charge} charge" : null) ?? "",
            };
            return text + " " + string.Join(", ", source.Where((string s) => !string.IsNullOrEmpty(s)));
        }
    }
}