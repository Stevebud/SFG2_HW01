using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : Enemy
{
    protected override void PlayerImpact(Player player)
    {
        //base.PlayerImpact(player);
        //player.Kill();
        Health health = player.GetComponent<Health>();
        if(health != null)
        {
            health.Kill();
        }
    }
}
