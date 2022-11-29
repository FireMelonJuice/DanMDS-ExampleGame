using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSystem : MonoBehaviour
{
    private List<Monster> monsters = new List<Monster>();

    // Start is called before the first frame update
    void Start()
    {
        monsters.AddRange(GetComponentsInChildren<Monster>(true));
        InvokeRepeating("RegenMonsters", 7.5f, 7.5f);
    }

    void RegenMonsters()
    {
        foreach(var monster in monsters)
        {
            monster.Regen();
        }
    }
}
