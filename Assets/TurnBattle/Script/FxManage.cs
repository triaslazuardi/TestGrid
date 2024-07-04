using Cysharp.Threading.Tasks;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxManage : MonoBehaviour
{
    public ParticleSystem particleObj;
    public float durationParticle;

    public async void RunFx() { 
        gameObject.SetActive(true);
        particleObj.Play();


        await UniTask.WaitForSeconds(durationParticle);

        LeanPool.Despawn(this);
    }
}
