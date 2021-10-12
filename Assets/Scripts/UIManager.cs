using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public List<Image> imgAva;
    public Image[] img_skill;
    public Text txtAttack, txtUltimate1, txtPassic1, txtPassic2, txtRound,txtDetailSkill;
    StatsSkillHero attack, passic1, passic2, ultimate;
    public GameObject obj_detailSkill;
    public void SetAva(Sprite _sprite, int id)
    {
        imgAva[id].sprite = _sprite;
        imgAva[id].gameObject.SetActive(true);
    }

    public void ChangeUIAttack(HeroManager hero)
    {
        attack = hero.baseData.ListStatsSkillHero[hero.dataHero.skillDataHeroes[0].idSkill];
        passic1 = hero.baseData.ListStatsSkillHero[hero.dataHero.skillDataHeroes[1].idSkill];
        passic2 = hero.baseData.ListStatsSkillHero[hero.dataHero.skillDataHeroes[2].idSkill];
        ultimate = hero.baseData.ListStatsSkillHero[hero.dataHero.skillDataHeroes[3].idSkill];
        txtAttack.text = attack.skill_name;
       // txtPassic1.text = passic1.statsSkillNormals.effectSkill[0].effect + "\n" + (passic1.statsSkillNormals.effectSkill[0].rate[0] * 100) + "%";
        txtPassic1.text = passic1.skill_name;
        txtPassic2.text = passic2.skill_name;
        txtUltimate1.text = ultimate.skill_name;
        if (hero.statHero.hero_mana >= ultimate.mana)
        {
            if (hero.isSkill)
            {
                img_skill[3].color = Color.red;
                img_skill[0].color = Color.white;
            }
            else
            {
                img_skill[0].color = Color.red;
                img_skill[3].color = Color.blue;
            }

        }
        else
        {
            img_skill[3].color = Color.white;
            img_skill[0].color = Color.red;
        }
    }
    public void ShowDetailSkill(int index)
    {
        obj_detailSkill.SetActive(true);
        obj_detailSkill.transform.position = img_skill[index].transform.position;
        if (index == 0)
        {
            txtDetailSkill.text = attack.detail;
        }
        else if(index == 1)
        {
            txtDetailSkill.text = passic1.detail;
        }
        else if (index == 2)
        {
            txtDetailSkill.text = passic2.detail;
        }
        else if (index == 3)
        {
            txtDetailSkill.text = ultimate.detail;
        }
    }
    public void HideDetailSkill()
    {
        obj_detailSkill.SetActive(false);
    }
    public void Replay()
    {
        SceneManager.LoadScene("WorldMap");
    }
}
