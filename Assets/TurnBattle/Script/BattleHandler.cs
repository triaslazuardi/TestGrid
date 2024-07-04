using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using TurnBaseTest.Enum;
using TurnBaseTest.So;
using TurnBaseTest.Character;
using UnityEngine;
using UnityEngine.Networking;

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

        private string apiServer = "https://6686f9fe83c983911b043c72.mockapi.io/GetRandom/getRandom";

        [Range(0, 2)]
        public int currenIdxPlayer = 0;

        [Range(0, 2)]
        public int currenIdxEnemy = 0;

        bool isProgress = false;

        private void Awake()
        {
            instance = this;
            state = StateAttack.Done;
            scrUi.SetPlay();
        }

        public void OnPlay() {
            if (isProgress) return;
            isProgress = true;

            if (scrUi.toggleServer.isOn)
            {
                StartCoroutine(playGameWaitServer());
            }
            else {
                playGame();
            }
        }

        public void OnPlayAgain()
        {
            if (isProgress) return;
            isProgress = true;

            LeanPool.Despawn(charPlayerHandle);
            LeanPool.Despawn(charEnemyHandle);

            if (scrUi.toggleServer.isOn)
            {
                StartCoroutine(playGameWaitServer());
            }
            else {
                playGame();
            }
        }

        public void playGame() {
            currenIdxPlayer = 0;
            currenIdxEnemy = 0;
            charPlayerHandle = SpawnCharacter(true);
            charEnemyHandle = SpawnCharacter(false);
            SetActiveChar(charPlayerHandle);
            state = StateAttack.WaitingPlayer;
            scrUi.SetDefault();
            SoundManager.GetInstance().PlaySFX("play");
            isProgress = false;
        }

        public IEnumerator playGameWaitServer()
        {
            scrUi.OperateBlock(true);
            using (UnityWebRequest webRequest = UnityWebRequest.Get(apiServer)) {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("[Server] Error: " + webRequest.error);
                    playGame();
                    scrUi.toggleServer.isOn = false;
                }
                else
                {
                    currenIdxPlayer = UnityEngine.Random.Range(0, prefabCharPlayer.Count);
                    currenIdxEnemy = UnityEngine.Random.Range(0, prefabCharEnemy.Count);
                    Debug.Log($"[Server] success: {currenIdxPlayer}, enemy {currenIdxEnemy}");
                    charPlayerHandle = SpawnCharacter(true);
                    charEnemyHandle = SpawnCharacter(false);
                    SetActiveChar(charPlayerHandle);
                    state = StateAttack.WaitingPlayer;
                    scrUi.SetDefault();
                    SoundManager.GetInstance().PlaySFX("play");
                    isProgress = false;
                }
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
            if (state == StateAttack.Done) return;

            if (BattleOver())
            {
                state = StateAttack.Done;
                SoundManager.GetInstance().PlaySFX("end");
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

