using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Spine.Unity;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public List<HeroManager> list1, list2, listRound;
    public List<LineUp> lineHero, lineEnemy;
    public HeroManager hero_beaten;
    private int indexTurn;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        listRound = new List<HeroManager>();
        for (int i = 0; i < list1.Count; i++)
        {
            listRound.Add(list1[i]);
            list1[i].transform.position = lineHero[list1[i].line].posHero.position;
            list1[i].Init();
        }
        for (int i = 0; i < list2.Count; i++)
        {
            listRound.Add(list2[i]);
            list2[i].transform.position = lineEnemy[list2[i].line].posHero.position;
            list2[i].Init();
        }
        Invoke("CheckNewRound", 0.1f);
        // CheckNewRound();
    }
    public enum BattleStats
    {
        None, Auto, Pause, End, Wait
    }
    public void CheckNewRound()
    {
        listRound = listRound.OrderByDescending(o => o.hero_speed).ToList();
        indexTurn = 0;
        listRound[indexTurn].objSelected.SetActive(true);
        CheckCanAttack(listRound[indexTurn]);
    }
    public void CheckCanAttack(HeroManager _hero)
    {
        if (_hero.isSkill)
        {

        }
        else
        {
            if (_hero.listSkill[0].statsSkillNormals.require == "min_hp")
            {
                if (_hero.isEnemy)
                {
                    List<HeroManager> heroClone = list1.OrderBy(o => o.hero_hp).ToList();
                    heroClone[0].objCanAttck.SetActive(true);
                }
                else
                {
                    List<HeroManager> heroClone = list2.OrderBy(o => o.hero_hp).ToList();
                    heroClone[0].objCanAttck.SetActive(true);
                }
            }
            else
            {
                if (_hero.isEnemy)
                {
                    if (CheckEnemyStandFront(_hero))
                    {
                        _hero.objSelected.SetActive(false);
                        NextTurn();
                        return;
                    }
                    else
                    {
                        for (int i = 0; i < list1.Count; i++)
                        {
                            if (!CheckHeroStandFront(list1[i]))
                            {
                                list1[i].objCanAttck.SetActive(true);
                            }
                        }
                    }
                }
                else
                {
                    if (CheckHeroStandFront(_hero))
                    {
                        _hero.objSelected.SetActive(false);
                        NextTurn();
                        return;
                    }
                    else
                    {
                        for (int i = 0; i < list2.Count; i++)
                        {
                            if (!CheckEnemyStandFront(list2[i]))
                            {
                                list2[i].objCanAttck.SetActive(true);
                            }
                        }
                    }
                }
            }
        }
    }
    public void NextTurn()
    {
        indexTurn += 1;
        if (indexTurn >= listRound.Count)
        {
            indexTurn = 0;
            CheckNewRound();
        }
        else
        {
            listRound[indexTurn].objSelected.SetActive(true);
            CheckCanAttack(listRound[indexTurn]);
        }
    }
    public bool CheckHeroStandFront(HeroManager _hero)
    {
        if (lineHero[_hero.line].PosX == 2)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < list1.Count; i++)
            {
                if (lineHero[list1[i].line].PosX == 2 && lineHero[list1[i].line].PosY == lineHero[_hero.line].PosY)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public bool CheckEnemyStandFront(HeroManager _hero)
    {
        if (lineEnemy[_hero.line].PosX == 3)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < list2.Count; i++)
            {
                if (lineEnemy[list2[i].line].PosX == 3 && lineEnemy[list2[i].line].PosY == lineEnemy[_hero.line].PosY)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public void HeroAttack(HeroManager _hero)
    {
        hero_beaten = _hero;
        Vector3 posEnd = new Vector3();
        if (hero_beaten.isEnemy)
        {
            posEnd = lineEnemy[hero_beaten.line].posAttack.position;
        }
        else
        {
            posEnd = lineHero[hero_beaten.line].posAttack.position;
        }
        listRound[indexTurn].transform.DOMove(posEnd, 0.5f, false).OnComplete(delegate
        {
            //  listRound[0].animator.Play(ConstData.AnimHeroAttack, 0, 0);
            // StartCoroutine(Attack());
            Attack();
        });
    }
    public void Hit()
    {
        CheckDamge();
        hero_beaten.skeletonAnimation.AnimationState.SetAnimation(0, ConstData.AnimHeroHit, false).Complete += delegate
         {
             hero_beaten.skeletonAnimation.AnimationState.SetAnimation(0, ConstData.AnimHeroIdle, true);
         };
    }
    public void Attack()
    {
        listRound[indexTurn].skeletonAnimation.AnimationState.SetAnimation(0, ConstData.AnimHeroAttack, false).Complete += delegate
        {
            listRound[indexTurn].skeletonAnimation.AnimationState.SetAnimation(0, ConstData.AnimHeroIdle, true);
            Vector3 posEnd = new Vector3();
            if (listRound[indexTurn].isEnemy)
            {
                posEnd = lineEnemy[listRound[indexTurn].line].posHero.position;
            }
            else
            {
                posEnd = lineHero[listRound[indexTurn].line].posHero.position;
            }
            listRound[indexTurn].transform.DOMove(posEnd, 0.5f, false).OnComplete(delegate
            {
                for (int i = 0; i < list1.Count; i++)
                {
                    list1[i].objCanAttck.SetActive(false);
                    list1[i].objSelected.SetActive(false);
                }
                for (int i = 0; i < list2.Count; i++)
                {
                    list2[i].objCanAttck.SetActive(false);
                    list2[i].objSelected.SetActive(false);
                }
                NextTurn();
            });
        };
    }
    public void CheckDamge()
    {
        float critDame = 1;
        float blockDame = 0;
        float armor = hero_beaten.hero_armour;
        float armorMultiplier = 1 - (0.06f * armor / (1 + 0.06f * Mathf.Abs(armor)));
        float damge = (listRound[indexTurn].hero_attack * critDame - blockDame) * armorMultiplier;
        Debug.Log("damge : " + damge);
        hero_beaten.BurnHp(damge);
    }
}