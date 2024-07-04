using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using TurnBaseTest.Enum;
using TurnBaseTest.So;
using TurnBaseTest.Character;
using UnityEngine;

namespace TurnBaseTest {
    public class BattleHandler : MonoBehaviour
    {
        private static BattleHandler instance;
        public static BattleHandler GetInstance()
        {
            return instance;
        }

        public DataSO dtGame;
        [SerializeField] private UIManage scrUi;

        [SerializeField] private List<CharacterHandler> prefabCharPlayer;
        [SerializeField] private List<CharacterHandler> prefabCharEnemy;
        private CharacterHandler charPlayerHandle;
        private CharacterHandler charEnemyHandle;
        private CharacterHandler activeCharHandle;
        public StateAttack state;

        [Range(0, 2)]
        public int currenIdxPlayer = 0;

        [Range(0, 2)]
        public int currenIdxEnemy = 0;

        private void Awake()
        {
            instance = this;
            state = StateAttack.Done;
            scrUi.SetPlay();
        }

        public void OnPlay() {
            if (scrUi.toggleServer.isOn)
            {
            }
            else {
                charPlayerHandle = SpawnCharacter(true);
                charEnemyHandle = SpawnCharacter(false);
                SetActiveChar(charPlayerHandle);
                state = StateAttack.WaitingPlayer;
                scrUi.SetDefault();
            }
        }

        public void OnPlayAgain()
        {
            LeanPool.Despawn(charPlayerHandle);
            LeanPool.Despawn(charEnemyHandle);

            if (scrUi.toggleServer.isOn)
            {

            }
            else {
                charPlayerHandle = SpawnCharacter(true);
                charEnemyHandle = SpawnCharacter(false);
                SetActiveChar(charPlayerHandle);
                state = StateAttack.WaitingPlayer;
                scrUi.SetDefault();
            }
            
        }

        private void Update()
        {
            if (state == StateAttack.WaitingPlayer) {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    state = StateAttack.Progress;
                    charPlayerHandle.Attack(charEnemyHandle, () => {
                        ChooseNextChar();
                    });
                }
            }
            
        }
        public CharacterHandler SpawnCharacter(bool isPlayerTeam)
        {
            Vector3 position;
            CharacterHandler charCurrent = null;

            if (isPlayerTeam)
            {
                position = new Vector3((0 - dtGame.distanceCharX), dtGame.distanceCharY);
                charCurrent = LeanPool.Spawn(prefabCharPlayer[currenIdxPlayer], position, Quaternion.identity);
            }
            else
            {
                position = new Vector3(dtGame.distanceCharX, dtGame.distanceCharY);
                charCurrent = LeanPool.Spawn(prefabCharEnemy[currenIdxEnemy], position, Quaternion.identity);
            }

            charCurrent.InitCharacter(isPlayerTeam);
            return charCurrent;
        }

        private void SetActiveChar(CharacterHandler characterHandler) {
            activeCharHandle = characterHandler;
        }

        private void ChooseNextChar()
        {
            if (BattleOver())
            {
                state = StateAttack.Done;
                return;
            }

            if (activeCharHandle == charPlayerHandle)
            {
                SetActiveChar(charEnemyHandle);
                state = StateAttack.Progress;

                charEnemyHandle.Attack(charPlayerHandle, () => {
                    ChooseNextChar();
                });
            }
            else {
                SetActiveChar(charPlayerHandle);
                state = StateAttack.WaitingPlayer;
            }
        }

        private bool BattleOver() { 
            if (charPlayerHandle.IsDie()) {
                charPlayerHandle.SetDie();
                scrUi.SetWinner(false);
                return true;
            }

            if (charEnemyHandle.IsDie()) {
                charEnemyHandle.SetDie();
                scrUi.SetWinner(true);
                return true;
            }

            return false;
        }
    }
}

