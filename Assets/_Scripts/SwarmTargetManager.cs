using System.Collections.Generic;
using UnityEngine;

public class SwarmTargetManager
{
    private static int targetSwitcher = 0;
    public static List<SwarmTarget> Targets { get; set; } = new();
    public static SwarmTarget currentTarget { get; set; }

    public static void SwitchTarget()
    {
        targetSwitcher = (targetSwitcher + 1) % Targets.Count;
        currentTarget = Targets[targetSwitcher];
    }

    public static void Register(SwarmTarget target)
    {
        if (currentTarget == null)
        {
            currentTarget = target;
        }
        Targets.Add(target);
    }
}
