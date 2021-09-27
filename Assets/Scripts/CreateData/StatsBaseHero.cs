using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StatsBaseHero : ScriptableObject
{
    public int id;
    public string origin;
    public float attack;
    public float hp;
    public float armour;
    public float speed;
    public float attack_lv;
    public float hp_lv;
    public float armour_lv;
    public float speed_lv;
    public float attack_ev;
    public float hp_ev;
    public float armour_ev;
    public float speed_ev;
}