using Godot;
using Hellkite.HellkiteCode.Extensions;
using Hellkite.HellkiteCode.Structs;
using Hellkite.HellkiteCode.Utilities;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace Hellkite.HellkiteCode.Nodes;

[GlobalClass]
public partial class NFireUpCounter : Control
{
    private static readonly StringName _v = new("v");

    private static readonly StringName _s = new("s");

    public static readonly (Color, Color, Color) RedFontColor = (new Color("ffe3e3"), new Color("00000030"),
        new Color("541111"));

    public static readonly (Color, Color, Color) GreenFontColor = (new Color("e3ffe3"), new Color("00000030"),
        new Color("115411"));

    public static readonly (Color, Color, Color) BlueFontColor = (new Color("e3e3ff"), new Color("00000030"),
        new Color("111154"));

    // TODO VFX for gaining FireUp

    private Player? _player;

    private Control _rotationLayers = null!;

    private TextureRect _icon = null!;

    private ShaderMaterial _hsv = null!;

    private float _lerpingFireUpCount;

    private float _velocity1;

    private float _velocity2;

    private float _velocity3;

    private FireUp _displayedFireUpCount = new();

    private MegaLabel[] _labels = [];

    private Tween? _hsvTween;

    private bool _isListeningToCombatState;

    //private HoverTip _hoverTip;

    public void Initialize(Player player)
    {
        _player = player;
        ConnectFireUpChangedSignal();
        RefreshVisibility();
    }

    public override void _Ready()
    {
        _rotationLayers = GetNode<Control>("%RotationLayers");
        _icon = GetNode<TextureRect>("%Icon");
        _hsv = (ShaderMaterial)_icon.Material;
        // FireUp is a single resource (Charge), so a single label is shown.
        _labels =
        [
            CreateLabel(RedFontColor),
        ];
        GetNode<MarginContainer>("%MarginContainer1").AddChild(_labels[0]);
        //var locString = new LocString("static_hover_tips", "HELLKITE-FIREUP_COUNT.description");
        //locString.Add("fireup", 0);
        //_hoverTip = new HoverTip(new LocString("static_hover_tips", "HELLKITE-FIREUP_COUNT.title"), locString,
        //   HellkiteResource.FireUpIcon);
        //Connect(Control.SignalName.MouseEntered, Callable.From(OnHovered));
        //Connect(Control.SignalName.MouseExited, Callable.From(OnUnhovered));
        SetFireUpCountText(new FireUp(0), true);
        Visible = false;
    }

    private static MegaLabel CreateLabel((Color, Color, Color) fontColor)
    {
        var label = new MegaLabel();
        label.MaxFontSize = 28;
        label.AutoSizeEnabled = false;
        label.HorizontalAlignment = HorizontalAlignment.Center;
        label.VerticalAlignment = VerticalAlignment.Center;
        label.AddThemeColorOverride("font_color", fontColor.Item1);
        label.AddThemeColorOverride("font_shadow_color", fontColor.Item2);
        label.AddThemeColorOverride("font_outline_color", fontColor.Item3);
        label.AddThemeConstantOverride("shadow_offset_x", 3);
        label.AddThemeConstantOverride("shadow_offset_y", 3);
        label.AddThemeConstantOverride("outline_size", 15);
        label.AddThemeConstantOverride("shadow_outline_size", 15);
        // Uses the default theme font; the custom kreon font theme is not bundled.
        label.AddThemeFontSizeOverride("font_size", 28);
        label.Text = "0";

        return label;
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
        hellkiteCombatState?.FireUpChanged -= OnFireUpChanged;
        _isListeningToCombatState = false;
    }

    private void ConnectFireUpChangedSignal()
    {
        if (_player != null && !_isListeningToCombatState)
        {
            var hellkiteCombatState = _player.PlayerCombatState?.Hellkite();
            hellkiteCombatState?.FireUpChanged += OnFireUpChanged;
            _isListeningToCombatState = true;
        }
    }

