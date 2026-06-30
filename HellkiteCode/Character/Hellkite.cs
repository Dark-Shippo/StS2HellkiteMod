using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using Hellkite.HellkiteCode.Extensions;
using Godot;
using Hellkite.HellkiteCode.Cards.Basic;
using Hellkite.HellkiteCode.Relics;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

namespace Hellkite.HellkiteCode.Character;

public class Hellkite : PlaceholderCharacterModel
{
    public const string CharacterId = "Hellkite";

    public static readonly Color Color = new("ffffff");

    public override Color NameColor => Color;
    
    public override CharacterGender Gender => CharacterGender.Feminine;
    
    public override int StartingHp => 75;
    
    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikeHellkite>(),
        ModelDb.Card<StrikeHellkite>(),
        ModelDb.Card<StrikeHellkite>(),
        ModelDb.Card<StrikeHellkite>(),
        ModelDb.Card<DefendHellkite>(),
        ModelDb.Card<DefendHellkite>(),
        ModelDb.Card<DefendHellkite>(),
        ModelDb.Card<DefendHellkite>(),
        ModelDb.Card<FirePunch>(),
        ModelDb.Card<StokeScales>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<InnerSpark>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<HellkiteCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<HellkiteRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<HellkitePotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder base game assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }

    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
}