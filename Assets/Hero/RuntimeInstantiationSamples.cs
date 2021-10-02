using UnityEngine;

using Spine;
using Spine.Unity;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using System.Text;

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
        //StartCoroutine(DownloadFile());
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
        //  runtimeSkeletonAnimation.AnimationName = "idle";
        runtimeSkeletonAnimation.Skeleton.SetSkin("skin_default");
        runtimeSkeletonAnimation.AnimationState.SetAnimation(0, "idle", true);
        runtimeSkeletonAnimation.skeleton.SetToSetupPose();
        runtimeSkeletonAnimation.skeleton.Update(0);
    }
    IEnumerator GetTextJson()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://elpis-test-asset.s3.ap-southeast-1.amazonaws.com/games/human.skel.bytes");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            File.WriteAllBytes(Application.dataPath + "/Resources/test.skel.bytes", www.downloadHandler.data);
            skeletonJson = Resources.Load("test.skel") as TextAsset;
        }
    }
    IEnumerator GetTextAtlas()
    {
        yield return new WaitForSeconds(1);
        UnityWebRequest www = UnityWebRequest.Get("https://elpis-test-asset.s3.ap-southeast-1.amazonaws.com/games/human.atlas.txt");
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
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://elpis-test-asset.s3.ap-southeast-1.amazonaws.com/games/human.png");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            textures[0] = ((DownloadHandlerTexture)www.downloadHandler).texture;
            textures[0].name = "human";
            ////Find the Standard Shader
            materialPropertySource = new Material(Shader.Find("Spine/Skeleton"));
            //Set Texture on the material
            materialPropertySource.SetTexture("_MainTex", textures[0]);
            // StartCoroutine(GetTextJson());
            //StartCoroutine(GetTextAtlas());
            StartCoroutine(DownloadFile("human.atlas.txt"));
            StartCoroutine(DownloadFile("human.skel.bytes"));
        }
    }
    IEnumerator DownloadFile(string nameFile)
    {
        var uwr = new UnityWebRequest("https://elpis-test-asset.s3.ap-southeast-1.amazonaws.com/games/" + nameFile, UnityWebRequest.kHttpVerbGET);
        string path = Path.Combine(Application.streamingAssetsPath, nameFile);
        uwr.downloadHandler = new DownloadHandlerFile(path);
        yield return uwr.SendWebRequest();
        if (uwr.result != UnityWebRequest.Result.Success)
            Debug.LogError(uwr.error);
        else
        {
            Debug.Log("File successfully downloaded and saved to " + path);
            if (nameFile.Contains("skel"))
            {
                //var asset = CreateInstance<ScriptableTextAsset>();
                //asset.m_Bytes = (byte[])bytes.Clone();
                //skeletonJson = new TextAsset(File.ReadAllBytes(Path.Combine(Application.streamingAssetsPath, nameFile)));
                byte[] bytes = File.ReadAllBytes(Path.Combine(Application.streamingAssetsPath, nameFile));
                Debug.Log(bytes.Length);
                string s1 = Encoding.UTF8.GetString(bytes);
                skeletonJson = new TextAsset(s1);
            }
            else
            {
                atlasText = new TextAsset(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, nameFile)));
            }
        }

    }
}