﻿using System;
using System.Collections;

using UnityEngine;

using Helpers;
using FMODUnity;
using MyBox;

namespace Player
{
    public partial class PlayerStateMachine : StateMachine<PlayerStateMachine, PlayerStateMachine.PlayerState, PlayerStateInput> {
        private PlayerAnimationStateManager _playerAnim;
        private SpriteRenderer _spriteR;
        private StudioEventEmitter _drillEmitter;
        public event Action OnPlayerRespawn;

        public bool UsingDrill => IsOnState<Diving>() || IsOnState<Dogoing>();

        #region Overrides
        protected override void SetInitialState() 
        {
            SetState<Grounded>();
            _playerAnim.Play(PlayerAnimations.SLEEPING);
        }

        protected override void Init()
        {
            _playerAnim = GetComponentInChildren<PlayerAnimationStateManager>();
            _spriteR = GetComponentInChildren<SpriteRenderer>();
            _drillEmitter = GetComponentInChildren<StudioEventEmitter>();

            OnPlayerRespawn += () =>
            {
                _spriteR.SetAlpha(1);
                PlayerCore.SpawnManager.Respawn();
                ShaderRespawn();
                Transition<Airborne>();
            };
        }


    private IEnumerator ShaderRespawnCo() {
        _spriteR.material.SetFloat("_Progress", 0);
        float timer = 0;
        float spawnAnimTime = .5f;
        while (timer < spawnAnimTime) {
            timer += Time.deltaTime;
            _spriteR.material.SetFloat("_Progress", timer/spawnAnimTime);
            yield return null;
        }
        _spriteR.material.SetFloat("_Progress", 2);
    }

    public void ShaderRespawn() {
        StartCoroutine(ShaderRespawnCo());
    }

        protected override void Update()
        {
            base.Update();

            if (PlayerCore.Input.JumpStarted())
            {
                CurrState.JumpPressed();
            }

            if (PlayerCore.Input.JumpFinished())
            {
                CurrState.JumpReleased();
            }

            if (PlayerCore.Input.DiveStarted())
            {
                CurrState.DivePressed();
            }

            if (PlayerCore.Input.RetryStarted())
            {
                PlayerCore.Actor.Die(PlayerCore.Actor.transform.position);
            }
            
            CurrInput.moveDirection = PlayerCore.Input.GetMovementInput();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            GameTimer.FixedUpdate(CurrInput.jumpBufferTimer);
            CurrState.SetGrounded(PlayerCore.Actor.IsGrounded(), PlayerCore.Actor.IsMovingUp);;
            CurrState.MoveX(CurrInput.moveDirection);
        }
        #endregion

        public void RefreshAbilities()
        {
            CurrState.RefreshAbilities();
        }

        public void OnDeath()
        {
            _spriteR.SetAlpha(0);
            Transition<Dead>();
        }
    }
}