using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldObserver : Observer
{
    public GameManager GameManagerRef;
    public GameObject CoinPrefab;

    // Gold amounts
    public int EvolutionGoldReward = 0;
    public int BattleWinGoldReward = 0;
    public int LevelUpGoldReward = 0;
    public int LogInGoldReward = 0;

    // Receives info on events from subjects
    public override void OnNotify(GameObject GO, Events _event)
    {
        // Check event and trigger relevant quest
        switch (_event)
        {
            case Events.ItemUsed:
                if (GO.name.Contains("Fireball"))
                {
                    // Used fireball
                    SpawnGoldCoins(GO.GetComponent<Item>().GoldToGive);
                }
                else if (GO.name.Contains("Boulder"))
                {
                    // Used Boulder
                    SpawnGoldCoins(GO.GetComponent<Item>().GoldToGive);
                }
                else if (GO.name.Contains("Vine"))
                {
                    // Used Vines
                    SpawnGoldCoins(GO.GetComponent<Item>().GoldToGive);
                }
                else if (GO.name.Contains("Storm"))
                {
                    // Used Storm Orb
                    SpawnGoldCoins(GO.GetComponent<Item>().GoldToGive);
                }
                else if (GO.name.Contains("Waterfall"))
                {
                    // Used Waterfall
                    SpawnGoldCoins(GO.GetComponent<Item>().GoldToGive);
                }
                else if (GO.name.Contains("Football"))
                {
                    // Used Football
                    SpawnGoldCoins(GO.GetComponent<Item>().GoldToGive);
                }
                else if (GO.name.Contains("Balloon"))
                {
                    // Used Balloon
                    SpawnGoldCoins(GO.GetComponent<Item>().GoldToGive);
                }
                else if (GO.name.Contains("Trampoline"))
                {
                    // Used Trampoline
                    SpawnGoldCoins(GO.GetComponent<Item>().GoldToGive);
                }
                else
                {
                    Debug.Log("Error Item not found (GoldObserver)");
                }
                break;

            case Events.Evolve:
                // Evolved
                SpawnGoldCoins(EvolutionGoldReward);
                break;

            case Events.BattleWon:
                // Won battle
                SpawnGoldCoins(BattleWinGoldReward);
                break;

            case Events.LevelUp:
                // Leveled up
                SpawnGoldCoins(LevelUpGoldReward);
                break;

            case Events.LogIn:
                SpawnGoldCoins(LogInGoldReward);
                break;

            default:
                break;
        }
    }

    void SpawnGoldCoins(int amountToSpawn)
    {
        for(int i = 0; i < amountToSpawn; i++)
        {
            GetComponent<SpawnObject>().Spawn(CoinPrefab);
        }
    }
}
