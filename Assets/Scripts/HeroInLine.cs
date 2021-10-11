using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class HeroInLine : MonoBehaviour, IDropHandler
{
    public bool isEnemy;
    public GameObject item
    {
        get
        {
            if(transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!item)
        {
            DragHandle.itemBeginDragged.transform.SetParent(transform);
            UserData.instance.SwapLine(DragHandle.indexDrag, transform.GetSiblingIndex());
        }
        else
        {
            transform.GetChild(0).transform.SetParent(DragHandle.itemBeginDragged.transform.parent);
            UserData.instance.SwapLine(DragHandle.indexDrag, transform.GetSiblingIndex());
            DragHandle.itemBeginDragged.transform.SetParent(transform);
        }
    }
}
