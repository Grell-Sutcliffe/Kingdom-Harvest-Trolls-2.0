using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerAnimationController : MonoBehaviour
{
    VillagerController villagerController;

    private Animator _animator;
    private Rigidbody2D _body;

    private void Start()
    {
        villagerController = GetComponent<VillagerController>();
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _animator.SetFloat("velocity", (villagerController.is_moving) ? (0) : (1));
    }
}
