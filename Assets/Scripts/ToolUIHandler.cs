using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolUIHandler : MonoBehaviour
{
    public Text toolName;
    public Image currentColourImage;

    public void ChangeTool(DrawingTool tool)
    {
        toolName.text = tool.toolName;
        currentColourImage.color = tool.paintColour;
    }
}
