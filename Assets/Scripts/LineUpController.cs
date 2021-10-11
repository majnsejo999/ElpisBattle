using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineUpController : MonoBehaviour
{
    public List<Transform> listTransHero, listTransEnemy;
    public BaseDataAll baseData;
    public UserData userData;
    public Material newMaterial;
    public void InitHeroToLine()
    {
        for (int i = 0; i < 8; i++)
        {
            CreateRuntimeAssetsAndGameObject(i,userData.data_hero[i].idHero, userData.data_hero[i].line, userData.data_hero[i].isEnemy);
        }
    }
    void CreateRuntimeAssetsAndGameObject(int index,int idHero, int index_line, bool isEnemy)
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
        skeletonGraphic.gameObject.AddComponent<CanvasGroup>();
    }
}
