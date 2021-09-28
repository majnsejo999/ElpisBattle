using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public List<HeroManager> list1, list2, listRound;
    public List<LineUp> lineHero, lineEnemy;
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
        CheckNewRound();
    }
    public enum BattleStats
    {
        None, Auto, Pause, End, Wait
    }
    public void CheckNewRound()
    {
        listRound = listRound.OrderByDescending(o => o.hero_speed).ToList();
        listRound[0].heroSelected.SetActive(true);
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
                        RaycastHit hit;
                        if (!Physics.Linecast(transform.position, list1[i].transform.position,out hit))
                        {
                            list1[i].heroCanAttck.SetActive(true);
                        }
                        else
                        {
                            Debug.Log(hit.transform.gameObject.name);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < list2.Count; i++)
                    {
                        RaycastHit hit;
                        if (!Physics.Linecast(transform.position, list2[i].transform.position,out hit))
                        {
                            list2[i].heroCanAttck.SetActive(true);
                        }
                        else
                        {
                            Debug.Log(hit.transform.gameObject.name);
                        }
                    }
                }
            }
        }
    }
}