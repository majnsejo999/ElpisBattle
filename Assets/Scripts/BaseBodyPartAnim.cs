using UnityEngine;
using Spine.Unity;

[CreateAssetMenu]
public class BaseBodyPartAnim : ScriptableObject, IHasSkeletonDataAsset
{
    public SkeletonDataAsset skeletonDataAsset;
    SkeletonDataAsset IHasSkeletonDataAsset.SkeletonDataAsset { get { return this.skeletonDataAsset; } }
    public int id;
    [SpineSkin]
    public string templateSkin;
    public EquipHook[] listBodyCharacter;
}
[System.Serializable]
public class EquipHook
{
    [SpineSlot]
    public string slot;
    [SpineAttachment(skinField: "templateSkin")]
    public string templateAttachment;
    public Sprite[] _sprite;
}