using UnityEngine;
using UnityEngine.U2D;
[CreateAssetMenu]
public class BaseBodyPartAnim : ScriptableObject
{
    public EquipHook[] listBodyCharacter;
    public SpriteAtlas[] spriteAtlas;
    public TextAsset skeletonJson;
    public TextAsset atlasText;
    public Texture2D[] textures;
    public Material materialPropertySource;
    public Sprite icon;
}
[System.Serializable]
public class EquipHook
{
    public string[] nameSlot;
    public string[] nameSprite;
}