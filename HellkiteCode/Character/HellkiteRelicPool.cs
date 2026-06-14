using BaseLib.Abstracts;
using Hellkite.HellkiteCode.Extensions;
using Godot;

namespace Hellkite.HellkiteCode.Character;

public class HellkiteRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => Hellkite.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}