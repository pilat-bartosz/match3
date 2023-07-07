using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
    GameManager gm;
    public int type;
    public float scale = 0.5f;
    float smallScale;

    public float diffX = 2.0f;
    public float diffY = 1.5f;

    Vector3 destination;
    bool isAnimated = false;

    bool shrink = true;

    void Start()
    {
        smallScale = scale * .8f;
        this.transform.localScale *= scale;
        gm = FindObjectOfType<GameManager>();
        if (!gm) Debug.LogError("GameManager not found");
    }
    public void InstantiateAtPosition(Vector3 vec)
    {
        this.transform.position = vec;
        destination = vec;
    }

    public void MoveAtPosition(Vector3 vec)
    {
        destination = vec;
    }

    public void MoveDown(int amount = 1)
    {
        destination += new Vector3(0, -diffY * amount, 0);
    }

    public void ToggleActual()
    {
        isAnimated = !isAnimated;
    }

    void Update()
    {
        if(transform.position != destination)
            this.transform.position = Vector3.MoveTowards(transform.position, destination, 0.2f);
        if(isAnimated && shrink)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(smallScale, smallScale, 1f), 0.01f);
            if (transform.localScale == new Vector3(smallScale, smallScale, 1f)) shrink = !shrink;
        }
        else
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(scale, scale, 1f), 0.01f);
            if (transform.localScale == new Vector3(scale, scale, 1f)) shrink = !shrink;
        }
    }

    void OnMouseDown()
    {
        gm.ItemClicked(this);
    }
}
