using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
[SelectionBase]
public class PlayerController : MonoBehaviour {
    static readonly int TINT = Shader.PropertyToID("_Tint");
    static readonly int COLOR_TINT = Shader.PropertyToID("_ColorTint");
    [SerializeField] HealthComponent healthComponent;

    [SerializeField] float dodgeTime;
    [SerializeField] float blockTime;

    [SerializeField] MeshRenderer renderer;

    [SerializeField] Material normalShader, negativeShader;

    bool isDodging;

    Vector3 initialPos;

    void Awake() {
        initialPos = transform.position;
        healthComponent.Health = 3;
    }

    void OnEnable() {
        GameManager.AttackEvent += OnAttack;
    }

    void OnDisable() {
        GameManager.AttackEvent -= OnAttack;
    }

    void OnAttack() {
        if (isDodging) return;
        TakeDamage();
    }
    void TakeDamage() {
        if (--healthComponent.Health <= 0) {
            GameManager.Instance.EndGame();
            renderer.material = normalShader;
            renderer.material.SetColor(COLOR_TINT, Color.red);
        }
    }

    void Update() {
        if (healthComponent.Health <= 0) return;
        if (Keyboard.current.aKey.wasPressedThisFrame) {
            DodgeLeft();
        } else if (Keyboard.current.dKey.wasPressedThisFrame) {
            DodgeRight();
        } else if (Keyboard.current.spaceKey.wasPressedThisFrame) {
            Block();
        } else if (Mouse.current.leftButton.wasPressedThisFrame) {
            Attack();
        }
    }

    async void DodgeLeft() {
        isDodging = true;
        transform.position = initialPos - transform.right;
        SetNegative();
        await Task.Delay((int)(dodgeTime * 1000));
        UnsetNegative();
        transform.position = initialPos;
        isDodging = false;
    }

    async void DodgeRight() {
        isDodging = true;
        transform.position = initialPos + transform.right;
        SetNegative();
        await Task.Delay((int)(dodgeTime * 1000));
        UnsetNegative();
        transform.position = initialPos;
        isDodging = false;
    }

    async void Block() {
        isDodging = true;
        SetNegative();
        await Task.Delay((int)(blockTime * 1000));
        UnsetNegative();
        isDodging = false;
    }

    void SetNegative() {
        renderer.material = negativeShader;
    }

    void UnsetNegative() {
        renderer.material = normalShader;
    }


    async void Attack() {
        renderer.material.SetColor(COLOR_TINT, Color.blue);
        await Task.Delay(100);
        renderer.material.SetColor(COLOR_TINT, Color.white);
    }
}
