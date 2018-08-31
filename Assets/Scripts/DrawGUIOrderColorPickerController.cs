using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DrawGUIOrderColorPickerController : MonoBehaviour
{
    public ColorPickerController[] colorPicker;
    private List<ColorPickerController> colorPickerList;
    private Dictionary<string, Color> emotionColors;

    /** 
     *  Initialization on Awake
     */
    void Awake()
    {
        InitializeColorPicker();
        InitializeEmotionDictionary();
    }

    /** 
     *  Initialize the color picker list
     */
    public void InitializeColorPicker()
    {
        colorPicker = GameObject.FindObjectsOfType<ColorPickerController>();
        colorPickerList = new List<ColorPickerController>();
        colorPickerList.AddRange(colorPicker);

        colorPickerList = colorPickerList.OrderBy(item => item.drawOrder).ToList();
        colorPickerList.Reverse();
        colorPickerList.CopyTo(colorPicker);
    }

    /** 
     *  Initialize the dictionnary with default color associated with emotion
     */
    public void InitializeEmotionDictionary()
    {
        emotionColors = new Dictionary<string, Color>() { };
        foreach (var elem in colorPickerList)
        {
            emotionColors.Add(elem.Title, elem.SelectedColor);
        }
    }

    /** 
     *  Show events update on GUI
     */
    void OnGUI()
    {
        foreach (var elem in colorPickerList)
        {
            elem._DrawGUI();
            UpdateEmotionColor(elem);
        }
    }

    /** 
     *  Update dictionnary with color selected for the emotion on the color picker
     */
    private void UpdateEmotionColor(ColorPickerController colorPicker)
    {
        emotionColors[colorPicker.Title] = colorPicker.SelectedColor;
    }

    /** 
     *  Return the dictionnary containing colors for each emotion
     */
    public Dictionary<string, Color> GetEmotionColors()
    {
        return emotionColors;
    }

    /** 
     *  Modify default color in the color picker according to a new dictionnary of colors for each emotion
     */
    public void SetEmotionColors(Dictionary<string, Color> newEmotionColors)
    {
        foreach (var elem in colorPickerList)
        {
            elem.SelectedColor = newEmotionColors[elem.Title];
            elem.SetColor(newEmotionColors[elem.Title]);
        }
    }
}
