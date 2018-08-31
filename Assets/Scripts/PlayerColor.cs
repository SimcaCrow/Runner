using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor
{
    private string name;
    private float angerR;
    private float angerG;
    private float angerB;
    private float surpriseR;
    private float surpriseG;
    private float surpriseB;
    private float joyR;
    private float joyG;
    private float joyB;
    private float sadnessR;
    private float sadnessG;
    private float sadnessB;

    public PlayerColor(string playerName, Color anger, Color surprise, Color joy, Color sadness)
    {
        name = playerName;
        angerR = anger.r;
        angerG = anger.g;
        angerB = anger.b;
        surpriseR = surprise.r;
        surpriseG = surprise.g;
        surpriseB = surprise.b;
        joyR = joy.r;
        joyG = joy.g;
        joyB = joy.b;
        sadnessR = sadness.r;
        sadnessG = sadness.g;
        sadnessB = sadness.b;
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public float AngerR
    {
        get { return angerR; }
        set { angerR = value; }
    }
    public float AngerG
    {
        get { return angerG; }
        set { angerG = value; }
    }
    public float AngerB
    {
        get { return angerB; }
        set { angerB = value; }
    }

    public float SurpriseR
    {
        get { return surpriseR; }
        set { surpriseR = value; }
    }
    public float SurpriseG
    {
        get { return surpriseG; }
        set { surpriseG = value; }
    }
    public float SurpriseB
    {
        get { return surpriseB; }
        set { surpriseB = value; }
    }

    public float JoyR
    {
        get { return joyR; }
        set { joyR = value; }
    }
    public float JoyG
    {
        get { return joyG; }
        set { joyG = value; }
    }
    public float JoyB
    {
        get { return joyB; }
        set { joyB = value; }
    }

    public float SadnessR
    {
        get { return sadnessR; }
        set { sadnessR = value; }
    }
    public float SadnessG
    {
        get { return sadnessG; }
        set { sadnessG = value; }
    }
    public float SadnessB
    {
        get { return sadnessB; }
        set { sadnessB = value; }
    }
}
