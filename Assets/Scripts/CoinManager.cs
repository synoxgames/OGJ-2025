using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

/* Currently : 
    CoinManager is initialized on gameLoad, 
    CoinManager keeps track of the coin count, but it is not persistent across game sessions.

    Future improvements:
        - Make coin count persistent across game sessions using playerrefs etc.
*/


public static class CoinManager
{
    private static int coinCount = 0; // Intial coin count

    // change the number of coins the player has
    public static void ChangeCoins(int change)
    {
        coinCount += change;
        Debug.Log("Coins added: " + change + ". Total coins: " + coinCount);
    }

    // returns how many coins the player has
    public static int GetCoinCount()
    {
        return coinCount;
    }
}
