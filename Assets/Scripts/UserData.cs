
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    public static UserData instance;
    public BaseDataAll baseData;
    public List<UserDataHero> data_hero;
    public LineUpController lineUpController;
    public int leaderHero, leaderEnemy;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        data_hero = new List<UserDataHero>();
        for (int i = 0; i < 4; i++)
        {
            CreatNewDataHero(i);
        }
        for (int i = 0; i < 4; i++)
        {
            CreatNewDataEnemy(i);
        }
        lineUpController.InitHeroToLine();
    }
    public void CreatNewDataHero(int index)
    {
        UserDataHero data1 = new UserDataHero();
        data1.idHero = Random.Range(0, 6);
        for (int i = 0; i < 9; i++)
        {
            int k = Random.Range(0, 7);
            data1.listIdBody.Add(k);
        }
        data1.hero_origin = baseData.ListStatsBaseHeroe[data1.idHero].origin;
        data1.line = index;
        AddSkill(data1.skillDataHeroes);
        data_hero.Add(data1);
    }
    public void CreatNewDataEnemy(int index)
    {
        UserDataHero data1 = new UserDataHero();
        data1.idHero = Random.Range(0, 6);
        for (int i = 0; i < 9; i++)
        {
            int k = Random.Range(0, 7);
            data1.listIdBody.Add(k);
        }
        data1.hero_origin = baseData.ListStatsBaseHeroe[data1.idHero].origin;
        data1.line = index;
        data1.isEnemy = true;
        AddSkill(data1.skillDataHeroes);
        data_hero.Add(data1);
    }
    public void AddSkill(List<SkillDataHero> listSkill)
    {
        int a1 = Random.Range(0, 39);
        //int a1 = 36;
        SkillDataHero skillDataHero1 = new SkillDataHero();
        skillDataHero1.idSkill = baseData.ListStatsSkillHero[a1].id;
        skillDataHero1.levelSkill = 1;
        listSkill.Add(skillDataHero1);
        int a2 = Random.Range(40, 87);
        SkillDataHero skillDataHero2 = new SkillDataHero();
        skillDataHero2.idSkill = baseData.ListStatsSkillHero[a2].id;
        skillDataHero2.levelSkill = 1;
        listSkill.Add(skillDataHero2);
        int a3 = Random.Range(88, 119);
        SkillDataHero skillDataHero3 = new SkillDataHero();
        skillDataHero3.idSkill = baseData.ListStatsSkillHero[a3].id;
        skillDataHero3.levelSkill = 1;
        listSkill.Add(skillDataHero3);
        int a4 = Random.Range(120, 184);
        SkillDataHero skillDataHero4 = new SkillDataHero();
        skillDataHero4.idSkill = baseData.ListStatsSkillHero[a4].id;
        skillDataHero4.levelSkill = 1;
        listSkill.Add(skillDataHero4);
    }
    public void SwapLine(int index, int newline)
    {
        data_hero[index].line = newline;
    }
}
[System.Serializable]
public class UserDataHero
{
    public int idHero;
    public List<int> listIdBody;
    public string hero_origin;
    public float level;
    public float levelMax;
    public float index_evolution;
    public float exp;
    public int line;
    public bool isEnemy;
    public List<SkillDataHero> skillDataHeroes;
    public UserDataHero()
    {
        idHero = 0;
        listIdBody = new List<int>();
        hero_origin = string.Empty;
        isEnemy = false;
        level = 21;
        levelMax = 30;
        index_evolution = 2;
        exp = 1000;
        line = 0;
        skillDataHeroes = new List<SkillDataHero>();
    }
}
[System.Serializable]
public class SkillDataHero
{
    public int idSkill;
    public int levelSkill;
    public SkillDataHero()
    {
        idSkill = 0;
        levelSkill = 1;
    }
}