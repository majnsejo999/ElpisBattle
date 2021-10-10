using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public Image[] imgAva;
    public Image img_attack, img_skill1;
    public Text txtAttack;

    public void SetAva(Sprite _sprite, int id)
    {
        imgAva[id].sprite = _sprite;
    }

    public void ChangeUIAttack(HeroManager hero)
    {
        if(hero.listSkill[0].statsSkillNormals.effectSkill[0].effect == "min_hp")
        {
            txtAttack.text = "Attack min_hp";
        }
        else
        {
            txtAttack.text = "Attack";
        }

        if (hero.statHero.hero_mana >= hero.listSkill[3].mana)
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
}
