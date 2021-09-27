using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    //def
    public List<HeroManager> list1, list2, listRound;

    private void Start()
    {
        listRound = new List<HeroManager>();
        for (int i = 0; i < list1.Count; i++)
        {
            listRound.Add(list1[i]);
        }
        for (int i = 0; i < list2.Count; i++)
        {
            listRound.Add(list2[i]);
        }
        CheckNewRound();
    }
    public enum BattleStats
    {
        None, Auto, Pause, End, Wait
    }

    void CheckNewRound()
    {
        listRound = listRound.OrderBy(o => o.hero_speed).ToList();
    }
}
