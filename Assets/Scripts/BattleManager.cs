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
                        RaycastHit2D h = Physics2D.Linecast(_hero.transform.position, list1[i].transform.position);                    
                        if(h.collider != null && h.collider.tag != "Enemy")
                        {
                            Debug.Log(h.collider.gameObject.name);
                        }
                        else
                        {
                            list1[i].heroCanAttck.SetActive(true);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < list2.Count; i++)
                    {
                        if (!Physics.Linecast(_hero.transform.position, list2[i].transform.position))
                        {
                            list2[i].heroCanAttck.SetActive(true);
                        }
                    }
                }
            }
        }
    }
}