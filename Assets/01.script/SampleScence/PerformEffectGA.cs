using System.Collections.Generic;
using UnityEngine;

public class PerformEffectGA : GameAction
{
    public Effect Effect { get; set; }
    public List<CombatantView> Target {  get; set; }
    public PerformEffectGA(Effect effect,List<CombatantView> targets)
    {
        Effect = effect;
        Target = targets==null ? null : new(targets);
    }
}
