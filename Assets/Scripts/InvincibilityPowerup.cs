using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityPowerup : PowerUpBase
{

    private Color _startingColor;
    private GameObject bodyObject = null;
    protected override void PowerDown(Player player)
    {
        if(player != null)
        {
            player.invincible = false;
        }
        if (bodyObject != null)
        {
            Renderer bodyRend = bodyObject.GetComponent<Renderer>();
            if (bodyRend != null)
            {
                //reset the color to indicate power down
                bodyRend.material.color = _startingColor;
            }
        }
    }

    protected override void PowerUp(Player player)
    {
        Transform Art = null;
        Transform body = null;
        if (player != null)
        {
            player.invincible = true;
            Art = player.transform.Find("Art");
            if (Art != null)
            {
                body = Art.transform.Find("Body");
                if (body != null)
                {
                    bodyObject = body.gameObject;
                    if (bodyObject != null)
                    {
                        Renderer bodyRend = bodyObject.GetComponent<Renderer>();
                        if(bodyRend != null)
                        {
                            //save the starting color and change color to cyan
                            _startingColor = bodyRend.material.color;
                            bodyRend.material.color = Color.cyan;
                        }
                    }
                }
            }
        }
    }
}
