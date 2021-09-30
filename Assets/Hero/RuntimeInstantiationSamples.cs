using UnityEngine;

using Spine;
using Spine.Unity;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

public class RuntimeInstantiationSamples : MonoBehaviour
{
    public TextAsset skeletonJson;
    public TextAsset atlasText;
    public Texture2D[] textures;
    public Material materialPropertySource;

    SpineAtlasAsset runtimeAtlasAsset;
    SkeletonDataAsset runtimeSkeletonDataAsset;
    SkeletonAnimation runtimeSkeletonAnimation;
    private void Start()
    {
        StartCoroutine(GetTexture());
        Invoke("CreateRuntimeAssetsAndGameObject", 5);
    }
    void CreateRuntimeAssetsAndGameObject()
    {
        // 1. Create the AtlasAsset (needs atlas text asset and textures, and materials/shader);
        // 2. Create SkeletonDataAsset (needs json or binary asset file, and an AtlasAsset)
        // 3. Create SkeletonAnimation (needs a valid SkeletonDataAsset)
        runtimeAtlasAsset = SpineAtlasAsset.CreateRuntimeInstance(atlasText, textures, materialPropertySource, true);
        runtimeSkeletonDataAsset = SkeletonDataAsset.CreateRuntimeInstance(skeletonJson, runtimeAtlasAsset, true);
        runtimeSkeletonAnimation = SkeletonAnimation.NewSkeletonAnimationGameObject(runtimeSkeletonDataAsset);
        runtimeSkeletonAnimation.AnimationName = "idle";
        runtimeSkeletonAnimation.Skeleton.SetSkin("skin_default");
        runtimeSkeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
        runtimeSkeletonAnimation.skeleton.SetToSetupPose();
        runtimeSkeletonAnimation.skeleton.Update(0);
    }
    IEnumerator GetTextJson()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://elpis.game/assert/animations/Naga/Naga.json");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            File.WriteAllText(Application.dataPath + "/Resources/test.json.txt", www.downloadHandler.text);
            skeletonJson = Resources.Load("test.json") as TextAsset;
        }
    }
    IEnumerator GetTextAtlas()
    {
        yield return new WaitForSeconds(1);
        UnityWebRequest www = UnityWebRequest.Get("https://elpis.game/assert/animations/Naga/Naga.atlas");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            File.WriteAllText(Application.dataPath + "/Resources/test.atlas.txt", www.downloadHandler.text);
            atlasText = Resources.Load("test.atlas") as TextAsset;
        }
    }
    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://elpis.game/assert/animations/Naga/Naga.png");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            textures[0] = ((DownloadHandlerTexture)www.downloadHandler).texture;
            textures[0].name = "Naga.png";
            ////Find the Standard Shader
            //materialPropertySource = new Material(Shader.Find("Spine/Skeleton"));
            //Set Texture on the material
            materialPropertySource.SetTexture("_MainTex", textures[0]);
            StartCoroutine(GetTextJson());
            StartCoroutine(GetTextAtlas());
        }
    }
}