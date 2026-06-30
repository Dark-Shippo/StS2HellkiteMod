using Hellkite.HellkiteCode.DynamicVars;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SmartFormat.Core.Extensions;
using static System.Int32;

namespace Hellkite.HellkiteCode.Formatters;

public class FireUpIconsFormatter
{
    // Prefixed formatter name to prevent conflict with other mods
    public string Name { get; set; } = "hlkIcon";
    public bool CanAutoDetect { get; set; } = false;

    public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
    {
        int amount;
        string iconText;
        switch (formattingInfo.CurrentValue)
        {
            case FireUpVar fireUpVar:
                amount = Convert.ToInt32(fireUpVar.PreviewValue);
                iconText = "[img]res://Hellkite/images/charui/big_energy.png[/img]";
                break;
            case DynamicVar dynVar:
                amount = Convert.ToInt32(dynVar.PreviewValue);
                iconText = GetFireUpIcon(formattingInfo.FormatterOptions);
                break;
            case int value:
                amount = value;
                iconText = GetFireUpIcon(formattingInfo.FormatterOptions);
                break;
            case decimal value:
                amount = (int)value;
                iconText = GetFireUpIcon(formattingInfo.FormatterOptions);
                break;
            case null:
                amount = 0;
                iconText = GetFireUpIcon(formattingInfo.FormatterOptions);
                break;
            default:
                throw new LocException(
                    $"Unknown value='{formattingInfo.CurrentValue}' type={formattingInfo.CurrentValue?.GetType()}");
        }

        if (formattingInfo.FormatterOptions.Contains('0')) amount = 0;

        var splitOpts = formattingInfo.FormatterOptions.Split(',');
        if (splitOpts.Length > 1)
            if (TryParse(splitOpts[1], out var newAmount))
                amount = newAmount;

        string finalText;
        if (amount <= 0)
            finalText = iconText;
        else if (formattingInfo.CurrentValue is DynamicVar dynamicVar)
            finalText = dynamicVar.ToHighlightedString(false) + " " + iconText;
        else
            finalText = $"{amount} {iconText}";

        formattingInfo.Write(finalText);

        return true;
    }

    private static string GetFireUpIcon(string format)
    {
        var name = format.Split(",").First();
        return name switch
        {
            "charge" => "[img]res://Hellkite/images/charui/big_energy.png[/img]",
            _ => throw new LocException($"Unknown value='{format}'")
        };
    }
}