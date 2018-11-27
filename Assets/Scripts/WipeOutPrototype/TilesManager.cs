using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesManager : MonoBehaviour
{
    public GameObject currentTile;
    public GameObject[] prefabs;

    public int lados, frenTras, r;
    public float repeatLados, repeatFrenTras;

    #region Creating a Singleton(design pattern) to be acessed by all the code once
    private static TilesManager instance;
    public static TilesManager Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<TilesManager>();
            return instance;
        }

        set
        {
            instance = value;
        }
    }

    public Stack<GameObject>[] RecycledTiles { get; set; }
    #endregion


    // Use this for initialization
    void Start()
    {
        RecycledTiles = new Stack<GameObject>[4];
        StackInitiliazer();
        CreatTiles(10);
        r = 0;
        for (int i = 0; i < 100; i++)
        {
            TilesMaker();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnTile(GameObject prefab, int i)
    {

        foreach (var item in RecycledTiles)
        {
            if (item.Count == 0)
            {
                CreatTiles(10);
                break;
            }
        }

        GameObject obg = RecycledTiles[i].Pop();
        obg.SetActive(true);
        obg.transform.position = currentTile.transform.GetChild(0).transform.GetChild(i).position;
        currentTile = obg;

        //currentTile = Instantiate(prefab,
        //   currentTile.transform.GetChild(0).transform.GetChild(i).position,
        //   Quaternion.identity);
    }

    int MakePath(int lastTypeTile, int lados, int frenTras, float repeatLados, float repeatFrenTras)
    {
        int newTypeTile = 0;

        if (lastTypeTile % 2 == 0)
        {
            if (Random.Range((float)0.0, (float)1.0) > repeatLados)
                return lastTypeTile;
            else
            {
                newTypeTile = Random.Range(0, 100);
                if (newTypeTile > lados)
                {
                    return 1;
                }
                else
                {
                    return 3;
                }
            }
        }
        else
        {
            if (Random.Range((float)0, (float)1) > repeatFrenTras)
                return lastTypeTile;
            else
            {
                newTypeTile = Random.Range(0, 100);
                if (newTypeTile > frenTras)
                {
                    return 0;
                }
                else
                {
                    return 2;
                }
            }
        }
        //return lastTypeTile;
    }

    public void TilesMaker()
    {
        r = MakePath(r, lados, frenTras, repeatLados, repeatFrenTras);
        SpawnTile(prefabs[r], r);
    }

    private void StackInitiliazer()
    {
        for (int i = 0; i < RecycledTiles.Length; i++)
        {
            RecycledTiles[i] = new Stack<GameObject>();

        }
    }

    private void CreatTiles(int amount)
    {
        int j = 0;
        for (int i = 0; i < amount * 4; i++)
        {
            if (j < 4)
            {

                RecycledTiles[j].Push(Instantiate(prefabs[j]));
                RecycledTiles[j].Peek().SetActive(false);
                j++;
            }
            else
            {
                j = 0;
                RecycledTiles[j].Push(Instantiate(prefabs[j]));
                RecycledTiles[j].Peek().SetActive(false);
                j++;
            }
        }
    }

    //prefabs = new GameObject[4];
    //prefabs[0] = frontTilePrefab;
    //prefabs[1] = backTilePrefab;
    //prefabs[2] = leftTilePrefab;
    //prefabs[3] = rightTilePrefab;

    //public GameObject frontTilePrefab;
    //public GameObject backTilePrefab;
    //public GameObject leftTilePrefab;
    //public GameObject rightTilePrefab;
}
