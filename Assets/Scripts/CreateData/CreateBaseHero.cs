using GoogleSheetsForUnity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GoogleSheetsForUnity.SpreadsheetsExample;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class CreateBaseHero : MonoBehaviour
{
    public string _tableName = "basedata";
    public StatsBaseHeroSheet[] _heroData;
    public StatsSkillSheet[] _skillData;
    private void OnEnable()
    {
        // Suscribe for catching cloud responses.
        Drive.responseCallback += HandleDriveResponse;
    }

    private void OnDisable()
    {
        // Remove listeners.
        Drive.responseCallback -= HandleDriveResponse;
    }
    public void GetAllPlayers(string nameTable)
    {
        Debug.Log("<color=yellow>Retrieving all players from the Cloud.</color>");
        _tableName = nameTable;
        // Get all objects from table 'PlayerInfo'.
        Drive.GetTable(_tableName, true);
    }
    public void GetAllTables()
    {
        Debug.Log("<color=yellow>Retrieving all data tables from the Cloud.</color>");

        // Get all objects from table 'PlayerInfo'.
        Drive.GetAllTables(true);
    }
    public void HandleDriveResponse(Drive.DataContainer dataContainer)
    {
        Debug.Log(dataContainer.msg);

        // First check the type of answer.
        if (dataContainer.QueryType == Drive.QueryType.getObjectsByField)
        {
            string rawJSon = dataContainer.payload;
            Debug.Log(rawJSon);

            // Check if the type is correct.
            if (string.Compare(dataContainer.objType, _tableName) == 0)
            {
                // Parse from json to the desired object type.
                StatsBaseHero[] heroes = JsonHelper.ArrayFromJson<StatsBaseHero>(rawJSon);

                for (int i = 0; i < heroes.Length; i++)
                {
                    Debug.Log("<color=yellow>Object retrieved from the cloud and parsed: \n</color>" +
                        "Origin: " + heroes[i].origin + "\n" +
                        "Attack: " + heroes[i].attack + "\n" +
                        "HP: " + heroes[i].hp + "\n" +
                        "Armour: " + heroes[i].armour + "\n" +
                        "Speed: " + heroes[i].speed + "\n");
                }
            }
        }

        // First check the type of answer.
        if (dataContainer.QueryType == Drive.QueryType.getTable)
        {
            string rawJSon = dataContainer.payload;
            Debug.Log(rawJSon);

            // Check if the type is correct.
            if (string.Compare(dataContainer.objType, _tableName) == 0)
            {
                if (_tableName == "StatsBase")
                {
                    _heroData = JsonHelper.ArrayFromJson<StatsBaseHeroSheet>(rawJSon);
                    string logMsg = "<color=yellow>" + _heroData.Length.ToString() + " objects retrieved from the cloud and parsed:</color>";
                    for (int i = 0; i < _heroData.Length; i++)
                    {
                        StatsBaseHero statsBaseHero = ScriptableObject.CreateInstance<StatsBaseHero>();
                        statsBaseHero.id = _heroData[i].id;
                        statsBaseHero.origin = _heroData[i].origin;
                        statsBaseHero.attack = _heroData[i].attack;
                        statsBaseHero.hp = _heroData[i].hp;
                        statsBaseHero.armour = _heroData[i].armour;
                        statsBaseHero.speed = _heroData[i].speed;
                        statsBaseHero.attack_lv = _heroData[i].attack_lv;
                        statsBaseHero.hp_lv = _heroData[i].hp_lv;
                        statsBaseHero.armour_lv = _heroData[i].armour_lv;
                        statsBaseHero.speed_lv = _heroData[i].speed_lv;
                        statsBaseHero.attack_ev = _heroData[i].attack_ev;
                        statsBaseHero.hp_ev = _heroData[i].hp_ev;
                        statsBaseHero.armour_ev = _heroData[i].armour_ev;
                        statsBaseHero.speed_ev = _heroData[i].speed_ev;
#if UNITY_EDITOR
                        AssetDatabase.CreateAsset(statsBaseHero, "Assets/BaseData/StatsBase/" + _heroData[i].id + "_" + _heroData[i].origin + ".asset");
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
#endif
                    }
                    Debug.Log(logMsg);
                }
                else if (_tableName == "SkillHero")
                {
                    _skillData = JsonHelper.ArrayFromJson<StatsSkillSheet>(rawJSon);
                    string logMsg = "<color=yellow>" + _heroData.Length.ToString() + " objects retrieved from the cloud and parsed:</color>";
                    for (int i = 0; i < _skillData.Length; i++)
                    {
                        StatsSkillHero statsSkill = ScriptableObject.CreateInstance<StatsSkillHero>();
                        statsSkill.id = i;
                        statsSkill.skill_name = _skillData[i].skill_name;
                        statsSkill.rality = _skillData[i].rality;
                        statsSkill.mana = _skillData[i].mana;
                        statsSkill.type_skill = _skillData[i].type_skill;
                        statsSkill.origin = _skillData[i].origin.Split('\n').ToList();
                        statsSkill.detail = _skillData[i].detail;
                        statsSkill.statsSkillNormals = new StatsSkill();
                        statsSkill.statsSkillNormals.target = StringToListFloat(_skillData[i].target.Split('/').ToList());
                        statsSkill.statsSkillNormals.require = _skillData[i].require;
                        statsSkill.statsSkillNormals.type_damge = _skillData[i].type_dame;
                        statsSkill.statsSkillNormals.damge = StringToListFloat(_skillData[i].dame.Split('/').ToList());
                        statsSkill.statsSkillNormals.round = _skillData[i].round;
                        statsSkill.statsSkillNormals.max_stack = _skillData[i].max_stack;
                        statsSkill.statsSkillNormals.effect_dame = StringToListFloat(_skillData[i].effect_dame.Split('/').ToList());
                        List<string> listEffect = new List<string>();
                        List<string> listRate = new List<string>();
                        if (_skillData[i].effect.Contains('\n'))
                        {
                            listEffect = _skillData[i].effect.Split('\n').ToList();
                            listRate = _skillData[i].rate.Split('\n').ToList();
                        }
                        else
                        {
                            listEffect.Add(_skillData[i].effect);
                            listRate.Add(_skillData[i].rate);
                        }
                        for (int j = 0; j < listEffect.Count; j++)
                        {
                            StatsEffectSkill effectSkill = new StatsEffectSkill();
                            effectSkill.effect = listEffect[j];
                            effectSkill.rate = StringToListFloat(listRate[j].Split('/').ToList());
                            statsSkill.statsSkillNormals.effectSkill.Add(effectSkill);
                        }
#if UNITY_EDITOR
                        AssetDatabase.CreateAsset(statsSkill, "Assets/BaseData/SkillBase/" + statsSkill.type_skill + "Skill" + i + ".asset");
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
#endif
                    }
                    Debug.Log(logMsg);
                }
            }
        }

        // First check the type of answer.
        if (dataContainer.QueryType == Drive.QueryType.getAllTables)
        {
            string rawJSon = dataContainer.payload;

            // The response for this query is a json list of objects that hold tow fields:
            // * objType: the table name (we use for identifying the type).
            // * payload: the contents of the table in json format.
            Drive.DataContainer[] tables = JsonHelper.ArrayFromJson<Drive.DataContainer>(rawJSon);

            // Once we get the list of tables, we could use the objTypes to know the type and convert json to specific objects.
            // On this example, we will just dump all content to the console, sorted by table name.
            string logMsg = "<color=yellow>All data tables retrieved from the cloud.\n</color>";
            for (int i = 0; i < tables.Length; i++)
            {
                logMsg += "\n<color=blue>Table Name: " + tables[i].objType + "</color>\n" + tables[i].payload + "\n";
            }
            Debug.Log(logMsg);
        }
    }
    public List<float> StringToListFloat(List<string> listString)
    {
        List<float> list1 = new List<float>();
        for (int i = 0; i < listString.Count; i++)
        {
            list1.Add(float.Parse(listString[i]));
        }
        return list1;
    }
}
[System.Serializable]
public class StatsBaseHeroSheet
{
    public int id;
    public string origin;
    public float attack;
    public float hp;
    public float armour;
    public float speed;
    public float attack_lv;
    public float hp_lv;
    public float armour_lv;
    public float speed_lv;
    public float attack_ev;
    public float hp_ev;
    public float armour_ev;
    public float speed_ev;
}
[System.Serializable]
public class StatsSkillSheet
{
    public string skill_name;
    public string rality;
    public string type_skill;
    public string origin;
    public int mana;
    public string detail;
    public string target;
    public string require;
    public string type_dame;
    public string dame;
    public string effect;
    public string rate;
    public float round;
    public float max_stack;
    public string effect_dame;
}