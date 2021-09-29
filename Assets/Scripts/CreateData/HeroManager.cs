using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using Spine.Unity.AttachmentTools;

public class HeroManager : MonoBehaviour
{
    [Header("Stats Hero")]
    public int idHero;
    public List<int> listIdBody;
    public string hero_origin;
    public float hero_attack;
    public float hero_hp;
    public float hero_armour;
    public float hero_speed;
    public float level;
    public float levelMax;
    public float index_evolution;
    public float exp;
    public float rality;
    public float sum_stats_body = 0;
    public int line;
    public BaseDataAll baseData;
    public SkeletonAnimation anim;
    public GameObject heroSelected, heroCanAttck;
    public List<StatsSkillHero> listSkill;
    public bool isSkill;
    public bool isEnemy;
    public Material sourceMaterial;
    [SpineSkin] public string DefaultSkinName = "skin_default";
    public void Init()
    {
        anim = this.GetComponent<SkeletonAnimation>();
        if (isEnemy)
        {
            anim.GetComponent<MeshRenderer>().sortingOrder = line;
        }
        else
        {
            anim.GetComponent<MeshRenderer>().sortingOrder = line + 6;
        }
        AddSkill();
        heroSelected.SetActive(false);
        heroCanAttck.SetActive(false);
        CalculateStatsHero(baseData.ListStatsBaseHeroe[idHero]);
        gameObject.SetActive(true);
    }
    public void AddSkill()
    {
        int a1 = Random.Range(0, 12);
        listSkill.Add(baseData.ListStatsSkillHero[a1]);
        int a2 = Random.Range(13, 34);
        listSkill.Add(baseData.ListStatsSkillHero[a2]);
        int a3 = Random.Range(13, 34);
        listSkill.Add(baseData.ListStatsSkillHero[a3]);
        int a4 = Random.Range(34, 54);
        listSkill.Add(baseData.ListStatsSkillHero[a4]);
    }
    public void CalculateStatsHero(StatsBaseHero statsBase)
    {
        hero_origin = statsBase.origin;
        SumStatsBody();
        hero_attack = statsBase.attack + statsBase.attack_lv * level + statsBase.attack_ev * index_evolution * rality;
        hero_hp = statsBase.hp + statsBase.hp_lv * level + statsBase.hp_ev * index_evolution * rality;
        hero_armour = statsBase.armour + statsBase.armour_lv * level + statsBase.armour_ev * index_evolution * rality;
        hero_speed = statsBase.speed + statsBase.speed_lv * level + statsBase.speed_ev * index_evolution * rality;
        AddStatsSkill(listSkill[1]);
        AddStatsSkill(listSkill[2]);
    }
    public void AddStatsSkill(StatsSkillHero skill)
    {
        switch (skill.statsSkillNormals.effectSkill[0].effect)
        {
            case "attck":
                hero_attack = hero_attack + (hero_attack * skill.statsSkillNormals.effectSkill[0].rate[0]);
                break;
            case "armor":
                hero_armour = hero_armour + (hero_armour * skill.statsSkillNormals.effectSkill[0].rate[0]);
                break;
            case "speed":
                hero_speed = hero_speed + (hero_speed * skill.statsSkillNormals.effectSkill[0].rate[0]);
                break;
            case "hp":
                hero_hp = hero_hp + (hero_hp * skill.statsSkillNormals.effectSkill[0].rate[0]);
                break;
        }
    }
    public void SumStatsBody()
    {
        var _skin2 = anim.Skeleton.Skin;
        for (int i = 0; i < listIdBody.Count; i++)
        {
            StatsBaseBody baseBody = baseData.ListStatsBaseBody[listIdBody[i]];
            hero_attack += baseBody.attack;
            hero_hp += baseBody.hp;
            hero_armour += baseBody.hp;
            hero_speed += baseBody.speed;
            sum_stats_body += (baseBody.attack + baseBody.hp + baseBody.hp + baseBody.speed);
            if (listIdBody[i] % 2 == 0)
            {
                ChangSkin(baseData.baseBodyPartAnim[0].listBodyCharacter[i]._sprite, _skin2);
            }
        }
        rality = 1 + (float)Mathf.Clamp((int)(sum_stats_body / 12) - 1, 0, 5) / 10;
        ClearSkin(_skin2);
    }
    public void ChangSkin(Sprite[] _sprite,Skin skin)
    {
        for (int i = 0; i < _sprite.Length; i++)
        {
            string nameSlot2 = _sprite[i].name;
            Debug.Log(nameSlot2);
            int slotIndex = anim.skeletonDataAsset.GetSkeletonData(true).FindSlot(nameSlot2).Index;
            Attachment attachment1 = anim.skeleton.GetAttachment(nameSlot2, nameSlot2);
            Attachment attachment2 = attachment1.GetRemappedClone(_sprite[i], sourceMaterial);
            skin.SetAttachment(slotIndex, nameSlot2, attachment2);
        }
    }

    public void ClearSkin(Skin _skinChange)
    {
        anim.skeleton.SetSkin(_skinChange);
        anim.skeleton.SetSlotsToSetupPose();
        anim.Update(0);
        AtlasUtilities.ClearCache();
    }
}
