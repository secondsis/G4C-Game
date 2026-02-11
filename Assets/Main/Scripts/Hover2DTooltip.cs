using UnityEngine;
using UnityEngine.EventSystems;

public class Hover2DTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public SimpleTooltipStyle simpleTooltipStyle;
    [TextArea] public string infoLeft = "";
    [TextArea] public string infoRight = "";
    private STController tooltipController;
    private bool showing = false;
    private bool hovering = false; 
    private bool _enabled = false;

    private void Awake()
    {
        tooltipController = FindFirstObjectByType<STController>();
        if (!tooltipController)
        {
            Debug.LogWarning("Could not find the Tooltip prefab");
            Debug.LogWarning("Make sure you don't have any other prefabs named `SimpleTooltip`");
        }
        if (!simpleTooltipStyle)
            simpleTooltipStyle = Resources.Load<SimpleTooltipStyle>("STDefault");
    }

    public void enableTooltip()
    {
        _enabled = true;
    }

    public void disableTooltip()
    {
        _enabled = false;
        OnPointerExit(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_enabled)
        {
            ShowTooltip();
            hovering = true;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
        hovering = false;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (hovering && _enabled)
        {
            ShowTooltip();
        }
    }

    public void ShowTooltip()
    {
        showing = true;

        // Update the text for both layers
        tooltipController.SetCustomStyledText(infoLeft, simpleTooltipStyle, STController.TextAlign.Left);
        tooltipController.SetCustomStyledText(infoRight, simpleTooltipStyle, STController.TextAlign.Right);

        // Then tell the controller to show it
        tooltipController.ShowTooltip();
    }

    public void HideTooltip()
    {
        if (!showing)
            return;
        showing = false;
        tooltipController.HideTooltip();
    }
}