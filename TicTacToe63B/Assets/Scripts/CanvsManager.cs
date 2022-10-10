using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CanvsManager : MonoBehaviour
{
    public GameManager gameManager;
    
    public Sprite fruitApple;
    public Sprite fruitStrawberry;
    public Sprite fruitEmpty;

    // Start is called before the first frame update
    private void Start()
    {
        AssignBoardPieceClicks();
    }
       
    private void AssignBoardPieceClicks()
    {
        for (int i=0; i < 3; i++)
        {
            for(int j=0; j<3; j++)
            {
                GameObject boardPiece = this.transform.Find("PanelBoardPieces/Loc" + i + "-" + j).gameObject;
                boardPiece.GetComponent<BoardPiece>().row = i;
                boardPiece.GetComponent<BoardPiece>().column = j;

                EventTrigger eventTrigger = boardPiece.GetComponent<EventTrigger>();             
                EventTrigger.Entry entry = new EventTrigger.Entry();
            
                entry.eventID = EventTriggerType.PointerClick;             
                entry.callback = new EventTrigger.TriggerEvent();
           
                UnityEngine.Events.UnityAction<BaseEventData> callback =
                    new UnityEngine.Events.UnityAction<BaseEventData>(GameBoardPieceEventMethod);
            
                
                entry.callback.AddListener(callback);         
                eventTrigger.triggers.Add(entry);
            }
        }
    }

    public void GameBoardPieceEventMethod(UnityEngine.EventSystems.BaseEventData baseEvent)
    {           
        PointerEventData bEvent = (PointerEventData)baseEvent;
        print(bEvent.pointerClick.gameObject.name);
        gameManager.SelectBoardPiece(bEvent.pointerClick.gameObject);
        
    }

    public void BoardPaint(GameObject gameObjBoardPiece)
    {
        print("inside boardpaint");
        BoardPiece boardPiece = gameObjBoardPiece.GetComponent<BoardPiece>();
        if (boardPiece.GetFruit() == Fruit.FruitType.Apple)
            gameObjBoardPiece.GetComponent<Image>().sprite = fruitApple;
        else if (boardPiece.GetFruit() == Fruit.FruitType.Strawberry)
            gameObjBoardPiece.GetComponent<Image>().sprite = fruitStrawberry;
        else
            gameObjBoardPiece.GetComponent<Image>().sprite = fruitEmpty;
    }

    public void ChangeBottomLabel(string message)
    {
        transform.Find("PanelBottom/LblMessage").GetComponent<TextMeshProUGUI>().text = message;
    }

    public void ChangeTopNames(string player1Name, string player2Name)
    {
        transform.Find("PanelTop/Player1Label").GetComponent<TextMeshProUGUI>().text = player1Name;
        transform.Find("PanelTop/Player2Label").GetComponent<TextMeshProUGUI>().text = player2Name;
    }



}
