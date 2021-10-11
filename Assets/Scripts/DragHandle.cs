using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Spine.Unity;
public class DragHandle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject itemBeginDragged;
    Vector3 startPosition;
    Transform startParent;
    SkeletonGraphic skeletonGraphic;
    public static int indexDrag;
    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeginDragged = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        skeletonGraphic = GetComponent<SkeletonGraphic>();
        indexDrag = transform.parent.GetComponent<HeroInLine>().indexHero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newPos = GetComponentInParent<Canvas>().worldCamera.ScreenToWorldPoint(eventData.position);
        newPos.z = 0;
        transform.position = newPos;
        if (skeletonGraphic.color == new Color32(255, 255, 255, 255))
        {
            skeletonGraphic.color = new Color32(255, 255, 255, 100);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeginDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        skeletonGraphic.color = new Color32(255, 255, 255, 255);
        transform.position = startPosition;
    }
}
