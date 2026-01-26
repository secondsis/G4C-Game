using System;
using UnityEngine;

public class Hover3DTooltip : MonoBehaviour
{
    public event Action OnTooltipShow;
    public event Action OnTooltipHide;
    public SimpleTooltipStyle simpleTooltipStyle;
    [TextArea] public string infoLeft = "Hello";
    [TextArea] public string infoRight = "";
    private STController tooltipController;
    private Outline outline;
    public bool showing = false;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        tooltipController = FindFirstObjectByType<STController>();
        if (!tooltipController)
        {
            Debug.LogWarning("Could not find the Tooltip prefab");
            Debug.LogWarning("Make sure you don't have any other prefabs named `SimpleTooltip`");
        }
        if (!simpleTooltipStyle)
            simpleTooltipStyle = Resources.Load<SimpleTooltipStyle>("STDefault");
    }
    public void OnHoverEnter()
    {
        outline.OutlineMode = Outline.Mode.OutlineVisible;
    }
    public void OnHoverOver()
    {
        ShowTooltip();
        // While hovering, (for crops) check for pressing E
    }
    public void OnHoverExit()
    {
        outline.OutlineMode = Outline.Mode.OutlineHidden;
        HideTooltip();
    }

    public void ShowTooltip()
    {
        showing = true;
        OnTooltipShow?.Invoke();

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
        OnTooltipHide?.Invoke();
        tooltipController.HideTooltip();
    }
}
