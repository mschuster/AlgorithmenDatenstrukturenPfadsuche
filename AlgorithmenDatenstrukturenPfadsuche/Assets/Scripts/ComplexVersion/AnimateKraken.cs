﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class AnimateKraken : MonoBehaviour {
    private Transform ship;

    private Animator controller;

    int hit = Animator.StringToHash("IsAttacking");

    float speed;

    void Start() {
        ship = GameObject.Find("Ship").transform;
        speed = 5f;
        controller = this.GetComponent<Animator>();
    }

    void Update() {
        Vector3 direction = ship.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, ship.position) < 15) {
            controller.SetBool("IsAttacking", true);
        } else {
            controller.SetBool("IsAttacking", false);
        }



    }
}
