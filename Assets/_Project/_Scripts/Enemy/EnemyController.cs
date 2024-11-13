using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    float currentTime;
    [SerializeField] float attackDelay;
    [SerializeField] float moveDistance;
    Vector3 initialPos;
    void Awake() {
        currentTime = 0;
        initialPos = transform.position;
    }
    void Update() {
        Tick();
    }

    void Tick() {
        currentTime += Time.deltaTime;
        if (currentTime >= attackDelay) {
            Attack();
            currentTime = 0;
        }
    }

    async void Attack() {
        await StepForward();
        await MoveForward();
        GameManager.Instance.Attack();
        await MoveBack();
    }


    async Task StepForward() {
        transform.position += transform.forward * (moveDistance * 0.25f);
        await Task.Delay(250);
    }
    async Task MoveForward() {
        transform.position += transform.forward * moveDistance;
        await Task.Delay(500);
    }

    async Task MoveBack() {
        transform.position = initialPos;
        await Task.Delay(0);
    }
}