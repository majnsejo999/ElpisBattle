
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    public static UserData instance;
    public BaseDataAll baseData;
    public List<UserDataHero> data_hero;
    public LineUpController lineUpController;
    private void Awake()
    {
        if(instance == null)
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
            int k = Random.Range(0, 9);
            data1.listIdBody.Add(k);
        }
        data1.hero_origin = baseData.ListStatsBaseHeroe[data1.idHero].origin;
        data1.line = index;
        data_hero.Add(data1);
    }
    public void CreatNewDataEnemy(int index)
    {
        UserDataHero data1 = new UserDataHero();
        data1.idHero = Random.Range(0, 6);
        for (int i = 0; i < 9; i++)
        {
            int k = Random.Range(0, 9);
            data1.listIdBody.Add(k);
        }
        data1.hero_origin = baseData.ListStatsBaseHeroe[data1.idHero].origin;
        data1.line = index;
        data1.isEnemy = true;
        data_hero.Add(data1);
    }

    public void SwapLine(int index , int newline)
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
    }
}