using System.Collections;
using System.Collections.Generic;
using TurnBaseTest.Character;
using UnityEngine;

namespace TurnBaseTest.So {
    [CreateAssetMenu(fileName = "Custom Item SO", menuName = "Scriptable Object/Game Default Data")]
    public class DataSO : ScriptableObject
    {
        [Header("ValueAttribute")]
        public int distanceCharX = 0;
        public int distanceCharY = 0;
        public int slideSpeed = 0;
        public float reachedDistance = 0;
        public float rangeDistance = 0;
        public Vector3 vecFx;

        [SerializeField] private List<int> damageVal;
        [SerializeField] private bool isPlayerAlwaysWin;


        [Header("DelayNumber")]
        public float delay1 = 0.125f;

        [Header("Fx")]
        public FxManage fxBlood = null;

        public int GetRandomeDamage() {
            if (isPlayerAlwaysWin) {
                return damageVal[0];
            }
            else
            {
                return damageVal[UnityEngine.Random.Range(0, damageVal.Count)];
            }
        }
    }
}

