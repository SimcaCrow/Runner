using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Affdex;
using System.Linq;

public class PlayerEmotionController : ImageResultsListener
{
    public Dictionary<string, float> emotions;

    /**
     *  Initialization
     */
    void Awake()
    {
        emotions = new Dictionary<string, float>() { { "currentAnger", 0f }, { "currentSurprise", 0f }, { "currentJoy", 0f }, { "currentSadness", 0f } };
    }

    /**
     *  Is called when a face is detected by Affectiva
     */
    public override void onFaceFound(float timestamp, int faceId)
    {
        ;// if(Debug.isDebugBuild) Debug.Log("Found the face");
    }

    /**
     *  Is called when Affectiva does not detect a face
     */
    public override void onFaceLost(float timestamp, int faceId)
    {
        ;// if(Debug.isDebugBuild) Debug.Log("Lost the face");
    }

    /**
     *  Fill the dictionary with the returned values from Affectiva, for each emotion
     */
    public override void onImageResults(Dictionary<int, Face> faces)
    {
        float val;
        if (faces.Count > 0)
        {
            if (faces[0].Emotions.TryGetValue(Emotions.Anger, out val)) emotions["currentAnger"] = val;
            if (faces[0].Emotions.TryGetValue(Emotions.Surprise, out val)) emotions["currentSurprise"] = val;
            if (faces[0].Emotions.TryGetValue(Emotions.Joy, out val)) emotions["currentJoy"] = val;
            if (faces[0].Emotions.TryGetValue(Emotions.Sadness, out val)) emotions["currentSadness"] = val;
        }
    }

    /**
     *  Return the predominant emotion detected
     */
    public string EmotionDetected(float emotionDetection)
    {
        float max = emotions.Values.Max();
        if (max >= emotionDetection) return emotions.FirstOrDefault(x => x.Value == emotions.Values.Max()).Key;
        return "null";
    }
}