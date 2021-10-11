using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class HeroInLine : MonoBehaviour, IDropHandler
{
    public int indexHero;
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
        if((isEnemy && !DragHandle.itemBeginDragged.transform.parent.GetComponent<HeroInLine>().isEnemy) || (!isEnemy && DragHandle.itemBeginDragged.transform.parent.GetComponent<HeroInLine>().isEnemy))
        {
            return;
        }
        if (!item)
        {
            DragHandle.itemBeginDragged.transform.SetParent(transform);
            UserData.instance.SwapLine(DragHandle.indexDrag, transform.GetSiblingIndex());
            indexHero = DragHandle.indexDrag;
        }
        else
        {
            transform.GetChild(0).transform.SetParent(DragHandle.itemBeginDragged.transform.parent);
            UserData.instance.SwapLine(indexHero, DragHandle.itemBeginDragged.transform.parent.GetSiblingIndex());
            DragHandle.itemBeginDragged.transform.parent.GetComponent<HeroInLine>().indexHero = indexHero;
            DragHandle.itemBeginDragged.transform.SetParent(transform);
            UserData.instance.SwapLine(DragHandle.indexDrag, transform.GetSiblingIndex());
            indexHero = DragHandle.indexDrag;
        }
    }
}
