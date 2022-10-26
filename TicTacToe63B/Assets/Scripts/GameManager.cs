using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
        //players.Add(new Player() { id = Player.Id.Player1, nickname = "P1", assignedFruit = Fruit.FruitType.Apple });
        //players.Add(new Player() { id = Player.Id.Player2, nickname = "P2", assignedFruit = Fruit.FruitType.Strawberry });

        Photon.Realtime.Player[] allPlayers = PhotonNetwork.PlayerList;
        foreach(Photon.Realtime.Player player in allPlayers)
        {
            if (player.ActorNumber == 1)
                players.Add(new Player() { id = Player.Id.Player1, nickname = player.NickName, assignedFruit = Fruit.FruitType.Apple });
            else if (player.ActorNumber == 2)
                players.Add(new Player() { id = Player.Id.Player2, nickname = player.NickName, assignedFruit = Fruit.FruitType.Strawberry });
        }


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

			//check winner
			bool win = CheckWinner(boardPiece);

			if (win)
			{
				print("Detected win by " + currentActivePlayer.id);
				canvasManager.ChangeBottomLabel("Winner:" + currentActivePlayer.nickname);
				stopGame = true;
			}
			else
			{
				//check if game is draw
				if (IsGameDraw())
				{
					print("Game is draw");
					canvasManager.ChangeBottomLabel("Game Draw");
					stopGame = true;
				}
				else
				{
					print("Game is not draw. Continue playing...");
					ChangeActivePlayer();
				}
			}


        }

    }

    private bool CheckWinner(BoardPiece boardPiece)
	{
		//check the row where the user clicked
		int rowCounter = 0;
        for(int i=0; i < 3; i++)
		{
			BoardPiece tmpBoardPiece = BoardMap[boardPiece.row, i];
            if(tmpBoardPiece != null)
			{
				if (tmpBoardPiece.GetFruit() == boardPiece.GetFruit())
					rowCounter = rowCounter + 1;
			}
		}

        if(rowCounter == 3)
		{
			print("Similar in row");
			return true;
		}

		//check the column where the user clicked
		int colCounter = 0;
        for(int i=0; i<3; i++)
		{
			BoardPiece tmpBoardPiece = BoardMap[i, boardPiece.column];
            if(tmpBoardPiece != null)
			{
				if (tmpBoardPiece.GetFruit() == boardPiece.GetFruit())
					colCounter += 1;
			}

		}

        if(colCounter == 3)
		{
			print("Similar in column");
			return true;
		}

		//check diagonal 1
		int diagOneCounter = 0;
		int diagCol1 = -1;

        for(int i=0; i<3; i++)
		{
			diagCol1 += 1;
			BoardPiece tmpBoardPiece = BoardMap[i, diagCol1];
            if(tmpBoardPiece != null)
			{
                if(tmpBoardPiece.GetFruit() == boardPiece.GetFruit())
				{
					diagOneCounter += 1;
				}
			}
		}

        if(diagOneCounter == 3)
		{
			print("Similar in diagonal 1");
			return true;
		}

		//check diagonal 2
		int diagCol2 = 3;
		int diagTwoCounter = 0;
		for (int i = 0; i < 3; i++){
			diagCol2 = diagCol2 - 1;
			BoardPiece tmpBoardPiece = BoardMap[i, diagCol2];

            if(tmpBoardPiece != null)
			{
                if(tmpBoardPiece.GetFruit() == boardPiece.GetFruit())
				{
					diagTwoCounter += 1;
				}
			}
		}

		if (diagTwoCounter == 3) {
			print("Similar in diagonal 2");
			return true;

        }

		return false;

	}


    private bool IsGameDraw()
	{
        foreach(BoardPiece boardPiece in BoardMap)
		{
			if (boardPiece == null)
				return false;
		}

		return true;

	}
   


    

   

    
}
