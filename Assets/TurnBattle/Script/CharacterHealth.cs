using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBaseTest.Character {
    public class CharacterHealth : MonoBehaviour
    {
        public int health = 0;
        public SpriteRenderer fillBar;

        public void InitHelath(int amount) {
            SetAmountHealth(amount);
            Operate(true);
        }

        public void GetDamage(int amount)
        {
            if(amount <= 0) return;
            SetAmountHealth(health-amount);
        }

        private void SetAmountHealth(int amount)
        {
            health = amount;

            if (amount <= 0)
            {
                fillBar.size = new Vector2(0, 1);
            }
            else {
                fillBar.size = new Vector2((float)amount / 100f, 1);
            }
            
        }

        public void Operate(bool isActive) {
            gameObject.SetActive(isActive);
        }
    }
}

