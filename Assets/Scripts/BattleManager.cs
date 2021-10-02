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

            }
            else
            {
                if (_hero.isEnemy)
                {
                    for (int i = 0; i < list1.Count; i++)
                    {
                        RaycastHit2D h = Physics2D.Linecast(_hero.transform.position, list1[i].transform.position);
                        if (h.collider != null && h.collider.tag != "Enemy")
                        {
                            Debug.Log(h.collider.gameObject.name);
                        }
                        else
                        {
                            list1[i].objCanAttck.SetActive(true);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < list2.Count; i++)
                    {
                        RaycastHit2D h = Physics2D.Linecast(_hero.transform.position, list2[i].transform.position);
                        if (h.collider != null && h.collider.tag != "Hero")
                        {
                            Debug.Log(h.collider.gameObject.name);
                        }
                        else
                        {
                            list2[i].objCanAttck.SetActive(true);
                        }
                    }
                }
            }
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
            });
        };
    }
}