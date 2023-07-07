using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour
{
    public Image[] imagesToFill;

    int[] objectiveDone;
    int[] objectiveToDo;

    bool isCounting = false;

    void Start()
    {
        objectiveDone = new int[imagesToFill.Length];
        objectiveToDo = new int[imagesToFill.Length];
    }

    public void StartGame()
    {
        isCounting = true;
    }
    public void EndGame()
    {
        isCounting = false;
    }
    public void Initialize(int[] toDo)
    {
        Debug.Log("Initializing objectives");
        if (toDo.Length != imagesToFill.Length)
            Debug.LogError("Wrong objectives array lenght!");
        toDo.CopyTo(objectiveToDo, 0);
        for (int i = 0; i < objectiveDone.Length; i++)
        {
            objectiveDone[i] = 0;
        }
        
        ImageInitialize();
    }

    void ImageInitialize()
    {
        Debug.Log("Initializing objective images");
        for (int i = 0; i < imagesToFill.Length; i++)
        {
            if(objectiveToDo[i] <= 0)
                imagesToFill[i].gameObject.SetActive(false);
            else
            {
                imagesToFill[i].fillAmount = 0f;
            }
        }
    }

    public void AddProgress(int id, int amount)
    {
        if (isCounting)
        {
            objectiveDone[id] += amount;
            imagesToFill[id].fillAmount = ObjectiveProgress(id);
        }
    }

    public float ObjectiveProgress(int id)
    {
        return ((float)objectiveDone[id] / (float)objectiveToDo[id]);
    }

    public bool IsObjectiveComplete()
    {
        for (int i = 0; i < imagesToFill.Length; i++)
        {
            if (ObjectiveProgress(i) < 1.0f) return false;
        }
        return true;
    }
}
