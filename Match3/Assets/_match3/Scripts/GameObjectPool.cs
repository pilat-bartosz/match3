using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectPool : MonoBehaviour
{
    public GameObject[] prefabs;

    public int TypeCount
    {
        get { return prefabs.Length; }
    }

    Stack<GameObject>[] stacks;

    void Awake()
    {
        if (prefabs.Length == 0)
            Debug.LogError("GameObjectPolls prefabs can't be null!");
        stacks = new Stack<GameObject>[prefabs.Length];
        for (int i = 0; i < prefabs.Length; i++)
        {
            stacks[i] = new Stack<GameObject>();
        }
    }

    public GameObject GetObject(int type)
    {
        int t = type - 1;
        if (stacks[t].Count > 0)
            return stacks[t].Pop();
        else
        {
            GameObject go = Instantiate(prefabs[t]);
            go.transform.parent = this.transform;
            return go;
        }
    }

    public void ReturnObject(GameObject go)
    {
        go.SetActive(false);
        stacks[go.GetComponent<Item>().type].Push(go);
    }
    public void ReturnObject(Item it)
    {
        it.gameObject.SetActive(false);
        stacks[it.type].Push(it.gameObject);
    }
}
