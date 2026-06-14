using BaseLib.Abstracts;
using BaseLib.Utils;
using Hellkite.HellkiteCode.Character;

namespace Hellkite.HellkiteCode.Potions;

[Pool(typeof(HellkitePotionPool))]
public abstract class HellkitePotion : CustomPotionModel;