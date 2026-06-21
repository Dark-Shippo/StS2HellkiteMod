using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace Hellkite.HellkiteCode.Nodes;

[GlobalClass]
public partial class NChargeCounter : Control
{
    public static AddedNode<NEnergyCounter, NChargeCounter> Node = new(CustomEnergyCounter =>
    {
        var control = new NChargeCounter();
        
        var tex = ResourceLoader.Load<Texture2D>("res://mod/images/image.png");
        
        var size = tex.GetSize();
        var texRect = new TextureRect();
        texRect.Name = tex.ResourcePath;
        texRect.Size = new(50, 50);
        texRect.Texture = tex;
        texRect.PivotOffset = size / 2f;
        texRect.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
        texRect.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
        texRect.MouseFilter = MouseFilterEnum.Ignore;
        
        control.Size = new(50, 50);
        control.Position = new(-126, -231);
        control.AddChild(texRect);
        
        var label = new Label { Text = "1" };
        label.SetAnchorsAndOffsetsPreset(LayoutPreset.Center);
        control.AddChild(label);
        
        return control;
    });
}