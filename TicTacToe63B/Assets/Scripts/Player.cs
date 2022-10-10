using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public Id id;
    public Fruit.FruitType assignedFruit;
    public string nickname;    

 
    public enum Id
    {
        Player1=1,
        Player2=2
    }

}
