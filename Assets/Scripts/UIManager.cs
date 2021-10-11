using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public Image[] imgAva;
    public Image img_attack, img_skill1;
    public Text txtAttack, txtRound;

    public void SetAva(Sprite _sprite, int id)
    {
        imgAva[id].sprite = _sprite;
    }

    public void ChangeUIAttack(HeroManager hero, StatsSkillHero attack, StatsSkillHero skill)
    {
        if (attack.statsSkillNormals.require == "min_hp")
        {
            txtAttack.text = "Attack min_hp" +"\n" +attack.rality;
        }
        else if (attack.statsSkillNormals.effectSkill[0].effect != "none")
        {
            txtAttack.text = "Attack " + "\n" + attack.statsSkillNormals.effectSkill[0].effect + "\n" + attack.rality;
        }
        else
        {
            txtAttack.text = "Attack" + "\n" + attack.rality;
        }

        if (hero.statHero.hero_mana >= skill.mana)
        {
            if (hero.isSkill)
            {
                img_skill1.color = Color.red;
                img_attack.color = Color.white;
            }
            else
            {
                img_attack.color = Color.red;
                img_skill1.color = Color.blue;
            }

        }
        else
        {
            img_skill1.color = Color.white;
            img_attack.color = Color.red;
        }
    }

    public void Replay()
    {
        SceneManager.LoadScene("WorldMap");
    }
}
