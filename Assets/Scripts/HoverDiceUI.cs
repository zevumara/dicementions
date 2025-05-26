using UnityEngine;
using UnityEngine.EventSystems;

public class HoverDiceUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public DiceBase targetDice;

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetDice?.OnHoverEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetDice?.OnHoverExit();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        targetDice?.OnClick();
    }
}