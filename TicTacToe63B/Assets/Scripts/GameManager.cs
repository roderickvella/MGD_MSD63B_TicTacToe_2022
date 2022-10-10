using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Player> players = new List<Player>();
    public Player currentActivePlayer;
    public CanvsManager canvasManager;
    public BoardPiece[,] BoardMap = new BoardPiece[3, 3];
    public bool stopGame = false;

    // Start is called before the first frame update
    void Start()
    {
        players.Add(new Player() { id = Player.Id.Player1, nickname = "P1", assignedFruit = Fruit.FruitType.Apple });
        players.Add(new Player() { id = Player.Id.Player2, nickname = "P2", assignedFruit = Fruit.FruitType.Strawberry });

        ChangeTopName();
        ChangeActivePlayer();
    }

  
    private void ChangeTopName()
    {
       canvasManager.ChangeTopNames(players.Find(x => x.id == Player.Id.Player1).nickname, players.Find(x => x.id == Player.Id.Player2).nickname);
    }

    public void ChangeActivePlayer()
    {
        if (currentActivePlayer == null)
            currentActivePlayer = players.Find(x => x.id == Player.Id.Player1); //by default set player1 as active player
        else if (currentActivePlayer.id == Player.Id.Player1)
            currentActivePlayer = players.Find(x => x.id == Player.Id.Player2);
        else if(currentActivePlayer.id == Player.Id.Player2)
            currentActivePlayer = players.Find(x => x.id == Player.Id.Player1);

        //notify CavasManager that player is changed
        canvasManager.ChangeBottomLabel("Player Turn:"+currentActivePlayer.nickname);
    }

    public void SelectBoardPiece(GameObject gameObjBoardPiece)
    {
        if (stopGame)
            return;

        BoardPiece boardPiece = gameObjBoardPiece.GetComponent<BoardPiece>();
       
        if (boardPiece.GetFruit() == Fruit.FruitType.None) //if still empty
        {                 
            //set fruit according to playing player
            boardPiece.SetFruit(currentActivePlayer.assignedFruit);

     
            //notify canvas manager to draw updated board
            canvasManager.BoardPaint(gameObjBoardPiece);
            
            //update BoardMap
            BoardMap[boardPiece.row, boardPiece.column] = boardPiece;

            //check winner - This is not implemented yet...found another job with a better salary ;)


        }

    }

   


    

   

    
}
