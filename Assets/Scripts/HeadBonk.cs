using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class HeadBonk : MonoBehaviour
{
    //[SerializeField] private CharacterController controller;
    private Vector3 surfaceNormal;
    private Player player;

    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (!hit.gameObject.CompareTag("Player") && !hit.isTrigger)
        {
            player.hitHead = true;
        }

    }
}
