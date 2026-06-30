using Godot;
using Hellkite.HellkiteCode.Extensions;
using Hellkite.HellkiteCode.Structs;
using Hellkite.HellkiteCode.Utilities;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Hellkite.HellkiteCode.Nodes;

/// <summary>
/// On-screen meter showing the player's total FireUp (Charge). Built entirely in code
/// from the bundled fireup icon, so it has no dependency on a Godot scene/shader/atlas
/// (the original counter scene was an unconverted Runesmith leftover).
/// </summary>
[GlobalClass]
public partial class NFireUpCounter : Control
{
    private static readonly Color FontColor = new("ffe3e3");
    private static readonly Color ShadowColor = new("00000030");
    private static readonly Color OutlineColor = new("541111");

    private Player? _player;

    private TextureRect _icon = null!;

    private MegaLabel _label = null!;

    private bool _isListeningToCombatState;

    public void Initialize(Player player)
    {
        _player = player;
        ConnectFireUpChangedSignal();
        RefreshVisibility();
    }

    public override void _Ready()
    {
        MouseFilter = MouseFilterEnum.Ignore;

        _icon = new TextureRect
        {
            Texture = HellkiteResource.FireUpIcon,
            ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
            StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
            MouseFilter = MouseFilterEnum.Ignore
        };
        _icon.SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect);
        AddChild(_icon);

        _label = CreateLabel();
        _label.SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect);
        AddChild(_label);

        SetFireUpCountText(new FireUp(0));
        Visible = false;
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        ConnectFireUpChangedSignal();
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        if (_player == null || !_isListeningToCombatState) return;
        var hellkiteCombatState = _player.PlayerCombatState?.Hellkite();
        if (hellkiteCombatState != null) hellkiteCombatState.FireUpChanged -= OnFireUpChanged;
        _isListeningToCombatState = false;
    }

    private void ConnectFireUpChangedSignal()
    {
        if (_player == null || _isListeningToCombatState) return;
        var hellkiteCombatState = _player.PlayerCombatState?.Hellkite();
        if (hellkiteCombatState == null) return;
        hellkiteCombatState.FireUpChanged += OnFireUpChanged;
        _isListeningToCombatState = true;
    }

    private void OnFireUpChanged(FireUp oldFireUp, FireUp newFireUp)
    {
        SetFireUpCountText(newFireUp);
        RefreshVisibility();
    }

    private static MegaLabel CreateLabel()
    {
        var label = new MegaLabel
        {
            MaxFontSize = 40,
            AutoSizeEnabled = false,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            MouseFilter = MouseFilterEnum.Ignore
        };
        label.AddThemeColorOverride("font_color", FontColor);
        label.AddThemeColorOverride("font_shadow_color", ShadowColor);
        label.AddThemeColorOverride("font_outline_color", OutlineColor);
        label.AddThemeConstantOverride("shadow_offset_x", 3);
        label.AddThemeConstantOverride("shadow_offset_y", 3);
        label.AddThemeConstantOverride("outline_size", 15);
        label.AddThemeConstantOverride("shadow_outline_size", 15);
        label.AddThemeFontSizeOverride("font_size", 40);
        label.Text = "0";
        return label;
    }

    private void SetFireUpCountText(FireUp fireUp)
    {
        _label?.SetTextAutoSize(fireUp.Total.ToString());
    }

    private void RefreshVisibility()
    {
        if (_player == null)
        {
            Visible = false;
            return;
        }

        var fireUp = _player.PlayerCombatState?.GetFireUp() ?? new FireUp();
        var shouldAlwaysShowFireUp = _player.Character is Character.Hellkite;
        Visible = shouldAlwaysShowFireUp || fireUp.Total > 0;
    }
}
