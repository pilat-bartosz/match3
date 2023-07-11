using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public CanvasScaler canvasScaler;
    public GridManager gridManager;
    public ObjectiveManager objectiveManager;

   

    Item actualItem;

    void Start()
    {
        FixCanvasScaleToFitResolutoion();
    }

    public void ItemClicked(Item item)
    {
        //Debug.Log(item.type + " " + item.transform.position);
        if (actualItem)
        {
            if (actualItem.Equals(item))
            {
                actualItem.ToggleActual();
                actualItem = null;
            }else 
            if (gridManager.CheckIfNeighbors(actualItem, item))
            {
                gridManager.SwitchPositions(actualItem, item);
                item.MoveAtPosition(actualItem.transform.position);
                actualItem.MoveAtPosition(item.transform.position);
                if (gridManager.TakeTurn())
                {
                    Debug.Log("switch back");
                    gridManager.SwitchPositions(actualItem, item);
                    item.MoveAtPosition(item.transform.position);
                    actualItem.MoveAtPosition(actualItem.transform.position);
                }
                actualItem.ToggleActual();
                actualItem = null;
            }
            else
            {
                actualItem.ToggleActual();
                actualItem = item;
                actualItem.ToggleActual();
            }

        }
        else
        {
            actualItem = item;
            actualItem.ToggleActual();
        }

        while (!gridManager.HaveAnotherMove())
        {
            gridManager.SuffleGrid();
        }
        gridManager.TakeTurn();

        if (objectiveManager.IsObjectiveComplete())
            EndGame();
    }

    void FixCanvasScaleToFitResolutoion()
    {
        Debug.Log("Screen width - "+Camera.main.pixelWidth);
        canvasScaler.matchWidthOrHeight = 1 - (float)Camera.main.pixelWidth / 1280;
    }

    public void ShortGame()
    {
        Debug.Log("ShortGame");
        int[] toDo = new int[] { 10, 10, 10, 10, 10, 10 };
        StartGame(toDo);
    }
    public void NormalGame()
    {
        Debug.Log("NormalGame");
        int[] toDo = new int[] { 20,20, 20, 20, 20, 20 };
        StartGame(toDo);
    }
    public void LongGame()
    {
        Debug.Log("LongGame");
        int[] toDo = new int[] { 30, 30, 30, 30, 30, 30 };
        StartGame(toDo);
    }
    public void EndlessGame()
    {
        Debug.Log("EndlessGame");
        int[] toDo = new int[] { 100, 100, 100, 100, 100, 100 };
        StartGame(toDo);
    }
    void StartGame(int[] toDoArray)
    {
        objectiveManager.Initialize(toDoArray);
        gridManager.StartGame();
        objectiveManager.StartGame();
        //ShowGameUI();
    }
    void EndGame()
    {
        objectiveManager.EndGame();
        gridManager.EndGame();
        ToggleCongratulationsUI();
    }
    public void Congrats()
    {
        ToggleCongratulationsUI();
        //ShowMenuUI();
    }

    void ToggleCongratulationsUI()
    {
        //Debug.Log(!CongratulationsUI.activeInHierarchy);
        //CongratulationsUI.SetActive(!CongratulationsUI.activeInHierarchy);
    }
    public void ToMenu()
    {
        objectiveManager.EndGame();
        gridManager.EndGame();
        //ShowMenuUI();
    }
    
}
