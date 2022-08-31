using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Slower : Enemy
{
    [SerializeField] float _slowAmount = 0.1f;

    protected override void PlayerImpact(Player player)
    {
        TankController controller = player.GetComponent<TankController>();
        if (controller != null)
        {
            controller.MaxSpeed -= _slowAmount;
            if(controller.MaxSpeed < 0.1f)
            {
                controller.MaxSpeed = 0.1f;
            }
        }
    }
}
