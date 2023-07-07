using UnityEngine;

public class Item : MonoBehaviour
{
    GameManager gm;
    public int type;
    public float scale = 0.5f;
    public float speed = 0.2f;
    float smallScale;

    Vector3 destination;
    bool isAnimated = false;

    bool shrink = true;

    void Start()
    {
        smallScale = scale * .8f;
        transform.localScale *= scale;
        gm = FindObjectOfType<GameManager>();
        if (!gm) Debug.LogError("GameManager not found");
    }
    public void TeleportToPosition(Vector3 vec)
    {
        transform.position = vec;
        destination = vec;
    }

    public void MoveAtPosition(Vector3 vec)
    {
        destination = vec;
    }

    public void ToggleActual()
    {
        isAnimated = !isAnimated;
    }

    void Update()
    {
        if(transform.position != destination)
            transform.position = Vector3.MoveTowards(transform.position, destination, speed*Time.deltaTime);
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
