using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsGenerationController : MonoBehaviour
{
    public GameObject[] platformsList;
    public GameObject firstPlatforms;
    protected GameObject lastPlatformCreated;
    protected string platformName;
    private TypeOfPlatform lastType;
    public GameObject backgroundBlock;
    private Transform generationPoint;
    public List<GameObject> elements;

    public int minHeight;
    public int maxHeight;
    protected int heightLevel;

    private List<GameObject> path;
    private bool[] platformsVisited;

    protected GameObject player;

    /** 
     *  Initialization
     */
    protected virtual void Start()
    {
        lastPlatformCreated = firstPlatforms;
        platformName = firstPlatforms.name;
        generationPoint = GameObject.Find("GenerationPoint").GetComponent<Transform>();
        heightLevel = 0;
        //Manual initialization of the weight of the platforms in the platformsList list
        ResetWeight();

        path = new List<GameObject>();
        platformsVisited = new bool[platformsList.Length];
        ResetVisited();

        player = GameObject.Find("Player");
        //Platform's probabilities are adapted only during the body mode not random, for a real player
        if (!PlayerPrefs.GetString("PlayerName").Equals("Default") && PlayerPrefs.GetInt("BodyMode") == 1 && PlayerPrefs.GetInt("RandomMode") == 0) AdaptProbabilities();

        SelectPlatformBody(TypeOfPlatform.leftArm);
    }

    /** 
     *  Update is called once per frame
     */
    protected virtual void Update()
    {
        GeneratePlatform();
    }

    /** 
     *  Generate the next blocks of the level between the generation point and the position and the platform generation controller position 
     */
    void GeneratePlatform()
    {
        if (transform.position.x < generationPoint.position.x)
        {
            if (path.Count == 0)
            {
                GeneratePlatformRandomly();
            }
            else
            {
                GeneratePlatformPath();
            }
        }
    }

    /** 
     *  Generate the next platform randomly
     */
    void GeneratePlatformRandomly()
    {
        int platformWidth = lastPlatformCreated.GetComponent<PrefabController>().width;
        int platformHeight = lastPlatformCreated.GetComponent<PrefabController>().height;
        int selector;
        GameObject[] neighbours = lastPlatformCreated.GetComponent<PrefabController>().possibleNeighbours;
        List<GameObject> accessibleNeighbours = new List<GameObject>();
        List<int> weights = new List<int>();

        for (int i = 0; i < neighbours.Length; i++)
        {
            platformHeight = neighbours[i].GetComponent<PrefabController>().height;
            if (heightLevel + platformHeight > minHeight && heightLevel + platformHeight < maxHeight)
            {
                //not two times the same body part in the adaptative mode
                if (neighbours[i].GetComponent<PrefabController>().myType != lastType || neighbours[i].GetComponent<PrefabController>().myType == TypeOfPlatform.neutral || (PlayerPrefs.GetInt("BodyMode") == 1 && PlayerPrefs.GetInt("RandomMode") == 1))
                {
                    //not two times the same platform
                    if (!neighbours[i].name.Equals(platformName) || neighbours[i].GetComponent<PrefabController>().myType == TypeOfPlatform.neutral)
                    {
                        accessibleNeighbours.Add(neighbours[i]);
                        //On prend le poids de la platform dans platformList parce que neighbours[i] n'a pas encore était instancié, sont Weight est nul
                        for (int j = 0; j < platformsList.Length; j++)
                        {
                            if (neighbours[i].Equals(platformsList[j]))
                            {
                                weights.Add(platformsList[j].GetComponent<PrefabController>().Weight);
                            }
                        }
                    }
                }
            }
        }

        selector = RandomSpawn(weights);
        platformWidth = accessibleNeighbours[selector].GetComponent<PrefabController>().width;
        platformHeight = accessibleNeighbours[selector].GetComponent<PrefabController>().height;
        lastPlatformCreated = accessibleNeighbours[selector];
        SpawnPlatform(platformWidth, platformHeight);
    }

    /** 
     *  Generate the next platform in the path
     */
    void GeneratePlatformPath()
    {
        lastPlatformCreated = path[0];
        path.RemoveAt(0);
        int platformWidth = lastPlatformCreated.GetComponent<PrefabController>().width;
        int platformHeight = lastPlatformCreated.GetComponent<PrefabController>().height;
        SpawnPlatform(platformWidth, platformHeight);
    }

    /** 
     *  Instantiate the new platform of the level
     */
    protected virtual void SpawnPlatform(int platformWidth, int platformHeight)
    {
        player.GetComponent<PlayerController>().SetData(lastPlatformCreated.name);
        int n = 1;
        bool firstPlatformsSpawn = lastPlatformCreated.Equals(firstPlatforms);
        if (firstPlatformsSpawn)
        {
            n = TrapGap();
        }
        else
        {
            platformName = lastPlatformCreated.name;
            lastType = lastPlatformCreated.GetComponent<PrefabController>().myType;
        }
        for (int i = 0; i < n; i++)
        {
            transform.position = new Vector3(transform.position.x + platformWidth, transform.position.y + platformHeight, transform.position.z);
            Instantiate(lastPlatformCreated, transform.position, transform.rotation);
            heightLevel = heightLevel + platformHeight;
            SpawnBackgroundBlocks(platformWidth, platformHeight);
            if (firstPlatformsSpawn) SpawnElement();
        }
    }

    /**
     * Compute the minimal gap between 2 traps  
     */
    protected int TrapGap()
    {
        float gravity = Physics.gravity.y;
        float gravityScale = player.GetComponent<Rigidbody2D>().gravityScale;
        float yCelerity = player.GetComponent<PlayerController>().playerJumpForce;
        float xCelerity = player.GetComponent<PlayerController>().PlayerSpeed;
        float delta = 2 * Mathf.Sqrt(((yCelerity * yCelerity) + 2) / ((gravity * gravityScale) * (gravity * gravityScale)));
        int distance = (int)(xCelerity * delta) + 1;
        return distance;
    }

    /** 
     *  Instantiate the new background blocks 
     */
    protected void SpawnBackgroundBlocks(int platformWidth, int platformHeight)
    {
        Vector3 backgroundPosition;
        if (platformHeight >= 0)
        {
            backgroundPosition = new Vector3(transform.position.x, transform.position.y - platformHeight - 5.5f, transform.position.z);
        }
        else
        {
            backgroundPosition = new Vector3(transform.position.x, transform.position.y - 5.5f, transform.position.z);
        }
        for (int i = 0; i < platformWidth; i++)
        {
            Instantiate(backgroundBlock, backgroundPosition, transform.rotation);
            backgroundPosition = new Vector3(backgroundPosition.x - 1, backgroundPosition.y, backgroundPosition.z);
        }
    }

    /**
     * Selects the next spawn decort element
     */
    protected void SpawnElement()
    {
        int selector;
        List<int> weights = new List<int>();
        for (int i = 0; i < elements.Count; i++)
        {
            weights.Add(elements[i].GetComponent<ElementController>().InitialSpawnWeight);
        }
        selector = RandomSpawn(weights);
        Instantiate(elements[selector], transform.position, transform.rotation);
    }

    /** 
     *  Select the next platform depending of the part of the body
     */
    public void SelectPlatformBody(TypeOfPlatform platformType)
    {
        if (platformType == TypeOfPlatform.neutral)
        {
            return;
        }
        List<GameObject> platformBody = new List<GameObject>();
        List<int> weights = new List<int>();
        int selector;
        for (int i = 0; i < platformsList.Length; i++)
        {
            if (platformsList[i].GetComponent<PrefabController>().myType == platformType)
            {
                platformBody.Add(platformsList[i]);
                weights.Add(platformsList[i].GetComponent<PrefabController>().Weight);
            }
        }
        selector = RandomSpawn(weights);
        DepthFirstSearch(firstPlatforms, platformBody[selector]);
    }

    /** 
     *  Find a path between the started platform and the targeted platform with a DepthFirstSearch algorith
     */
    void DepthFirstSearch(GameObject start, GameObject target)
    {
        GameObject current;
        int unvisitedNeighbours = 0;
        int index;
        Stack<GameObject> stack = new Stack<GameObject>();
        stack.Push(start);

        while (stack.Count > 0)
        {
            current = stack.Pop();
            index = System.Array.IndexOf(platformsList, current);
            if (!platformsVisited[index])
            {
                path.Add(current);
                platformsVisited[index] = true;
                if (current.Equals(target))
                {
                    return;
                }
                unvisitedNeighbours = 0;
                for (int i = 0; i < current.GetComponent<PrefabController>().possibleNeighbours.Length; i++)
                {
                    if (!path.Contains(current.GetComponent<PrefabController>().possibleNeighbours[i]))
                    {
                        stack.Push(current.GetComponent<PrefabController>().possibleNeighbours[i]);
                        unvisitedNeighbours++;
                    }
                }
                if (unvisitedNeighbours == 0)
                {
                    path.RemoveAt(path.Count - 1);
                }
            }
        }
        ResetVisited();
    }

    /** 
     *  Reset the content of platformsVisited to false 
     */
    protected void ResetVisited()
    {
        for (int i = 0; i < platformsVisited.Length; i++)
        {
            platformsVisited[i] = false;
        }
    }

    /** 
     *  Select the next platform depending of the part of the body
     */
    protected void AdaptProbabilities()
    {
        string playerName = PlayerPrefs.GetString("PlayerName");
        Dictionary<string, float> data = DataFileManager.FindData(playerName);
        if (data == null) return;
        float adjustment = 0.90f;
        foreach (KeyValuePair<string, float> entry in data)
        {
            if (entry.Value >= 0.5)
            {
                int index1 = -1;
                for (int i = 0; i < platformsList.Length; i++)
                {
                    if (platformsList[i].name == entry.Key)
                    {
                        index1 = i;
                        break;
                    }
                }
                if (index1 != -1)
                {
                    int index2 = -1;
                    for (int i = 0; i < platformsList.Length; i++)
                    {
                        if (platformsList[i].GetComponent<PrefabController>().myType == platformsList[index1].GetComponent<PrefabController>().myType && platformsList[i].tag == "coinPlatform")
                        {
                            index2 = i;
                            break;
                        }
                    }
                    if (index2 != -1)
                    {
                        int diffWeight = System.Convert.ToInt32(platformsList[index1].GetComponent<PrefabController>().InitialSpawnWeight * (entry.Value * adjustment));
                        platformsList[index1].GetComponent<PrefabController>().Weight = platformsList[index1].GetComponent<PrefabController>().Weight - diffWeight;
                        platformsList[index2].GetComponent<PrefabController>().Weight = platformsList[index2].GetComponent<PrefabController>().Weight + diffWeight;
                    }
                }
            }
        }
    }

    /** 
     *  Change the probability of a platform depending of the tag given
     */
    protected void TagProbabilities(string tag, int change)
    {
        for (int i = 0; i < platformsList.Length; i++)
        {
            if (platformsList[i].tag == tag)
            {
                if (change >= 0)//platformsList[i].GetComponent<PrefabController>().Weight + change >= 0)
                {
                    platformsList[i].GetComponent<PrefabController>().Weight = change;
                }
            }
        }
    }

    /** 
     *  Reset the weight of all the platform in platformList 
     */
    protected void ResetWeight()
    {
        for (int i = 0; i < platformsList.Length; i++)
        {
            platformsList[i].GetComponent<PrefabController>().Weight = platformsList[i].GetComponent<PrefabController>().InitialSpawnWeight;
        }
    }

    /** 
     *  Select an index depending on a list of weights
     */
    int RandomSpawn(List<int> weights)
    {
        int total = 0;
        foreach (int elem in weights)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < weights.Count; i++)
        {
            if (randomPoint < weights[i])
            {
                return i;
            }
            else
            {
                randomPoint -= weights[i];
            }
        }
        return weights.Count - 1;
    }
}
