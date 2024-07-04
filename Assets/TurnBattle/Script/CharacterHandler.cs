using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using TurnBaseTest.Enum;
using UnityEngine;
using UnityEngine.Events;

namespace TurnBaseTest.Character {
    public class CharacterHandler : MonoBehaviour
    {
        [SerializeField] private CharacterBase baseCharacter;
        [SerializeField] private CharacterHealth healthCharacter;
        [SerializeField] private Transform transformCharacter;
        [SerializeField] private Transform transformSign;

        private StateSliding state;
        private Vector3 slideTargetPosition;
        private UnityAction onSlideComplete;

        public bool IsPlayer = false;

        private void Update()
        {
            switch (state)
            {
                case StateSliding.Idle:
                    break;
                case StateSliding.Progress:
                    break;
                case StateSliding.Sliding:
                    transform.position += (slideTargetPosition-GetPosition()) * BattleHandler.GetInstance().dtGame.slideSpeed * Time.deltaTime;

                    Debug.Log("[Distance] " + Vector3.Distance(GetPosition(), slideTargetPosition) + ", reach : " + BattleHandler.GetInstance().dtGame.reachedDistance);
                    if (Vector3.Distance(GetPosition(), slideTargetPosition) <= BattleHandler.GetInstance().dtGame.reachedDistance) {
                        transform.position = slideTargetPosition;
                        onSlideComplete?.Invoke();
                    }
                    break;
            }
        }

        public void InitCharacter(bool _isplayer)
        {
            IsPlayer = _isplayer;
            baseCharacter.CharacterIdle();
            state = StateSliding.Idle;
            SetupScaleRotation();
            healthCharacter.InitHelath(100);
        }

        public void SetupScaleRotation() {
            if (IsPlayer)
            {
                transformCharacter.localScale = Vector3.one;
            }
            else {
                transformCharacter.localScale = new Vector3(-1,1,1);
            }
        }

        public void SetDie()
        {
            baseCharacter.CharacterDie();
            healthCharacter.Operate(false);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public bool IsDie()
        {
            return (healthCharacter.health <= 0);
        }

        public void Attack(CharacterHandler targetChar, UnityAction OnAttackComplete = null) {
            Vector3 slideTargetposition = targetChar.GetPosition() + (GetPosition() - targetChar.GetPosition()).normalized * BattleHandler.GetInstance().dtGame.rangeDistance;
            Vector3 startingPosition = GetPosition();
            transformSign.gameObject.SetActive(true);

            SlideToPosition(slideTargetposition, () =>
            {
                state = StateSliding.Progress;
                Vector3 attactDir = (targetChar.GetPosition() - GetPosition().normalized);
                baseCharacter.CHaracterAttact(attactDir,() => {
                    var fxItem = LeanPool.Spawn(BattleHandler.GetInstance().dtGame.fxBlood, targetChar.transform);
                    fxItem.transform.localPosition = BattleHandler.GetInstance().dtGame.vecFx;
                    fxItem.RunFx();
                    targetChar.healthCharacter.GetDamage(BattleHandler.GetInstance().dtGame.GetRandomeDamage());
                    SoundManager.GetInstance().PlaySFX("attack");
                }, () =>
                {
                    SlideToPosition(startingPosition, () =>
                    {
                        state = StateSliding.Idle;
                        baseCharacter.CharacterIdle();
                        transformSign.gameObject.SetActive(false);
                        OnAttackComplete?.Invoke();
                    });
                });
            });
        }

        private void SlideToPosition(Vector3 _targetPos, UnityAction _OnSlideComplete) { 
            this.slideTargetPosition = _targetPos;
            this.onSlideComplete = _OnSlideComplete;
            state = StateSliding.Sliding;
        }
    }
}

