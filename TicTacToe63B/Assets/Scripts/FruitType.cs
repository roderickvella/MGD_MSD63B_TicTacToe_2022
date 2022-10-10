using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private FruitType fruitType;

    public Fruit()
    {
        this.fruitType = FruitType.None;
    }

    public FruitType GetFruit()
    {
        return this.fruitType;
    }

    public void SetFruit(FruitType fruitType)
    {
        print("SEt FRUIT");
        this.fruitType = fruitType;
    }

    public enum FruitType
    {
        None,
        Strawberry,
        Apple
    }
}
