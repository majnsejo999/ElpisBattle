using UnityEngine;

[CreateAssetMenu]
public class BaseBodyPartAnim : ScriptableObject
{
    public int id;
    public EquipHook[] listBodyCharacter;
}
[System.Serializable]
public class EquipHook
{
    public string nameBodyPart;
    public Sprite[] _sprite;
}