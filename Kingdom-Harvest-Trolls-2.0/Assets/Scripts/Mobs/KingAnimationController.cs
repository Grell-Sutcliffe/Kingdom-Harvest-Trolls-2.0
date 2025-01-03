using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingAnimationController : MonoBehaviour
{
    KnightController knightController;

    private Animator _animator;
    private Rigidbody2D _body;

    private void Start()
    {
        knightController = GetComponent<KnightController>();
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _animator.SetFloat("velocity", ((knightController.target == null) || (knightController.is_attacing == true)) ? (0) : (1));
    }
}