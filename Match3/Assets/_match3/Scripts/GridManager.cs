using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public GameObjectPool pool;
    public ObjectiveManager om;

    public Vector2 startPosition;
    public Vector2 gap;
    public int sizeX;
    public int sizeY;

    Item[,] grid;
    List<Item> itemsChecked;

    void Start()
    {
        grid = new Item[sizeX, sizeY];
        itemsChecked = new List<Item>();
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeTurn();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            SuffleGrid();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            bool tmp = HaveAnotherMove();
            Debug.Log("There is more moves: "+tmp);
        }
    }

    public void StartGame()
    {
        do
        {
            InstantiateRandomGrid();
            TakeTurn();
        } while (!HaveAnotherMove());
    }

    public void EndGame()
    {
        ClearGrid();
    }

    public bool TakeTurn()
    {
        Debug.Log("Turn");
        int ret=0;
        do
        {
            CheckGridVertical(sizeX, sizeY); //Vertical
            CheckGridHorisontal(sizeX, sizeY); //Horisontal
            ret++;
        } while (DestroyMatch());
        //Debug.Log("Take turn "+ret);
        return ret == 1 ? true : false;     
    }
    public void SuffleGrid()
    {
        Debug.Log("SuffleGrid");
        ClearGrid();
        InstantiateRandomGrid();
    }
    public bool CheckIfNeighbors(Item i1, Item i2)
    {
        Pair<int, int> p1 = CoordinatesOf<Item>(grid, i1);
        Pair<int, int> p2 = CoordinatesOf<Item>(grid, i2);

        if (Mathf.Abs(p1.First - p2.First) + Mathf.Abs(p1.Second - p2.Second) == 1)
            return true;

        return false;
    }
    public void SwitchPositions(Item i1, Item i2)
    {
        Pair<int, int> p1 = CoordinatesOf<Item>(grid, i1);
        Pair<int, int> p2 = CoordinatesOf<Item>(grid, i2);
        Item tmp = grid[p1.First, p1.Second];
        grid[p1.First, p1.Second] = grid[p2.First, p2.Second];
        grid[p2.First, p2.Second] = tmp;
    }

    void InstantiateRandomGrid()
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                GameObject go = GetRandomItem();
                go.SetActive(true);
                go.GetComponent<Item>().InstantiateAtPosition(new Vector2(startPosition.x + (gap.x * i), startPosition.y - (gap.y * j)));
                grid[i, j] = go.GetComponent<Item>();
                //Debug.Log("Tag - " + (Item)System.Enum.Parse(typeof(ItemTag), go.tag));
                //grid[j, i] = (ItemTag)System.Enum.Parse(typeof(ItemTag), go.tag);
            }
        }
    }
    void ClearGrid()
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                pool.ReturnObject(grid[i, j].gameObject);
            }
        }
    }
  
    void CheckGridVertical(int x, int y)
    {
        //Debug.Log("Checking Vertical");
        int actualTypeY = -1;
        int actualCountY = -1;
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (actualTypeY != grid[i, j].type)
                {
                    if (actualCountY >= 3)
                    {
                        Debug.Log("Match count:" + actualCountY + " of type: " + actualTypeY);
                        for (int f = 0; f < actualCountY; f++)
                        {
                            if(!itemsChecked.Contains(grid[i, j - 1 - f]))
                                itemsChecked.Add(grid[i, j-1 - f]);
                            //Debug.Log("Pos " + i + " " + (j-1 - f));
                        }
                    }
                    actualTypeY = grid[i, j].type;
                    actualCountY = 1;
                }
                else
                {
                    actualCountY++;
                }
                //Debug.Log(i + " | " + j);
            }
            if (actualCountY >= 3)
            {
                Debug.Log("Match count:" + actualCountY + " of type: " + actualTypeY);
                for (int f = 0; f < actualCountY; f++)
                {
                    if (!itemsChecked.Contains(grid[i, y - 1 - f]))
                        itemsChecked.Add(grid[i, y - 1 - f]);
                    //Debug.Log("Pos " + i + " " + (y - 1 - f));
                }
            }
            actualTypeY = -1;
            actualCountY = -1;
        }
    }
    void CheckGridHorisontal(int x, int y)
    {
        //Debug.Log("Checking Horisontal");
        int actualTypeX = -1;
        int actualCountX = -1;
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                if (actualTypeX != grid[j, i].type)
                {
                    if (actualCountX >= 3)
                    {
                        Debug.Log("Match count:" + actualCountX + " of type: " + actualTypeX);
                        for (int f = 0; f < actualCountX; f++)
                        {
                            if (!itemsChecked.Contains(grid[j - 1 - f, i]))
                                itemsChecked.Add(grid[j -1 - f, i]);
                            //Debug.Log("Pos " + (j-1 - f) + " " + i);
                        }
                    }
                    actualTypeX = grid[j, i].type;
                    actualCountX = 1;
                }
                else
                {
                    actualCountX++;
                }
                //Debug.Log(i + " | " + j);
            }
            if (actualCountX >= 3)
            {
                Debug.Log("Match count:" + actualCountX + " of type: " + actualTypeX);
                for (int f = 0; f < actualCountX; f++)
                {
                    if (!itemsChecked.Contains(grid[x - 1 - f, i]))
                        itemsChecked.Add(grid[x - 1 - f, i]);
                    //Debug.Log("Pos " + (x - 1 - f) + " " + i);
                }
            }
            actualTypeX = -1;
            actualCountX = -1;
        }
    }

    bool DestroyMatch()
    {
        if (itemsChecked.Count > 0)
        {
            List<Pair<int, int>> pos = new List<Pair<int, int>>();
            for (int i = 0; i < itemsChecked.Count; i++)
            {
                pool.ReturnObject(itemsChecked[i]);
                om.AddProgress(itemsChecked[i].type, 1);
                pos.Add(CoordinatesOf<Item>(grid, itemsChecked[i]));
            }
            itemsChecked.Clear();
            foreach (Pair<int, int> p in pos)
            {
                grid[p.First, p.Second] = null;
            }

            MoveItemsDown();
            return true;
        }
        else return false;
    }

    void MoveItemsDown()
    {
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                if(!grid[i, j])
                {
                    MoveItemDown(i,j);
                }    
            }
        }
    }
    void MoveItemDown(int x, int y)
    {
        if (y == 0)
        {
            GameObject go = GetRandomItem();
            go.SetActive(true);
            go.GetComponent<Item>().InstantiateAtPosition(new Vector2(startPosition.x + (gap.x * x), startPosition.y - (gap.y * (y-1))));
            go.GetComponent<Item>().MoveDown();
            grid[x, y] = go.GetComponent<Item>();
        }
        else
        {
            grid[x, y] = grid[x, y - 1];
            grid[x, y - 1] = null;
            grid[x, y].MoveDown();
            MoveItemDown(x, y - 1);
        }
    }

    GameObject GetRandomItem()
    {
        return pool.GetObject(Random.Range(1, pool.TypeCount + 1));
    }

    public bool HaveAnotherMove()
    {
        int x = sizeX;
        int y = sizeY;

        int actualType = 0 ;
        //vertical
        for (int i = 0; i < x; i++) 
        {
            for (int j = 0; j < y; j++)
            {
                actualType = grid[i, j].type;
                if (j + 1 < y && grid[i, j + 1].type == actualType) // *XX* case
                {
                    if (j - 2 >= 0 && grid[i, j - 2].type == actualType) return true;
                    if (i + 1 < x && j - 1 >= 0 && grid[i + 1, j - 1].type == actualType) return true;
                    if (i - 1 >= 0 && j - 1 >= 0 && grid[i - 1, j - 1].type == actualType) return true;

                    if (j + 3 < y && grid[i, j + 3].type == actualType) return true;
                    if (i + 1 < x && j + 2 < y && grid[i + 1, j + 2].type == actualType) return true;
                    if (i - 1 >= 0 && j + 2 < y && grid[i - 1, j + 2].type == actualType) return true;
                }
                else if (j + 2 < y && grid[i, j + 2].type == actualType) // X*X case
                {
                    if ((i - 1 >= 0 && j + 1 < y && grid[i - 1, j + 1].type == actualType) ||
                        (i + 1 < x && j + 1 < y && grid[i + 1, j + 1].type == actualType))
                        return true;
                }
            }
        }
        //horisontal
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                actualType = grid[j, i].type;
                if (j + 1 < x && grid[j + 1, i].type == actualType) // *XX* case
                {
                    if (j - 2 >= 0 && grid[j - 2, i].type == actualType) return true;
                    if (j - 1 >= 0 && i - 1 >= 0 && grid[j - 1, i - 1].type == actualType) return true;
                    if (j - 1 >= 0 && i + 1 < y && grid[j - 1, i + 1].type == actualType) return true;

                    if (j + 3 < x && grid[j + 3, i].type == actualType) return true;
                    if (j + 2 < x && i - 1 >= 0 && grid[j + 2, i - 1].type == actualType) return true;
                    if (j + 2 < x && i + 1 < y && grid[j + 2, i + 1].type == actualType) return true;

                }
                else if (j + 2 < x && grid[j + 2, i].type == actualType) // X*X case
                {
                    if ((j + 1 < x && i - 1 >= 0 && grid[j + 1, i - 1].type == actualType) ||
                        (j + 1 < x && i + 1 < y && grid[j + 1, i + 1].type == actualType))
                        return true;
                }
            }

        }
        return false;
    }

    public Pair<int,int> CoordinatesOf<T>(T[,] matrix, T value)
    {
        for (int x = 0; x < matrix.GetLength(0); ++x)
        {
            for (int y = 0; y < matrix.GetLength(1); ++y)
            {
                if (matrix[x, y].Equals(value))
                    return new Pair<int, int>(x, y);
            }
        }

        return new Pair<int, int>(-1, -1);
    }
}