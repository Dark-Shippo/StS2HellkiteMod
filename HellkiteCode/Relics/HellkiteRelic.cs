using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Hellkite.HellkiteCode.Character;
using Hellkite.HellkiteCode.Extensions;

namespace Hellkite.HellkiteCode.Relics;

/// <summary>
/// This is the base class for your mod's relics, which is set up to load the relic's images from your mod's resources.
/// When creating a relic, right click the Relics folder and create a new file with the Custom Relic template.
/// This will generate a class that extends this one.
/// You can also just create the class manually; just make sure to inherit from this class.
///
/// The [Pool] annotation marks this relic as being tied to your specific character. Inheriting from this class means
/// that your relics don't need to invidually say which pool they should be in.
/// </summary>
[Pool(typeof(HellkiteRelicPool))]
public abstract class HellkiteRelic : CustomRelicModel
{
    // Image path overrides intentionally omitted so relics fall back to the base
    // CustomRelicModel default (placeholder) art until custom relic images are added.
}