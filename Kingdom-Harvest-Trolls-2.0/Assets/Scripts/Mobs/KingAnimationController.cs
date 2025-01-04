using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingAnimationController : MonoBehaviour
{
    KingController kingController;

    private Animator _animator;
    private Rigidbody2D _body;

    private void Start()
    {
        kingController = GetComponent<KingController>();
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _animator.SetFloat("velocity", (!kingController.is_moving) ? (0) : (1));
    }
}