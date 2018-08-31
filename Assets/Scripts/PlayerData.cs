using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    private string name;
    private int id;
    private int randomMode;
    private int platform5;
    private int platform6;
    private int platform7;
    private int platform10;
    private int spikes;
    private int catcher;
    private int evilBlock;
    private int evilSaw;
    
public PlayerData(string playerName, int playerPlatform5, int playerPlatform6, int playerPlatform7, int playerPlatform10, int playerSpikes, int playerCatcher, int playerEvilBlock, int playerEvilSaw, int playerRandom)
    {
        name = playerName;
        randomMode = playerRandom;
        platform5 = playerPlatform5;
        platform6 = playerPlatform6;
        platform7 = playerPlatform7;
        platform10 = playerPlatform10;
        spikes = playerSpikes;
        catcher = playerCatcher;
        evilBlock = playerEvilBlock;
        evilSaw = playerEvilSaw;
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public int RandomMode
    {
        get { return randomMode; }
        set { randomMode = value; }
    }

    public int Platform5
    {
        get { return platform5; }
        set { platform5 = value; }
    }
    public int Platform6
    {
        get { return platform6; }
        set { platform6 = value; }
    }
    public int Platform7
    {
        get { return platform7; }
        set { platform7 = value; }
    }
    public int Platform10
    {
        get { return platform10; }
        set { platform10 = value; }
    }
    public int Spikes
    {
        get { return spikes; }
        set { spikes = value; }
    }
    public int Catcher
    {
        get { return catcher; }
        set { catcher = value; }
    }
    public int EvilBlock
    {
        get { return evilBlock; }
        set { evilBlock = value; }
    }
    public int EvilSaw
    {
        get { return evilSaw; }
        set { evilSaw = value; }
    }
}
