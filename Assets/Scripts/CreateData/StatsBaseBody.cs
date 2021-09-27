using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StatsBaseBody : ScriptableObject
{
    public int id;
    public string origin;
    public float attack;
    public float hp;
    public float armour;
    public float speed;
}