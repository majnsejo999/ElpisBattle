using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoBehaviour
{
    [Header("Stats Hero")]
    public int idHero;
    public List<int> listIdBody;
    public string hero_origin;
    public float hero_attack;
    public float hero_hp;
    public float hero_armour;
    public float hero_speed;
    public float level;
    public float levelMax;
    public float index_evolution;
    public float exp;
    public float rality;
    public float sum_stats_body = 0;
    public BaseDataAll baseData;
    private void Start()
    {
        CalculateStatsHero(baseData.ListStatsBaseHeroe[idHero]);
    }
    public void CalculateStatsHero(StatsBaseHero statsBase)
    {
        hero_origin = statsBase.origin;
        SumStatsBody();
        hero_attack = statsBase.attack + statsBase.attack_lv * level + statsBase.attack_ev * index_evolution * rality;
        hero_hp = statsBase.hp + statsBase.hp_lv * level + statsBase.hp_ev * index_evolution * rality;
        hero_armour = statsBase.armour + statsBase.armour_lv * level + statsBase.armour_ev * index_evolution * rality;
        hero_speed = statsBase.speed + statsBase.speed_lv * level + statsBase.speed_ev * index_evolution * rality;
    }
    public void SumStatsBody()
    {
        for (int i = 0; i < listIdBody.Count; i++)
        {
            StatsBaseBody baseBody = baseData.ListStatsBaseBody[listIdBody[i]];
            hero_attack += baseBody.attack;
            hero_hp += baseBody.hp;
            hero_armour += baseBody.hp;
            hero_speed += baseBody.speed;
            sum_stats_body += (baseBody.attack + baseBody.hp + baseBody.hp + baseBody.speed);
        }
        rality = 1 + (float)Mathf.Clamp((int)(sum_stats_body / 12) - 1, 0, 5) / 10;
    }
}
