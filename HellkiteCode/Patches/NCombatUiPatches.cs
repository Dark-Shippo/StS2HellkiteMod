using BaseLib.Utils;
using Godot;
using HarmonyLib;
using Hellkite.HellkiteCode.Field;
using Hellkite.HellkiteCode.Nodes;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace Hellkite.HellkiteCode.Patches;

[HarmonyPatch(typeof(NCombatUi), nameof(NCombatUi.Activate))]
internal class NCombatUiPatches
{
    [HarmonyPostfix]
    private static void Postfix(NCombatUi __instance, CombatState state)
    {
        //IL_0036: Unknown result type (might be due to invalid IL or missing references)
        //IL_004a: Unknown result type (might be due to invalid IL or missing references)
        NFireUpCounter nFireUpCounter = ((NotNullSpireField<NCombatUi, NFireUpCounter>)(object)HellkiteNode.NFireUpCounter)[__instance];
        nFireUpCounter.Initialize(LocalContext.GetMe((ICombatState)(object)state));
        ((Node)nFireUpCounter).Reparent((Node)(object)__instance._energyCounter, true);
        ((CanvasItem)nFireUpCounter).ShowBehindParent = true;
        ((Control)nFireUpCounter).Position = new Vector2(0f, -120f);
        ((Control)nFireUpCounter).Size = new Vector2(128f, 128f);
    }
}