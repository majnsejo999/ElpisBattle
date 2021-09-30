using UnityEngine;
using UnityEngine.U2D;
[CreateAssetMenu]
public class BaseBodyPartAnim : ScriptableObject
{
    public int id;
    public EquipHook[] listBodyCharacter;
    public SpriteAtlas[] spriteAtlas;
}
[System.Serializable]
public class EquipHook
{
    public string[] nameSprite;
}