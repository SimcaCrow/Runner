using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorPickerController : MonoBehaviour
{
    public Texture2D colorSpace;
    public string Title = "Emotion";
    public int drawOrder = 0;
    public Color SelectedColor;

    static ColorPickerController activeColorPicker = null;

    private enum ESTATE
    {
        Hidden,
        Showed,
        Showing,
        Hidding
    };
    ESTATE mState = ESTATE.Hidden;

    private Color TempColor;
    private GUIStyle titleStyle = null;
    private Color textColor = new Color(0.94f, 0.33f, 0.31f, 1);
    private Texture2D txColorDisplay;
    private Vector2 startPos;

    private int sizeFull = 200;
    private int sizeHidden = 20;
    private float sizeCurr = 0;
    private float animTime = 0.5f;
    private float delta = 0;

    /** 
     *  Initialization
     */
    void Start()
    {
        startPos = new Vector2(this.transform.position.x, this.transform.position.y);
        txColorDisplay = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        sizeCurr = sizeHidden;
        SetColor(SelectedColor);
    }

    /** 
     *  Update is called once per frame
     */
    public void _DrawGUI()
    {
        if (titleStyle == null)
        {
            titleStyle = new GUIStyle(GUI.skin.label);
            titleStyle.normal.textColor = textColor;
        }

        GUI.DrawTexture(new Rect(startPos.x, startPos.y, sizeCurr, sizeCurr), colorSpace);
        GUI.DrawTexture(new Rect(startPos.x + sizeCurr + 10, startPos.y, 40, 20), txColorDisplay);
        GUI.Label(new Rect(startPos.x + sizeCurr + 60, startPos.y, 200, 30), Title, titleStyle);

        UpdateScaleState();
        DrawColorPicker();
    }

    /** 
     *  Update scaling states
     */
    private void UpdateScaleState()
    {
        if (mState == ESTATE.Showing)
        {
            sizeCurr = Mathf.Lerp(sizeHidden, sizeFull, delta / animTime);
            if (delta / animTime > 1.0f)
            {
                mState = ESTATE.Showed;
            }
            delta += Time.deltaTime;
        }
        if (mState == ESTATE.Hidding)
        {
            sizeCurr = Mathf.Lerp(sizeFull, sizeHidden, delta / animTime);
            if (delta / animTime > 1.0f)
            {
                mState = ESTATE.Hidden;
            }
            delta += Time.deltaTime;
        }
    }

    /** 
     *  Draw Color Picker
     */
    private void DrawColorPicker()
    {
        Rect rect = new Rect(startPos.x, startPos.y, sizeCurr, sizeCurr);
        Event e = Event.current;
        Vector2 mousePos = e.mousePosition;

        bool isLeftMBtnClicked = e.type == EventType.MouseUp;
        bool isLeftMBtnDragging = e.type == EventType.MouseDrag;
        bool isLeftMBtnMoving = e.type == EventType.MouseMove;

        bool openCondition = rect.Contains(mousePos) && ((isLeftMBtnClicked || isLeftMBtnDragging || isLeftMBtnMoving) && e.isMouse);
        bool closeCondition = isLeftMBtnClicked || (!rect.Contains(mousePos)) && (e.isMouse && isLeftMBtnMoving || e.type == EventType.MouseDown);

        if (openCondition && (activeColorPicker == null || activeColorPicker.mState == ESTATE.Hidden) && mState == ESTATE.Hidden)
        {
            mState = ESTATE.Showing;
            activeColorPicker = this;
            delta = 0;
        }

        if (closeCondition && mState == ESTATE.Showed)
        {
            if (isLeftMBtnClicked)
            {
                ApplyColor();
            }
            else
            {
                SetColor(SelectedColor);
            }
            mState = ESTATE.Hidding;
            delta = 0;
        }

        if (mState == ESTATE.Showed)
        {
            if (rect.Contains(mousePos))
            {
                float coeffX = colorSpace.width / sizeCurr;
                float coeffY = colorSpace.height / sizeCurr;
                Vector2 localImagePos = (mousePos - startPos);
                Color res = colorSpace.GetPixel((int)(coeffX * localImagePos.x), colorSpace.height - (int)(coeffY * localImagePos.y) - 1);
                SetColor(res);
                if (isLeftMBtnDragging)
                {
                    ApplyColor();
                }
            }
            else
            {
                SetColor(SelectedColor);

            }
        }
    }

    /** 
     *	Apply the temporary color
     */
    private void ApplyColor()
    {
        SelectedColor = TempColor;
    }

    /** 
     *	Set the Color Display color
     */
    public void SetColor(Color color)
    {
        TempColor = color;
        if (txColorDisplay != null)
        {
            txColorDisplay.SetPixel(0, 0, color);
            txColorDisplay.Apply();
        }
    }
}