    //private void OnHovered()
    //{
    //    var nHoverTipSet = NHoverTipSet.CreateAndShow(this, _hoverTip);
    //    nHoverTipSet?.GlobalPosition = GlobalPosition + new Vector2(-70, -240);
    //}
//
    //private void OnUnhovered()
    //{
    //    NHoverTipSet.Remove(this);
    //}

    private void OnFireUpChanged(FireUp oldFireUp, FireUp newFireUp)
    {
        UpdateFireUpCount(oldFireUp, newFireUp);
        RefreshVisibility();
    }

    private static readonly Color TranslucentColor = new("8080805a");

    private bool _isTranslucent;

    private void ToggleVisibility()
    {
        _isTranslucent = !_isTranslucent;
        Modulate = _isTranslucent ? TranslucentColor : Colors.White;
    }

    public override void _Process(double delta)
    {
        if (_player == null) return;
        var fireUp = GetPlayerFireUp(_player);
        var rotSpeed = fireUp.Total == 0 ? 10f : 40f;
        for (var i = 0; i < _rotationLayers.GetChildCount(); i++)
            _rotationLayers.GetChild<Control>(i).RotationDegrees += (float)delta * rotSpeed * (i + 1);

        _lerpingFireUpCount =
            MathHelper.SmoothDamp(_lerpingFireUpCount, fireUp.Charge, ref _velocity1, 0.1f, (float)delta);
    }

    public override void _GuiInput(InputEvent inputEvent)
    {
        if (inputEvent is not InputEventMouseButton eventMouseButton) return;
        var isClicked = eventMouseButton.ButtonIndex switch
        {
            MouseButton.Left or MouseButton.Right => true,
            _ => false
        };
        if (isClicked && inputEvent.IsReleased()) ToggleVisibility();
    }

    private static FireUp GetPlayerFireUp(Player player)
    {
        var fireUp = player.PlayerCombatState?.GetFireUp() ?? new FireUp();
        return fireUp;
    }

    private void UpdateFireUpCount(FireUp oldCount, FireUp newCount)
    {
        // FireUp should only go up or down together so there shouldn't be a case where 1 fireup go up and another go down 
        if (newCount.Total < oldCount.Total)
        {
            _hsvTween?.Kill();
            _hsv.SetShaderParameter(_v, 1f);
            _lerpingFireUpCount = newCount.Charge;
            SetFireUpCountText(newCount);
        }
        else if (newCount.Total > oldCount.Total)
        {
            _hsvTween?.Kill();
            _hsvTween = CreateTween();
            _hsvTween.TweenMethod(Callable.From<float>(UpdateShaderV), 2f, 1f, 0.2);
            //TODO vfx gain fireup
        }
    }

    private void SetFireUpCountText(FireUp fireUp, bool initSetup = false)
    {
        if (initSetup || _displayedFireUpCount != fireUp)
        {
            _displayedFireUpCount = fireUp;
            for (var i = 0; i < _labels.Length; i++)
            {
                var label = _labels[i];
                var elemValue = fireUp.ByIndex(i);
                var fontColor = i switch
                {
                    0 => RedFontColor.Item1,
                    1 => GreenFontColor.Item1,
                    2 => BlueFontColor.Item1,
                    _ => throw new ArgumentOutOfRangeException()
                };

                label.AddThemeColorOverride(ThemeConstants.Label.FontColor, elemValue == 0 ? StsColors.red : fontColor);
                label.SetTextAutoSize(elemValue.ToString());
            }

            if (fireUp.Total == 0)
            {
                _hsv.SetShaderParameter(_s, 0.5f);
                _hsv.SetShaderParameter(_v, 0.85f);
            }
            else
            {
                _hsv.SetShaderParameter(_s, 1f);
                _hsv.SetShaderParameter(_v, 1f);
            }
        }
    }

    private void UpdateShaderV(float value)
    {
        _hsv.SetShaderParameter(_v, value);
    }

    private void RefreshVisibility()
    {
        if (_player == null)
        {
            Visible = false;
            return;
        }

        var fireUp = GetPlayerFireUp(_player);

        var shouldAlwaysShowFireUp = _player.Character is Character.Hellkite;

        Visible = Visible || shouldAlwaysShowFireUp || fireUp.Total > 0;
    }
}