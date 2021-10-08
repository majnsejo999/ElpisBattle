using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAvatar : MonoBehaviour
{
    Dictionary<string, Sprite> DicAvatar;

    [SerializeField]
    RenderTexture renderTexture;

    [SerializeField]
    Camera cameraa;

    Texture2D t;

    Rect rect = new Rect(0, 0, 122, 122);

    Sprite a;
    public Transform ava;
    public Sprite Avatar(string id)
    {
        if (DicAvatar == null)
            DicAvatar = new Dictionary<string, Sprite>();
        if (DicAvatar.ContainsKey(id))
        {
            return DicAvatar[id];
        }
        else
        {
            RenderTexture.active = renderTexture;
            this.cameraa.Render();
            t = new Texture2D(renderTexture.width, renderTexture.height);
            t.ReadPixels(rect, 0, 0);
            t.Apply();
            a = Sprite.Create(t, rect, Vector2.zero);
            DicAvatar.Add(id, a);
            return a;
        }
    }
}
