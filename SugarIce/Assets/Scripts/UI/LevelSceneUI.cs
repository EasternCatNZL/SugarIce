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

    private Text timerText;
    private Text scoreText;

    [Header("UI Positioning")]
    public List<Transform> orderPositions = new List<Transform>();

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
        GameObject menuItemClone = Instantiate(orderVisual, orderPositions[posIndex].position, Quaternion.identity);
        //do animation?
    }

    //Logic when order is completed or expired
    public void RemoveOrderFromUI(int posIndex)
    {

    }

    //Logic to rearrange orders when one is removed ahead of last in list
    private void RearrangeOrdersInUI(int posIndex)
    {

    }

    //set text ui
    public void ScoreTextUpdate(float scoreVal)
    {
        scoreText.text = scoreVal.ToString();
    }
}
