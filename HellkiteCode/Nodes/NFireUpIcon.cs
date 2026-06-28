using Godot;
using Hellkite.HellkiteCode.Cards;
using Hellkite.HellkiteCode.Utilities;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace Hellkite.HellkiteCode.Nodes;

public partial class NFireUpIcon : TextureRect
{
    //private Font _font = BaseResourceIndex.FontKreonBoldShared;
//
    //private MegaLabel[] _labels = new MegaLabel[1];
//
    //private TextureRect[] _unplayableIcons = new TextureRect[1];
//
    //public NCard? NCard { get; private set; }
//
    //public NFireUpIcon WithData(NCard nCard)
    //{
    //    NCard = nCard;
//
    //    return this;
    //}
//
    //public override void _Ready()
    //{
    //    _labels =
    //    [
    //        CreateLabel(NFireUpCounter.RedFontColor),
    //    ];
    //    _unplayableIcons =
    //    [
    //        //CreateUnplayableIcon(),
    //    ];
//
    //    _labels[1].AddChild(_unplayableIcons[1]);
    //    _unplayableIcons[1].Position = new Vector2(-4, 0);
    //    AddChild(_labels[1]);
//
    //    _labels[0].Position = new Vector2(34, 17);
    //}
//
    //public void UpdateFireUpCostVisuals(PileType pileType)
    //{
    //    if (NCard!.Visibility != ModelVisibility.Visible || NCard.Model is not HellkiteCard hellkiteCard)
    //    {
    //        for (var i = 0; i < 3; i++)
    //        {
    //            var label = _labels[i];
    //            label.SetTextAutoSize(string.Empty);
    //            var fontColor = GetFontColor(i);
    //            label.AddThemeColorOverride(ThemeConstants.Label.FontColor, fontColor.Item1);
    //            label.AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, fontColor.Item3);
    //        }
//
    //        Visible = false;
    //        return;
    //    }
//
    //    var fireUpModifier = hellkiteCard.GetFireUpCostWithModifiers();
    //    var fireUpCostColor = HellkiteCardCostHelper.GetFireUpCostColor(hellkiteCard, hellkiteCard.CombatState);
    //    var isXCost = hellkiteCard.HasFireUpCostX;
    //    for (var i = 0; i < 3; i++)
    //    {
    //        var label = _labels[i];
    //        label.SetTextAutoSize(isXCost
    //            ? (i == 0 ? "X" : string.Empty)
    //            : fireUpModifier.ByIndex(i).ToString());
    //        UpdateFireUpCostColor(pileType, hellkiteCard, fireUpCostColor, i);
    //    }
//
    //    Visible = isXCost || fireUpModifier.Total >= 0;
//
    //    var shouldShowUnplayableIcon = false;
    //    if (pileType == PileType.Hand && !hellkiteCard.CanPlay(out var reason, out _))
    //        shouldShowUnplayableIcon = !((reason & UnplayableReason.StarCostTooHigh) != 0);
//
    //    foreach (var unplayableIcon in _unplayableIcons) unplayableIcon.Visible = shouldShowUnplayableIcon;
    //}
//
    //private void UpdateFireUpCostColor(PileType pileType, HellkiteCard card, CardCostColor fireUpCostColor,
    //    int index)
    //{
    //    var (fontColor, _, fontOutlineColor) = GetFontColor(index);
//
    //    if (card.WasFireUpCostJustUpgraded)
    //    {
    //        fontColor = StsColors.green;
    //        fontOutlineColor = StsColors.energyGreenOutline;
    //    }
//
    //    if (pileType == PileType.Hand)
    //    {
    //        fontColor = NCard.GetCostTextColorInHand(fireUpCostColor, NCard!._pretendCardCanBePlayed, fontColor);
    //        fontOutlineColor =
    //            NCard.GetCostOutlineColorInHand(fireUpCostColor, NCard!._pretendCardCanBePlayed, fontOutlineColor);
    //    }
//
    //    _labels[index].AddThemeColorOverride(ThemeConstants.Label.FontColor, fontColor);
    //    _labels[index].AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, fontOutlineColor);
    //}
//
    //private static (Color, Color, Color) GetFontColor(int index)
    //{
    //    return index switch
    //    {
    //        0 => NFireUpCounter.RedFontColor,
    //        _ => throw new ArgumentOutOfRangeException()
    //    };
    //}
//
    //private MegaLabel CreateLabel((Color, Color, Color) fontColor)
    //{
    //    var label = new MegaLabel();
    //    label.MaxFontSize = 22;
    //    label.MinFontSize = 16;
    //    label.AutoSizeEnabled = false;
    //    label.HorizontalAlignment = HorizontalAlignment.Center;
    //    label.VerticalAlignment = VerticalAlignment.Center;
    //    label.LayoutMode = 0; //position
    //    label.Size = new Vector2(28, 36);
    //    label.SetAnchorsPreset(LayoutPreset.Center);
    //    label.AddThemeColorOverride("font_color", fontColor.Item1);
    //    label.AddThemeColorOverride("font_shadow_color", fontColor.Item2);
    //    label.AddThemeColorOverride("font_outline_color", fontColor.Item3);
    //    label.AddThemeConstantOverride("shadow_offset_x", 2);
    //    label.AddThemeConstantOverride("shadow_offset_y", 2);
    //    label.AddThemeConstantOverride("outline_size", 12);
    //    label.AddThemeConstantOverride("shadow_outline_size", 12);
    //    label.AddThemeFontOverride("font", _font);
    //    label.AddThemeFontSizeOverride("font_size", 22);
    //    label.Text = "0";
//
    //    return label;
    //}
//
    //private static TextureRect CreateUnplayableIcon()
    //{
    //    var textRect = new TextureRect();
    //    textRect.Texture = BaseResourceIndex.CardUnplayableIcon;
    //    textRect.ExpandMode = ExpandModeEnum.IgnoreSize;
    //    textRect.Size = new Vector2(36, 36);
    //    return textRect;
    //}
}