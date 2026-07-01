using Hellkite.HellkiteCode.Extensions;
using Hellkite.HellkiteCode.Nodes;
using Hellkite.HellkiteCode.Structs;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hellkite.HellkiteCode.Hooks;

public static class HellkiteHook
{
    private static async Task Dispatch<T>(ICombatState combatState, Func<T, Task> action) where T : class
	{
		foreach (T item in combatState.IterateHookListeners().OfType<T>())
		{
			AbstractModel abstractModel = item as AbstractModel;
			await action(item);
			abstractModel.InvokeExecutionFinished();
		}
	}

	private static async Task Dispatch<T>(ICombatState combatState, Func<T, Task> action, IEnumerable<AbstractModel> filter) where T : class
	{
		foreach (T item in combatState.IterateHookListeners().OfType<T>().Intersect(filter.OfType<T>()))
		{
			AbstractModel abstractModel = item as AbstractModel;
			await action(item);
			abstractModel.InvokeExecutionFinished();
		}
	}

	private static async Task Dispatch<T>(ICombatState combatState, PlayerChoiceContext choiceContext, Func<T, Task> action) where T : class
	{
		foreach (T item in combatState.IterateHookListeners().OfType<T>())
		{
			AbstractModel abstractModel = item as AbstractModel;
			choiceContext.PushModel(abstractModel);
			await action(item);
			abstractModel.InvokeExecutionFinished();
			choiceContext.PopModel(abstractModel);
		}
	}

	private static TResult Aggregate<T, TResult>(ICombatState combatState, TResult seed, Func<T, TResult, TResult> action) where T : class
	{
		return combatState.IterateHookListeners().OfType<T>().Aggregate(seed, (TResult curr, T model) => action(model, curr));
	}

	public static FireUp ModifyFireUpGain(ICombatState combatState, Player player, FireUp originalAmount,
		ValueProp props, CardModel? cardSource,
		out IEnumerable<AbstractModel> modifiers)
	{
		var modifyingModels = new List<AbstractModel>();
		var res = Aggregate<IModifyFireUpGain, FireUp>(combatState, originalAmount, (model, current) =>
		{
			var next = model.ModifyFireUpGain(player, current, props, cardSource);
			if (next != current) modifyingModels.Add((AbstractModel)model);
			return next;
		});
		modifiers = modifyingModels;
		return res;
	}

	public static Task AfterModifyingFireUpGain(ICombatState combatState, IEnumerable<AbstractModel> modifiers)
	{
		return Dispatch<IAfterModifyingFireUpGain>(combatState,
			model => model.AfterModifyingFireUpGain());
	}

	public static Task AfterFireUpGained(ICombatState combatState, FireUp amount, Player player,
		CardPlay? cardPlay = null)
	{
		return Dispatch<IAfterFireUpGained>(combatState,
			model => model.AfterFireUpGained(combatState, amount, player, cardPlay));
	}

	public static FireUp ModifyFireUpCost(ICombatState combatState, CardModel card, FireUp originalCost)
	{
		if (originalCost.Total < 0) return originalCost;
		
		var modifiedCost = originalCost;
		(_, modifiedCost) = TryModifyFireUpCost(combatState, card, modifiedCost);

		return modifiedCost;
	}

	public static (bool, FireUp) TryModifyFireUpCost(ICombatState combatState, CardModel card,
		FireUp modifiedCost)
	{
		var isModified = false;
		foreach (var model in combatState.IterateHookListeners())
			switch (model)
			{
				case IModifyFireUpCost modifyFireUpCost:
					isModified |= modifyFireUpCost.TryModifyFireUpCost(card, modifiedCost, out modifiedCost);
					break;
				case BrilliantScarf scarf:
					isModified |= scarf.TryModifyFireUpCost(card, modifiedCost, out modifiedCost);
					break;
				case VoidFormPower voidForm:
					isModified |= voidForm.TryModifyFireUpCost(card, modifiedCost, out modifiedCost);
					break;
			}

		return (isModified, modifiedCost);
	}

	public static Task AfterFireUpSpent(ICombatState combatState, FireUp amount, Player spender)
	{
		return Dispatch<IAfterFireUpSpent>(combatState, model => model.AfterFireUpSpent(amount, spender));
	}

	public static Task AfterEnterOvercharge(ICombatState combatState)
	{
		return Dispatch<IAfterEnterOvercharge>(combatState, model => model.AfterEnterOvercharge());
	}

	public static Task AfterScorchTriggered(ICombatState combatState, Creature scorchedTarget)
	{
		return Dispatch<IAfterScorchTriggered>(combatState, model => model.AfterScorchTriggered(scorchedTarget));
	}
	
	public static decimal ModifyCharge(ICombatState combatState, Player player, decimal charge, ValueProp props,
		CardModel? cardSource, CardPlay? cardPlay, out IEnumerable<AbstractModel> modifiers)
	{
		var modifyingModels = new List<AbstractModel>();
		var res = Aggregate<IModifyCharge, decimal>(combatState, charge, (model, current) =>
		{
			var next = model.ModifyCharge(player, current, props, cardSource);
			if (next != current) modifyingModels.Add((AbstractModel)model);
			return next;
		});
		modifiers = modifyingModels;
		return res;
	}

	public static Task AfterModifyingCharge(ICombatState combatState, IEnumerable<AbstractModel> modifiers)
	{
		return Dispatch<IAfterModifyingCharge>(combatState,
			model => model.AfterModifyingCharge());
	}
}