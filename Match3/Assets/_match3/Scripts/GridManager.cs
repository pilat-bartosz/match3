using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using Random = UnityEngine.Random;

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
            var tmp = HaveAnotherMove();
            Debug.Log("There is more moves: " + tmp);
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
        var ret = 0;
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
        var p1 = CoordinatesOf(grid, i1);
        var p2 = CoordinatesOf(grid, i2);

        return math.abs(p1.x - p2.x) + math.abs(p1.y - p2.y) == 1;
    }

    public void SwitchPositions(Item i1, Item i2)
    {
        var p1 = CoordinatesOf(grid, i1);
        var p2 = CoordinatesOf(grid, i2);
        (grid[p1.x, p1.y], grid[p2.x, p2.y]) = (grid[p2.x, p2.y], grid[p1.x, p1.y]);
    }

    void InstantiateRandomGrid()
    {
        for (var i = 0; i < sizeX; i++)
        {
            for (var j = 0; j < sizeY; j++)
            {
                var go = GetRandomItem();
                go.SetActive(true);
                go.GetComponent<Item>()
                    .TeleportToPosition(new Vector2(startPosition.x + (gap.x * i), startPosition.y - (gap.y * j)));
                grid[i, j] = go.GetComponent<Item>();
                //Debug.Log("Tag - " + (Item)System.Enum.Parse(typeof(ItemTag), go.tag));
                //grid[j, i] = (ItemTag)System.Enum.Parse(typeof(ItemTag), go.tag);
            }
        }
    }

    void ClearGrid()
    {
        for (var i = 0; i < sizeX; i++)
        {
            for (var j = 0; j < sizeY; j++)
            {
                pool.ReturnObject(grid[i, j].gameObject);
            }
        }
    }

    void CheckGridVertical(int x, int y)
    {
        //Debug.Log("Checking Vertical");
        var actualTypeY = -1;
        var actualCountY = -1;
        for (var i = 0; i < x; i++)
        {
            for (var j = 0; j < y; j++)
            {
                if (actualTypeY != grid[i, j].type)
                {
                    if (actualCountY >= 3)
                    {
                        Debug.Log("Match count:" + actualCountY + " of type: " + actualTypeY);
                        for (var f = 0; f < actualCountY; f++)
                        {
                            if (!itemsChecked.Contains(grid[i, j - 1 - f]))
                                itemsChecked.Add(grid[i, j - 1 - f]);
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
                for (var f = 0; f < actualCountY; f++)
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
        var actualTypeX = -1;
        var actualCountX = -1;
        for (var i = 0; i < y; i++)
        {
            for (var j = 0; j < x; j++)
            {
                if (actualTypeX != grid[j, i].type)
                {
                    if (actualCountX >= 3)
                    {
                        Debug.Log("Match count:" + actualCountX + " of type: " + actualTypeX);
                        for (var f = 0; f < actualCountX; f++)
                        {
                            if (!itemsChecked.Contains(grid[j - 1 - f, i]))
                                itemsChecked.Add(grid[j - 1 - f, i]);
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
                for (var f = 0; f < actualCountX; f++)
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
            var pos = new List<int2>();
            for (var i = 0; i < itemsChecked.Count; i++)
            {
                pos.Add(CoordinatesOf(grid, itemsChecked[i]));
                pool.ReturnObject(itemsChecked[i]);
                om.AddProgress(itemsChecked[i].type, 1);
            }

            itemsChecked.Clear();
            foreach (var p in pos)
            {
                grid[p.x, p.y] = null;
            }

            bool wasMoved;
            do
            {
                wasMoved = MoveItemsDown();
            } while (wasMoved);

            return true;
        }
        else return false;
    }

    private bool MoveItemsDown()
    {
        var wasSomethingMoved = false;
        for (var i = 0; i < sizeX; i++)
        {
            //check by the columns
            for (var j = 0; j < sizeY; j++)
            {
                if (!grid[i, j])
                {
                    MoveItemDownTo(i, j);

                    wasSomethingMoved = true;
                }
            }
        }

        return wasSomethingMoved;
    }

    private Vector3 GetPositionFromGrid(int2 gridPosition)
    {
        return GetPositionFromGrid(gridPosition.x, gridPosition.y);
    }
    
    private Vector3 GetPositionFromGrid(int x, int y)
    {
        return new Vector3(startPosition.x + gap.x * x, startPosition.y - gap.y * y,0);
    }

    void MoveItemDownTo(int x, int y)
    {
        //position is at the top so spawn new item
        if (y == 0)
        {
            var go = GetRandomItem();
            go.SetActive(true);
            var item = go.GetComponent<Item>();
            item.TeleportToPosition(GetPositionFromGrid(x,y-1));
            item.MoveAtPosition(GetPositionFromGrid(x,y));
            grid[x, y] = item;
        }
        else
        {
            if (grid[x, y - 1] != null)
            {
                grid[x, y] = grid[x, y - 1];
                grid[x, y - 1] = null;
                grid[x, y].MoveAtPosition(GetPositionFromGrid(x,y));
            }
        }
        //MoveItemDownTo(x, y - 1);
    }

    GameObject GetRandomItem()
    {
        return pool.GetObject(Random.Range(1, pool.TypeCount + 1));
    }

    public bool HaveAnotherMove()
    {
        var x = sizeX;
        var y = sizeY;

        var actualType = 0;
        //vertical
        for (var i = 0; i < x; i++)
        {
            for (var j = 0; j < y; j++)
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
        for (var i = 0; i < y; i++)
        {
            for (var j = 0; j < x; j++)
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

    public int2 CoordinatesOf<T>(T[,] matrix, T value)
    {
        for (var x = 0; x < matrix.GetLength(0); ++x)
        {
            for (var y = 0; y < matrix.GetLength(1); ++y)
            {
                if (matrix[x, y].Equals(value))
                    return new int2(x, y);
            }
        }

        return new int2(-1, -1);
    }
}