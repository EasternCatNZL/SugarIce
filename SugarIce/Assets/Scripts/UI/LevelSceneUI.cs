using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelSceneUI : MonoBehaviour
{
    [Header("UI Components")]
    public string levelFinishMessage = "Finished!";
    [Tooltip("UI object that details order visually")]
    public GameObject orderVisual;
    public List<GameObject> orderVisualList = new List<GameObject>();
    public Text timerText;
    public Text scoreText;

    [Header("UI Positioning")]
    public List<RectTransform> orderPositions = new List<RectTransform>();
    public float orderMoveAlongTime = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Update timer text
    public void TimerTextUpdate(int minutesLeft, int secondsLeft)
    {
        timerText.text = minutesLeft + ":" + secondsLeft;
    }

    //Logic when order is created
    public void CreateOrderInUI(int posIndex, Product newOrder)
    {
        //create a new object at the specified index
        GameObject menuItemVisual = Instantiate(orderVisual, orderPositions[posIndex].position, Quaternion.identity);
        menuItemVisual.transform.SetParent(transform);
        //Set the image used based on product
        menuItemVisual.GetComponent<OrderItem>().orderImage = newOrder.productImage;
        //Add to list of visuals
        orderVisualList.Add(menuItemVisual);
        //do animation?
    }

    //Logic when order is completed or expired
    public void RemoveOrderFromUI(int posIndex)
    {
        //Get the object that needs to be removed
        GameObject toRemove = orderVisualList[posIndex];
        orderVisualList.RemoveAt(posIndex);
        //Destroy the removed object
        Destroy(toRemove);
        //Rearrange the orders based on what was removed and what else is in list
        RearrangeOrdersInUI(posIndex);
    }

    //Logic to rearrange orders when one is removed ahead of last in list
    private void RearrangeOrdersInUI(int posIndex)
    {
        //If from removed point so that all images move to new pos after one before vacated
        for(int i = posIndex; i < orderVisualList.Count; i++)
        {
            //If object in this index after i removed <- i+1 = i after i removed
            if (orderVisualList[i])
            {
                //Move the order visual to new pos
                orderVisualList[i].GetComponent<RectTransform>().DOAnchorPos(orderPositions[i].anchoredPosition, orderMoveAlongTime, false);
            }
            //else, no orders beyond this point
            else
            {
                return;
            }
        }
    }

    //set text ui
    public void ScoreTextUpdate(float scoreVal)
    {
        scoreText.text = "Score: " + scoreVal.ToString();
    }
}
