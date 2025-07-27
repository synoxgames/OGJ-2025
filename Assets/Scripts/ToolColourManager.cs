using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// keeps track of which tools are unlocked
public static class ToolColourManager
{
    // a list of tools that are unlocked
    static HashSet<string> unlockedTools = new HashSet<string>();
    static HashSet<string> unlockedColours = new HashSet<string>();

    // set a tool to be unlocked
    public static void UnlockTool(string name)
    {
        unlockedTools.Add(name);
    }

    // set a colour to be unlocked
    public static void UnlockColour(string name)
    {
        unlockedColours.Add(name);
    }

    // checks if a tool or colour is unlocked
    public static bool IsUnlocked(string name)
    {
        if (unlockedTools.Contains(name) || unlockedColours.Contains(name))
        {
            return true;
        }
        return false;
    }
}
