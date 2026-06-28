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
        var fireUpCounter = PreloadManager.Cache.GetScene(HellkiteResource.NFireUpCounterPath)
            .Instantiate<NFireUpCounter>();
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