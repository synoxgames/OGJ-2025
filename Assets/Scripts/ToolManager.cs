using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// keeps track of which tools are unlocked
public static class ToolManager
{
    static HashSet<string> unlocked;

    public static void UnlockTool(string name)
    {
        if (unlocked == null)
        {
            unlocked = new HashSet<string>();
        }

        unlocked.Add(name);
    }

    public static bool IsUnlocked(string name)
    {
        if (unlocked == null)
        {
            unlocked = new HashSet<string>();
        }

        if (unlocked.Contains(name))
        {
            return true;
        }
        return false;
    }
}
