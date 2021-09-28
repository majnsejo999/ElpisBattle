using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StatsSkillHero : ScriptableObject
{
    public int id;
    public string skill_name;
    public string rality;
    public string type_skill;
    public string detail;
    public List<string> origin;
    public int mana;
    public Sprite spriteSkill;
    public StatsSkill statsSkillNormals;
}
[System.Serializable]
public class StatsSkill
{
    public List<float> target;
    public string require;
    public string type_damge;
    public List<float> damge;
    public List<StatsEffectSkill> effectSkill;
    public float round;
    public float max_stack;
    public List<float> effect_dame;
    public StatsSkill()
    {
        target = new List<float>();
        require = string.Empty;
        type_damge = string.Empty;
        damge = new List<float>();
        effectSkill = new List<StatsEffectSkill>();
        round = 0;
        max_stack = 0;
        effect_dame = new List<float>();
    }
}
[System.Serializable]
public class StatsEffectSkill
{
    public string effect;
    public List<float> rate;
    public StatsEffectSkill()
    {
        effect = string.Empty;
        rate = new List<float>();
    }
}