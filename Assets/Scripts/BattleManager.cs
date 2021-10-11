using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Spine.Unity;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public HeroManager heroPrefabs;
    public List<HeroManager> list1, list2, listRound;
    public List<LineUp> lineHero, lineEnemy;
    public HeroManager hero_beaten;
    private int indexTurn, indexRound = 1;
    public BaseDataAll baseData;
    public GetAvatar getAvatar;
    public UIManager uIManager;
    public Dictionary<string, List<GameObject>> listPool;
    public GameObject txtClone;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        listPool = new Dictionary<string, List<GameObject>>();
    }
    public void InitBattle()
    {
        indexRound = 0;
        uIManager.txtRound.text = "Round : " + indexRound;
        listRound = new List<HeroManager>();
        for (int i = 0; i < 8; i++)
        {
            HeroManager heroClone = Instantiate(heroPrefabs, transform);
            if (i < 4)
            {
                heroClone.idHero = Random.Range(0, 6);
                heroClone.line = i;
                heroClone.baseData = baseData;
                list1.Add(heroClone);
                listRound.Add(heroClone);
                heroClone.transform.position = lineHero[heroClone.line].posHero.position;
                heroClone.Init();
            }
            else
            {
                heroClone.idHero = Random.Range(0, 6);
                heroClone.line = i - 3;
                heroClone.baseData = baseData;
                heroClone.isEnemy = true;
                list2.Add(heroClone);
                listRound.Add(heroClone);
                heroClone.transform.position = lineEnemy[heroClone.line].posHero.position;
                heroClone.Init();
            }

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
        indexRound += 1;
        uIManager.txtRound.text = "Round : " + indexRound;
        listRound = listRound.OrderByDescending(o => o.statHero.hero_speed).ToList();
        indexTurn = 0;
        listRound[indexTurn].objSelected.SetActive(true);
        CheckCanAttack(listRound[indexTurn]);
        //for (int i = 0; i < listRound.Count; i++)
        //{
        //    listRound[i].skeletonAnimation.gameObject.layer = 8;
        //    getAvatar.gameObject.transform.position = listRound[i].posAva.transform.position;
        //    listRound[i].sprAva = getAvatar.Avatar("hero" + i);
        //    uIManager.SetAva(listRound[i].sprAva, i);
        //    if (listRound[i].isEnemy)
        //        listRound[i].skeletonAnimation.gameObject.layer = 7;
        //    else
        //        listRound[i].skeletonAnimation.gameObject.layer = 6;

        //    if (!uIManager.imgAva[i].gameObject.activeInHierarchy)
        //    {
        //        uIManager.imgAva[i].gameObject.SetActive(true);
        //    }
        //}
        for (int i = 0; i < listRound.Count; i++)
        {
            uIManager.SetAva(baseData.baseBodyPartAnim[listRound[i].idHero].icon, i);
            if (!uIManager.imgAva[i].gameObject.activeInHierarchy)
            {
                uIManager.imgAva[i].gameObject.SetActive(true);
            }
        }
    }
    public void CheckCanAttack(HeroManager _hero)
    {
        uIManager.ChangeUIAttack(_hero);
        if (_hero.isSkill)
        {

        }
        else
        {
            if (_hero.listSkill[0].statsSkillNormals.require == "min_hp")
            {
                if (_hero.isEnemy)
                {
                    List<HeroManager> heroClone = list1.OrderBy(o => o.statHero.hero_hp).ToList();
                    heroClone[0].objCanAttck.SetActive(true);
                }
                else
                {
                    List<HeroManager> heroClone = list2.OrderBy(o => o.statHero.hero_hp).ToList();
                    heroClone[0].objCanAttck.SetActive(true);
                }
            }
            else
            {
                if (_hero.isEnemy)
                {
                    if (CheckEnemyStandFront(_hero))
                    {
                        Invoke("ShowTextEndTurn", 1f);
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
                        Invoke("ShowTextEndTurn", 1f);
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
        uIManager.imgAva[indexTurn].gameObject.SetActive(false);
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
        Vector3 posEnd = new Vector3();
        if (listRound[indexTurn].hero_origin == "Elf" || listRound[indexTurn].hero_origin == "Dwarf")
        {
            posEnd = listRound[indexTurn].gameObject.transform.position;
        }
        else
        {
            if (hero_beaten.isEnemy)
            {
                posEnd = lineEnemy[hero_beaten.line].posAttack.position;
            }
            else
            {
                posEnd = lineHero[hero_beaten.line].posAttack.position;
            }
        }
        listRound[indexTurn].transform.DOMove(posEnd, 0.2f, false).OnComplete(delegate
        {
            //  listRound[0].animator.Play(ConstData.AnimHeroAttack, 0, 0);
            // StartCoroutine(Attack());
            Attack();
        });
    }
    public void Hit()
    {
        CheckDamge();
    }
    public void Attack()
    {
        listRound[indexTurn].ChangeMana(30);
        string animAttack = listRound[indexTurn].hero_origin != "Naga" ? "attack" : "skill";
        listRound[indexTurn].skeletonAnimation.AnimationState.SetAnimation(0, animAttack, false).Complete += delegate
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
            listRound[indexTurn].transform.DOMove(posEnd, 0.3f, false).OnComplete(delegate
            {
                NextTurn();
            });
        };
    }
    public void CheckDamge()
    {
        float critDame = CheckCrit() ? listRound[indexTurn].statHero.crit_dame : 1;
        float dameReduction = hero_beaten.statHero.dame_reduction * listRound[indexTurn].statHero.hero_attack;
        float armor = hero_beaten.statHero.hero_armour;
        float armorMultiplier = 1 - (ConstData.Dame_rate * armor / (1 + ConstData.Dame_rate * Mathf.Abs(armor)));
        float damge = (listRound[indexTurn].statHero.hero_attack * critDame - dameReduction) * armorMultiplier;
        string effectSkill = listRound[indexTurn].listSkill[0].statsSkillNormals.effectSkill[0].effect;
        float dameEffect = listRound[indexTurn].listSkill[0].statsSkillNormals.effect_dame[0];
        if (!CheckEffectDame())
            hero_beaten.BurnHp(damge);
        else
            hero_beaten.BurnHp(damge, effectSkill, dameEffect);
    }
    public bool CheckCrit()
    {
        float critRate = listRound[indexTurn].statHero.crit_rate * 100;
        if (critRate == 0)
        {
            return false;
        }
        else
        {
            float a = Random.Range(0, 100);
            if (a <= critRate)
            {
                return true;
            }
            return false;
        }
    }
    public void RemoveHero(HeroManager heroDie)
    {
        int k = listRound.FindIndex(x => x == heroDie);
        listRound.Remove(heroDie);
        if (heroDie.isEnemy)
        {
            list2.Remove(heroDie);
        }
        else
        {
            list1.Remove(heroDie);
        }
        if (indexTurn >= k)
        {
            indexTurn -= 1;
        }
    }
    public bool CheckEffectDame()
    {
        float effectDameRate = listRound[indexTurn].listSkill[0].statsSkillNormals.effectSkill[0].rate[0] * 100;
        if (effectDameRate == 0)
        {
            return false;
        }
        else
        {
            float a = Random.Range(0, 100);
            if (a <= effectDameRate)
            {
                return true;
            }
            return false;
        }
    }

    public void SwapAttackSkill(bool useSkill)
    {
        if (useSkill)
        {
            if (listRound[indexTurn].statHero.hero_mana < listRound[indexTurn].listSkill[0].mana)
                return;
            if (listRound[indexTurn].isSkill)
                return;
            else
            {
                uIManager.img_skill1.color = Color.red;
                uIManager.img_attack.color = Color.white;
                listRound[indexTurn].isSkill = true;
                CheckCanAttack(listRound[indexTurn]);
            }
        }
        else
        {
            if (listRound[indexTurn].statHero.hero_mana >= listRound[indexTurn].listSkill[0].mana)
            {
                uIManager.img_attack.color = Color.red;
                uIManager.img_skill1.color = Color.blue;
            }
            else
            {
                uIManager.img_skill1.color = Color.white;
                uIManager.img_attack.color = Color.red;
            }
            listRound[indexTurn].isSkill = false;
            CheckCanAttack(listRound[indexTurn]);
        }
    }
    public void ShowTextEndTurn()
    {
        GameObject go = Pool(txtClone, transform);
        TextMeshPro textMeshPro = go.GetComponent<TextMeshPro>();
        go.transform.position = Vector3.zero;
        textMeshPro.text = "Hero can't attack";
        //textMeshPro.autoSizeTextContainer = true;
        textMeshPro.rectTransform.pivot = new Vector2(0.5f, 0);

        textMeshPro.alignment = TextAlignmentOptions.Bottom;
        textMeshPro.fontSize = 15;
        textMeshPro.enableKerning = false;
        textMeshPro.sortingOrder = 50;
        textMeshPro.color = new Color32(255, 255, 0, 255);

        Vector3 posEnd = new Vector3(go.transform.position.x, go.transform.position.y + 2, go.transform.position.z);
        go.transform.DOMove(posEnd, 1f, false).OnComplete(delegate
        {
            listRound[indexTurn].objSelected.SetActive(false);
            NextTurn ();
            //textMeshPro.autoSizeTextContainer = false;
            DePool(go);
        });
    }
    public GameObject Pool(GameObject obj, Transform trs)
    {
        GameObject objReturn;

        if (listPool.ContainsKey(obj.name))
        {
            if (listPool[obj.name].Count > 0)
            {
                objReturn = listPool[obj.name][0];
                objReturn.transform.SetParent(trs);
                objReturn.SetActive(true);
                listPool[obj.name].RemoveAt(0);
            }
            else
            {
                objReturn = Instantiate(obj, trs);
                objReturn.name = obj.name;
            }
        }
        else
        {
            objReturn = Instantiate(obj, trs);
            objReturn.name = obj.name;
        }

        if (!objReturn.activeSelf)
        {
            obj.SetActive(true);
        }

        return objReturn;
    }

    public void DePool(GameObject obj)
    {
        if (obj.activeSelf)
        {
            if (listPool.ContainsKey(obj.name))
            {
                listPool[obj.name].Add(obj);
            }
            else
            {
                listPool.Add(obj.name, new List<GameObject> { obj });
            }

            obj.SetActive(false);
        }
    }
}