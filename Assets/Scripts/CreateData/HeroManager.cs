using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using Spine.Unity.AttachmentTools;
using UnityEngine.U2D;
using TMPro.Examples;
using TMPro;
using DG.Tweening;
using System.Linq;

public class HeroManager : MonoBehaviour
{
    [Header("Stats Hero")]
    public UserDataHero dataHero;
    public StatHero statHero;
    public float rality;
    public float sum_stats_body = 0;
    public SkeletonAnimation skeletonAnimation;
    public GameObject objSelected, objCanAttck, posAva, img_leader;
    public BaseDataAll baseData;
    public bool isSkill;
    public bool isEnemy;
    public bool isLeader;
    public Material sourceMaterial;
    public Sprite sprAva;
    public SpriteRenderer spriteHp, spriteMana;
    public List<EffectHit> effectHit,effectHits_start;
    public void Init(UserDataHero _dataHero)
    {
        dataHero = _dataHero;
        effectHit = new List<EffectHit>();
        effectHits_start = new List<EffectHit>();
        CreateRuntimeAssetsAndGameObject();
        objSelected.SetActive(false);
        objCanAttck.SetActive(false);
        CalculateStatsHero(baseData.ListStatsBaseHeroe[dataHero.idHero]);
        gameObject.SetActive(true);
        if (isEnemy)
        {
            skeletonAnimation.GetComponent<MeshRenderer>().sortingOrder = dataHero.line;
        }
        else
        {
            skeletonAnimation.GetComponent<MeshRenderer>().sortingOrder = dataHero.line + 6;
        }
        skeletonAnimation.AnimationState.Event += HandleEvent;
        if (isLeader)
            img_leader.SetActive(true);
        else
            img_leader.SetActive(false);
    }
    void CreateRuntimeAssetsAndGameObject()
    {
        // Debug.Log(idHero);
        sourceMaterial = baseData.baseBodyPartAnim[dataHero.idHero].materialPropertySource;
        SpineAtlasAsset runtimeAtlasAsset = SpineAtlasAsset.CreateRuntimeInstance(baseData.baseBodyPartAnim[dataHero.idHero].atlasText, baseData.baseBodyPartAnim[dataHero.idHero].textures, sourceMaterial, true);
        SkeletonDataAsset runtimeSkeletonDataAsset = SkeletonDataAsset.CreateRuntimeInstance(baseData.baseBodyPartAnim[dataHero.idHero].skeletonJson, runtimeAtlasAsset, true);
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

    public void CalculateStatsHero(StatsBaseHero statsBase)
    {
        dataHero.hero_origin = statsBase.origin;
        SumStatsBody();
        statHero.hero_attack = statsBase.attack + statsBase.attack_lv * dataHero.level + statsBase.attack_ev * dataHero.index_evolution * rality;
        statHero.hero_hp = statsBase.hp + statsBase.hp_lv * dataHero.level + statsBase.hp_ev * dataHero.index_evolution * rality;
        statHero.hero_armour = statsBase.armour + statsBase.armour_lv * dataHero.level + statsBase.armour_ev * dataHero.index_evolution * rality;
        statHero.hero_speed = statsBase.speed + statsBase.speed_lv * dataHero.level + statsBase.speed_ev * dataHero.index_evolution * rality;
        statHero.hero_attack *= ConstData.Bonus_Attack;
        statHero.hero_hp *= ConstData.Bonus_Hp;
        statHero.hero_armour *= ConstData.Bonus_Armor;
        statHero.hero_speed *= ConstData.Bonus_Speed;
        statHero.hero_mana = 0;
        AddStatsSkill1(baseData.ListStatsSkillHero[dataHero.skillDataHeroes[1].idSkill]);
        AddStatsSkill2(baseData.ListStatsSkillHero[dataHero.skillDataHeroes[2].idSkill]);
        statHero.max_hp = statHero.hero_hp;
        statHero.max_mana = ConstData.Max_Mana;
        ChangeMana(0);
    }
    public void AddStatsSkill1(StatsSkillHero skill)
    {
        int level_skill = dataHero.skillDataHeroes[1].levelSkill;
        switch (skill.statsSkillNormals.effectSkill[0].effect)
        {
            case "attack":
                statHero.hero_attack = statHero.hero_attack *(1+ skill.statsSkillNormals.effectSkill[0].rate[level_skill]);
                break;
            case "armor":
                statHero.hero_armour = statHero.hero_armour * (1 + skill.statsSkillNormals.effectSkill[0].rate[level_skill]);
                break;
            case "speed":
                statHero.hero_speed = statHero.hero_speed*(1 + skill.statsSkillNormals.effectSkill[0].rate[level_skill]);
                break;
            case "hp":
                statHero.hero_hp = statHero.hero_hp * (1 + skill.statsSkillNormals.effectSkill[0].rate[level_skill]);
                break;
            case "crit_rate":
                statHero.crit_rate+= statHero.crit_rate * (1 + skill.statsSkillNormals.effectSkill[0].rate[level_skill]);
                statHero.crit_dame = skill.statsSkillNormals.effectSkill[1].rate[level_skill];
                break;
            case "pure_dame":
                statHero.pure_dame = statHero.pure_dame * (1 + skill.statsSkillNormals.effectSkill[0].rate[level_skill]);
                break;
            case "evasion":
                statHero.evasion = skill.statsSkillNormals.effectSkill[0].rate[level_skill];
                break;
            case "magic_resist":
                statHero.magic_resist = skill.statsSkillNormals.effectSkill[0].rate[level_skill];
                break;
            case "dame_reduction":
                statHero.dame_reduction = skill.statsSkillNormals.effectSkill[0].rate[level_skill];
                break;
            case "armor_reduction":
                statHero.armor_reduction = skill.statsSkillNormals.effectSkill[0].rate[level_skill];
                break;
            case "dodge_chance":
                statHero.dodge_chance = skill.statsSkillNormals.effectSkill[0].rate[level_skill];
                break;
            case "mana_start":
                statHero.hero_mana = skill.statsSkillNormals.effectSkill[0].rate[level_skill];
                break;
        }
    }
    public void AddStatsSkill2(StatsSkillHero skill)
    {
        int level_skill = dataHero.skillDataHeroes[2].levelSkill;
        for(int i = 0; i < skill.statsSkillNormals.effectSkill.Count; i++)
        {
            EffectHit effectHitClone = new EffectHit();
            effectHitClone.nameEffect = skill.statsSkillNormals.effectSkill[i].effect;
            effectHitClone.dameEffect = skill.statsSkillNormals.effectSkill[i].rate[level_skill];
            effectHitClone.round = skill.statsSkillNormals.round;
            effectHits_start.Add(effectHitClone);
        }
    }
    public void SumStatsBody()
    {
        var _skin2 = skeletonAnimation.Skeleton.Skin;
        for (int i = 0; i < dataHero.listIdBody.Count; i++)
        {
            if (i >= 3)
            {
                StatsBaseBody baseBody = baseData.ListStatsBaseBody[dataHero.listIdBody[i]];
                statHero.hero_attack += baseBody.attack;
                statHero.hero_hp += baseBody.hp;
                statHero.hero_armour += baseBody.hp;
                statHero.hero_speed += baseBody.speed;
                sum_stats_body += (baseBody.attack + baseBody.hp + baseBody.hp + baseBody.speed);
            }
            ChangSkin(baseData.baseBodyPartAnim[dataHero.idHero].listBodyCharacter[i], baseData.baseBodyPartAnim[dataHero.idHero].spriteAtlas[dataHero.listIdBody[i] % 2], _skin2);
        }
        rality = 1 + (float)Mathf.Clamp((int)(sum_stats_body / 12) - 1, 0, 5) / 10;
        ClearSkin(_skin2);
    }
    public void ChangSkin(EquipHook bodyParts, SpriteAtlas spriteAtlas, Skin skin)
    {
        for (int i = 0; i < bodyParts.nameSlot.Length; i++)
        {
            // Debug.Log(bodyParts.nameSlot[i]);
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
            List<HeroManager> list_hero = new List<HeroManager>();
            list_hero.Add(gameObject.GetComponent<HeroManager>());
            BattleManager.instance.HeroAttack(list_hero);
        }
    }
    public void ClearSkin(Skin _skinChange)
    {
        skeletonAnimation.skeleton.SetSkin(_skinChange);
        skeletonAnimation.skeleton.SetSlotsToSetupPose();
        skeletonAnimation.Update(0);
        AtlasUtilities.ClearCache();
    }
    [SpineEvent] public string EventName = "attack";
    void HandleEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == EventName)
        {
            BattleManager.instance.CheckDamge();
        }
    }
    public void BurnHp(float damge, EffectHit effectSkill)
    {
        if (Checkdodge() || CheckEvasion())
        {
            skeletonAnimation.AnimationState.SetAnimation(0, ConstData.AnimHeroIdle, true);
        }
        else
        {
            statHero.hero_hp -= damge;
            ShowTextDame(damge);
            ChangeMana(10);
            CheckEffectHit();
            if (statHero.hero_hp <= 0)
            {
                statHero.hero_hp = 0;
                BattleManager.instance.RemoveHero(this);
                Die();
            }
            else
            {
                if (!string.IsNullOrEmpty(effectSkill.nameEffect))
                {
                    effectHit.Add(effectSkill);
                    Debug.Log(effectSkill.nameEffect);
                }
                skeletonAnimation.AnimationState.SetAnimation(0, ConstData.AnimHeroHit, false).Complete += delegate
                {
                   skeletonAnimation.AnimationState.SetAnimation(0, (effectSkill.nameEffect == "stun" || effectSkill.nameEffect == "frezee") ? ConstData.AnimHeroStun : ConstData.AnimHeroIdle, true);
                };
            }
            spriteHp.size = new Vector2(statHero.hero_hp / statHero.max_hp * 1.5f, 0.15f);
        }
    }
    public void Die()
    {
        skeletonAnimation.AnimationState.SetAnimation(0, ConstData.AnimHeroDie, false).Complete += delegate
        {
            gameObject.SetActive(false);
        };
    }
    public void ChangeMana(float _mana)
    {
        statHero.hero_mana += _mana;
        if (statHero.hero_mana >= 100)
        {
            statHero.hero_mana = 100;
        }
        spriteMana.size = new Vector2(statHero.hero_mana / statHero.max_mana * 1.5f, 0.08f);
    }
    public bool CheckEvasion()
    {
        if (statHero.evasion == 0)
            return false;
        else
        {
            float a = Random.Range(0, 100);
            if (a <= statHero.evasion * 100)
            {
                return true;
            }
            return false;
        }
    }
    public bool Checkdodge()
    {
        if (statHero.dodge_chance == 0)
            return false;
        else
        {
            float a = Random.Range(0, 100);
            if (a <= statHero.dodge_chance * 100)
            {
                return true;
            }
            return false;
        }
    }
    public void CheckEffectHit()
    {
        switch (effectHits_start[0].nameEffect)
        {
            case "return_dame":
                break;
            case "counter_dame":
                break;
            case "revive":
                break;
            case "importal":
                break;
            case "regen_hp":
                break;
            case "attack_hp":
                break;
            case "reflect_magic_dame":
                break;
        }
    }
    public void ShowTextDame(float dame)
    {
        GameObject go = BattleManager.instance.Pool(BattleManager.instance.txtClone, BattleManager.instance.transform);
        go.transform.position = transform.position;
        TextMeshPro textMeshPro = go.GetComponent<TextMeshPro>();
        textMeshPro.text = ((int)dame).ToString();
        //   textMeshPro.autoSizeTextContainer = true;
        textMeshPro.rectTransform.pivot = new Vector2(0.5f, 0);

        textMeshPro.alignment = TextAlignmentOptions.Bottom;
        textMeshPro.fontSize = 7;
        textMeshPro.enableKerning = false;
        textMeshPro.sortingOrder = 50;
        textMeshPro.color = new Color32(255, 255, 0, 255);
        Vector3 posEnd = new Vector3(go.transform.position.x, go.transform.position.y + 4, go.transform.position.z);
        go.transform.DOMove(posEnd, 1f, false).OnComplete(delegate
         {
             //   textMeshPro.autoSizeTextContainer = false;
             BattleManager.instance.DePool(go);
         });
    }
}
[System.Serializable]
public class StatHero
{
    public float hero_attack;
    public float hero_hp;
    public float hero_armour;
    public float hero_speed;
    public float hero_mana;
    public float max_hp;
    public float max_mana;
    public float crit_rate;
    public float crit_dame;
    public float pure_dame;
    public float evasion; // chỉ số né đánh thường
    public float magic_resist;
    public float dame_reduction;
    public float armor_reduction;
    public float dodge_chance; // chỉ số né cả skill lẫn đánh thường
    public float return_dame;
    public float counter_dame;
    public StatHero()
    {
        hero_attack = 0;
        hero_hp = 0;
        hero_armour = 0;
        hero_speed = 0;
        hero_mana = 0;
        max_hp = 0;
        max_mana = 0;
        crit_rate = 0;
        crit_dame = 1;
        pure_dame = 100;
        evasion = 0;
        magic_resist = 0;
        dame_reduction = 0;
        armor_reduction = 0;
        dodge_chance = 0;
    }
}
[System.Serializable]
public class EffectHit
{
    public string nameEffect;
    public float dameEffect;
    public float round;
    public float stack;
    public EffectHit()
    {
        nameEffect = string.Empty;
        dameEffect = 0;
        round = 0;
        stack = 0;
    }
}
