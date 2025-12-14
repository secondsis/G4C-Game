using UnityEngine;

public class Hover3DTooltip : MonoBehaviour
{
    public SimpleTooltipStyle simpleTooltipStyle;
    [TextArea] public string infoLeft = "Hello";
    [TextArea] public string infoRight = "";
    private STController tooltipController;
    private Outline outline;
    private bool showing = false;

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
    }
    public void OnHoverExit()
    {
        outline.OutlineMode = Outline.Mode.OutlineHidden;
        HideTooltip();
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
