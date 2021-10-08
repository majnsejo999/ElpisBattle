using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public Image[] imgAva;

    public void SetAva(Sprite _sprite, int id)
    {
        imgAva[id].sprite = _sprite;
    }
}
