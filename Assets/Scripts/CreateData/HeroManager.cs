using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using Spine.Unity.AttachmentTools;
using UnityEngine.U2D;
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
    public SkeletonAnimation skeletonAnimation;
    public GameObject objSelected, objCanAttck;
    public List<StatsSkillHero> listSkill;
    public bool isSkill;
    public bool isEnemy;
    public Material sourceMaterial;
    public void Init()
    {
        CreateRuntimeAssetsAndGameObject();
        AddSkill();
        objSelected.SetActive(false);
        objCanAttck.SetActive(false);
        CalculateStatsHero(baseData.ListStatsBaseHeroe[idHero]);
        gameObject.SetActive(true);
        if (isEnemy)
        {
            skeletonAnimation.GetComponent<MeshRenderer>().sortingOrder = line;
        }
        else
        {
            skeletonAnimation.GetComponent<MeshRenderer>().sortingOrder = line + 6;
        }
        skeletonAnimation.AnimationState.Event += HandleEvent;
    }
    void CreateRuntimeAssetsAndGameObject()
    {
        sourceMaterial = baseData.baseBodyPartAnim[idHero].materialPropertySource;
        SpineAtlasAsset runtimeAtlasAsset = SpineAtlasAsset.CreateRuntimeInstance(baseData.baseBodyPartAnim[idHero].atlasText, baseData.baseBodyPartAnim[idHero].textures, sourceMaterial, true);
        SkeletonDataAsset runtimeSkeletonDataAsset = SkeletonDataAsset.CreateRuntimeInstance(baseData.baseBodyPartAnim[idHero].skeletonJson, runtimeAtlasAsset, true);
        skeletonAnimation = SkeletonAnimation.NewSkeletonAnimationGameObject(runtimeSkeletonDataAsset);
        skeletonAnimation.transform.parent = gameObject.transform;
        skeletonAnimation.transform.localPosition = Vector3.zero;
        skeletonAnimation.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        if (isEnemy)
            skeletonAnimation.skeleton.FlipX = true;
        skeletonAnimation.Skeleton.SetSkin("skin_default");
        skeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
        skeletonAnimation.skeleton.SetToSetupPose();
        skeletonAnimation.skeleton.Update(0);
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
        var _skin2 = skeletonAnimation.Skeleton.Skin;
        for (int i = 0; i < listIdBody.Count; i++)
        {
            StatsBaseBody baseBody = baseData.ListStatsBaseBody[listIdBody[i]];
            hero_attack += baseBody.attack;
            hero_hp += baseBody.hp;
            hero_armour += baseBody.hp;
            hero_speed += baseBody.speed;
            sum_stats_body += (baseBody.attack + baseBody.hp + baseBody.hp + baseBody.speed);
            ChangSkin(baseData.baseBodyPartAnim[idHero].listBodyCharacter[i], baseData.baseBodyPartAnim[idHero].spriteAtlas[listIdBody[i] % 2], _skin2);
        }
        rality = 1 + (float)Mathf.Clamp((int)(sum_stats_body / 12) - 1, 0, 5) / 10;
        ClearSkin(_skin2);
    }
    public void ChangSkin(EquipHook bodyParts, SpriteAtlas spriteAtlas, Skin skin)
    {
        for (int i = 0; i < bodyParts.nameSlot.Length; i++)
        {
            //Debug.Log(bodyParts.nameSlot[i]);
            int slotIndex = skeletonAnimation.skeletonDataAsset.GetSkeletonData(true).FindSlot(bodyParts.nameSlot[i]).Index;
            Attachment attachment1 = skeletonAnimation.skeleton.GetAttachment(bodyParts.nameSlot[i], bodyParts.nameSlot[i]);
            Attachment attachment2 = attachment1.GetRemappedClone(spriteAtlas.GetSprite(bodyParts.nameSprite[i]), sourceMaterial);
            skin.SetAttachment(slotIndex, bodyParts.nameSlot[i], attachment2);
        }
    }

    private void OnMouseDown()
    {
        if (objCanAttck.activeInHierarchy)
        {
            BattleManager.instance.HeroAttack(gameObject.GetComponent<HeroManager>());
        }
    }
    public void ClearSkin(Skin _skinChange)
    {
        skeletonAnimation.skeleton.SetSkin(_skinChange);
        skeletonAnimation.skeleton.SetSlotsToSetupPose();
        skeletonAnimation.Update(0);
        AtlasUtilities.ClearCache();
    }
    [SpineEvent] public string footstepEventName = "attack";
    void HandleEvent(TrackEntry trackEntry, Spine.Event e)
    {
        // Play some sound if the event named "footstep" fired.
        if (e.Data.Name == footstepEventName)
        {
            BattleManager.instance.Hit();
        }
    }
}
