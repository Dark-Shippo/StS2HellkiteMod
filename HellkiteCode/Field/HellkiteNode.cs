using BaseLib.Utils;
using Hellkite.HellkiteCode.Nodes;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using Hellkite.HellkiteCode.Utilities;

namespace Hellkite.HellkiteCode.Field;

public static class HellkiteNode
{
    public static readonly AddedNode<NCombatUi, NFireUpCounter> NFireUpCounter = new(ui =>
    {
        // Built in code (see NFireUpCounter) rather than instantiated from a scene, since
        // the old fireup_counter.tscn is an unconverted Runesmith scene (wrong script + missing assets).
        var fireUpCounter = new NFireUpCounter();
        ui.AddChild(fireUpCounter);
        return fireUpCounter;
    });

    //public static readonly AddedNode<NCard, NFireUpIcon> NFireUpIcon = new(card =>
    //{
    //    var fireUpIcon = PreloadManager.Cache.GetScene(HellkiteResource.NFireUpIconPath)
    //        .Instantiate<NFireUpIcon>().WithData(card);
    //    var cardContainer = card.GetChild(0)!;
    //    cardContainer.AddChild(fireUpIcon);
    //    cardContainer.MoveChild(fireUpIcon, cardContainer.GetNode("%StarIcon").GetIndex());
    //    return fireUpIcon;
    //});
}