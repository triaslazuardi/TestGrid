using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TurnBaseTest {

    public class UIManage : MonoBehaviour
    {
        [SerializeField] private GameObject objWinLose;
        [SerializeField] private GameObject objPlay;
        public Toggle toggleServer;
        [SerializeField] private TMP_Text txtWinLose;
        [SerializeField] private CanvasGroup canvasUI;


        public void SetWinner(bool isPlayer) {
            if (isPlayer)
            {
                txtWinLose.text = "Player Wins";
            }
            else {
                txtWinLose.text = "Player Lose";
            }
            objWinLose.SetActive(true);
            toggleServer.gameObject.SetActive(true);
            AnimateCanvas();
        }

        public void SetPlay()
        {
            objPlay.SetActive(true);
            objWinLose.SetActive(false);
            toggleServer.gameObject.SetActive(true);
            AnimateCanvas();
        }

        public void SetDefault() {
            canvasUI.DOFade(0, BattleHandler.GetInstance().dtGame.delay1).OnComplete(() => {
                objPlay.SetActive(false);
                objWinLose.SetActive(false);
                toggleServer.gameObject.SetActive(false);
                canvasUI.alpha = 0f;
            });
        }

        public void AnimateCanvas() {
            canvasUI.DOFade(1, BattleHandler.GetInstance().dtGame.delay1 * 2);
        }

        public void OnSettingSound() { 
        
        }
    }

}


