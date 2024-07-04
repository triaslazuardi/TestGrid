using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace TurnBaseTest.Character {
    public class CharacterBase : MonoBehaviour
    {
        [SerializeField] public Animator animCharacter = null;
        [SerializeField] public float durationAttact = 0f;

        public void CharacterIdle()
        {
            ResetAllAnimatorTriggers();
            animCharacter.SetTrigger("idle");
        }
        public void CharacterRun()
        {
            animCharacter.SetTrigger("run");
        }
        public void CharacterDie()
        {
            animCharacter.SetTrigger("die");
        }

        private void CharacterAttackType1()
        {
            animCharacter.SetTrigger("attack_1");

        }
        private void CharacterAttackType2()
        {
            ResetAllAnimatorTriggers();
            animCharacter.SetTrigger("attack_2");
        }

        public async void CHaracterAttact(Vector3 dirAttack, UnityAction callbackDamage , UnityAction callback) {
            CharacterAttackType2();
            //float waitTime = animCharacter.GetCurrentAnimatorStateInfo(0).normalizedTime % 1;
            await UniTask.WaitForSeconds(durationAttact-BattleHandler.GetInstance().dtGame.delay1);
            callbackDamage?.Invoke();
            await UniTask.WaitForSeconds(BattleHandler.GetInstance().dtGame.delay1);
            callback?.Invoke();
        }

        public void ResetAllAnimatorTriggers()
        {
            foreach (var trigger in animCharacter.parameters)
            {
                if (trigger.type == AnimatorControllerParameterType.Trigger)
                {
                    animCharacter.ResetTrigger(trigger.name);
                }
            }
        }

    }
}


