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
        [SerializeField] private GameObject objBlock;

        public Toggle toggleServer;
        public Toggle toggleSoundBgm;
        public Toggle toggleSoundSfx;

        [SerializeField] private TMP_Text txtWinLose;
        [SerializeField] private CanvasGroup canvasUI;

        private void Start()
        {
            Debug.Log("[sound] prefbgm :  " + PlayerPrefs.GetInt("bgmSound"));
            Debug.Log("[sound] prefsfx :  " + PlayerPrefs.GetInt("sfxSound"));

            //CheckSound();
            OperateBgm(PlayerPrefs.GetInt("bgmSound", 1) >= 1 ? true : false);
            OperateSfx(PlayerPrefs.GetInt("sfxSound", 1) >= 1 ? true : false);

            
        }
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
                canvasUI.blocksRaycasts = false;
                OperateBlock(false);
            });
        }

        public void AnimateCanvas() {
            canvasUI.DOFade(1, BattleHandler.GetInstance().dtGame.delay1 * 2);
            canvasUI.blocksRaycasts = true;
        }

        public void OperateBlock(bool isActive) {
            objBlock.SetActive(isActive);
        }

        #region Sound 

        //public void CheckSound() {
        //    toggleSoundBgm.isOn = PlayerPrefs.GetInt("bgmSound", 1) >= 1 ? true : false;
        //    toggleSoundSfx.isOn = PlayerPrefs.GetInt("sfxSound", 1) >= 1 ? true : false;
        //}

        public void OnBGmSound()
        {
            OperateBgm(toggleSoundBgm.isOn);
        }

        public void OnSfxSound()
        {
            OperateSfx(toggleSoundSfx.isOn);
        }

        public void OperateBgm(bool isActive) {
            toggleSoundBgm.isOn = isActive;
            SoundManager.GetInstance().MuteSoundBGM(isActive);
        }

        public void OperateSfx(bool isActive) {
            toggleSoundSfx.isOn = isActive;
            SoundManager.GetInstance().MuteSoundSFX(isActive);
        }
        #endregion


    }

}


