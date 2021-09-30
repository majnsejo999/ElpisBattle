using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public List<HeroManager> list1, list2, listRound;
    public List<LineUp> lineHero, lineEnemy;
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
        listRound[0].objSelected.SetActive(true);
        CheckCanAttack(listRound[0]);
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
                        if (!Physics.Linecast(_hero.transform.position, list2[i].transform.position))
                        {
                            list2[i].objCanAttck.SetActive(true);
                        }
                    }
                }
            }
        }
    }

    public void HeroAttack(HeroManager hero_beaten)
    {
        Vector3 posEnd = new Vector3();
        if (hero_beaten.isEnemy)
        {
            posEnd = lineEnemy[hero_beaten.line].posAttack.position;
        }
        else
        {
            posEnd = lineHero[hero_beaten.line].posAttack.position;
        }
        listRound[0].transform.DOMove(posEnd, 0.5f, false).OnComplete(delegate
        {
            listRound[0].animator.Play(ConstData.AnimHeroAttack, 0, 0);
        });
    }
    public void HeroEndAttack()
    {
        listRound[0].animator.Play(ConstData.AnimHeroIdle, 0, 0);
        Vector3 posEnd = new Vector3();
        if (listRound[0].isEnemy)
        {
            posEnd = lineEnemy[listRound[0].line].posHero.position;
        }
        else
        {
            posEnd = lineHero[listRound[0].line].posHero.position;
        }
        listRound[0].transform.DOMove(posEnd, 0.5f, false).OnComplete(delegate
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
            listRound.Remove(listRound[0]);
            CheckNewRound();
        });
    }
}