using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LineUpController : MonoBehaviour
{
    public List<Transform> listTransHero, listTransEnemy;
    public BaseDataAll baseData;
    public UserData userData;
    public Material newMaterial;
    public GameObject TabLeader, img_leader;
    public Image[] listAva;
    public Image imgAvaLeader;
    public void InitHeroToLine()
    {
        for (int i = 0; i < 8; i++)
        {
            CreateRuntimeAssetsAndGameObject(i, userData.data_hero[i].idHero, userData.data_hero[i].line, userData.data_hero[i].isEnemy);
        }
        imgAvaLeader.sprite = baseData.baseBodyPartAnim[userData.data_hero[userData.leaderHero].idHero].icon;
    }
    void CreateRuntimeAssetsAndGameObject(int index, int idHero, int index_line, bool isEnemy)
    {
        // Debug.Log(idHero);
        Material sourceMaterial = newMaterial;
        SpineAtlasAsset runtimeAtlasAsset = SpineAtlasAsset.CreateRuntimeInstance(baseData.baseBodyPartAnim[idHero].atlasText, baseData.baseBodyPartAnim[idHero].textures, sourceMaterial, true);
        SkeletonDataAsset runtimeSkeletonDataAsset = SkeletonDataAsset.CreateRuntimeInstance(baseData.baseBodyPartAnim[idHero].skeletonJson, runtimeAtlasAsset, true);
        SkeletonGraphic skeletonGraphic = SkeletonGraphic.NewSkeletonGraphicGameObject(runtimeSkeletonDataAsset, isEnemy ? listTransEnemy[index_line] : listTransHero[index_line], sourceMaterial);
        skeletonGraphic.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
        skeletonGraphic.rectTransform.pivot = new Vector2(0.5f, 0);
        if (isEnemy)
            skeletonGraphic.initialFlipX = true;
        skeletonGraphic.initialSkinName = "skin_default";
        skeletonGraphic.startingAnimation = "";
        skeletonGraphic.Initialize(true);
        skeletonGraphic.AnimationState.SetAnimation(0, "idle", true);
        skeletonGraphic.SetMaterialDirty();
        skeletonGraphic.gameObject.AddComponent<DragHandle>();
        HeroInLine handle = skeletonGraphic.transform.parent.GetComponent<HeroInLine>();
        handle.isEnemy = isEnemy;
        handle.indexHero = index;
        skeletonGraphic.gameObject.AddComponent<CanvasGroup>();
    }

    public void Battle()
    {
        BattleManager.instance.InitBattle();
        gameObject.SetActive(false);
    }
    public void ShowLeaderTab()
    {
        TabLeader.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            listAva[i].sprite = baseData.baseBodyPartAnim[userData.data_hero[i].idHero].icon;
        }
    }
    public void HideLeaderTab()
    {
        TabLeader.SetActive(false);
        imgAvaLeader.sprite = baseData.baseBodyPartAnim[userData.data_hero[userData.leaderHero].idHero].icon;
    }
    public void SwapSetLeader(int index)
    {
        img_leader.transform.SetParent(listAva[index].transform);
        img_leader.transform.localPosition = new Vector3(0, 61, 0);
        userData.leaderHero = index;
    }
}
