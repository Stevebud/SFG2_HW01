using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : CollectibleBase
{
    [SerializeField] int _treasureValue = 1;

    //when the player collects the treasure,
    protected override void Collect(Player player)
    {
        //increase the Player _treasureCount by _treasureValue
        player.IncreaseTreasure(_treasureValue);
    }

    protected override void Movement(Rigidbody rb)
    {
        //rotate around the z axis
        Quaternion turnOffset = Quaternion.Euler(0, 0, MovementSpeed);
        rb.MoveRotation(rb.rotation * turnOffset);
    }
}
