using UnityEngine;
using UnityEngine.EventSystems;

public class buttonAnimation : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler , IPointerDownHandler, IPointerUpHandler
{
    
    private float buttonScale = 1.2f;
    private float pressScale = 0.9f;
    private bool isHovering ;
    
    private Vector3 originalScale;
  
    void Awake()
    {
        originalScale = transform.localScale;
       
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        transform.localScale = originalScale * buttonScale;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        transform.localScale = originalScale;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = originalScale * pressScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isHovering)
        {
            transform.localScale = originalScale * buttonScale;
        }
        else
        {
            transform.localScale = originalScale;
        }
    }


   
    
}
