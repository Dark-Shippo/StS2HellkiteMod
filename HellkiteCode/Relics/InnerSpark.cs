using Hellkite.HellkiteCode.Commands;
using Hellkite.HellkiteCode.Relics;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Rooms;

namespace Hellkite.HellkiteCode.Relics;

public class InnerSpark() : HellkiteRelic
{
    private const int StartingCharge = 3;

    public override RelicRarity Rarity => RelicRarity.Starter;

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is not CombatRoom)
            return;

        await HellkitePlayerCmd.GainFireUp(new FireUp(StartingCharge), Owner);
    }
}