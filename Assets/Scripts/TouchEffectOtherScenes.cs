using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class TouchEffectOtherScenes : MonoBehaviour
{
    public Object touchDownEffectF;
    public Object touchClickEffectF;
    GameObject CurrentTouchDownEffect;
    Vector3 currentTouchDownEffectPos;
    GameObject CurrentTouchClickEffect;
    public float intervalOfTouchClickEffect;
    bool touchClickOn;

    void Start()
    {
        CurrentTouchDownEffect = null;
        currentTouchDownEffectPos = Vector3.zero;
        CurrentTouchClickEffect = null;
        touchClickOn = false;
    }

    void Update()
    {
        if (SplashScreen.isFinished == true)
        {
            RunTouchDownEffect();
            StartCoroutine("RunTouchClickEffect");
            RunTouchUp();
        }        
    }

    void RunTouchDownEffect()
    {
        if (Input.GetMouseButtonDown(0) == true)
        {
            CheckCurrentTouchDownEffect();
            currentTouchDownEffectPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GenTouchDownParticle();
        }
    }

    void CheckCurrentTouchDownEffect()
    {
        if (CurrentTouchDownEffect != null)
        {
            ParticleSystem ps = CurrentTouchDownEffect.GetComponent<ParticleSystem>();
            if (ps.isEmitting)
            {
                ps.Stop(true);
            }
        }
    }

    void GenTouchDownParticle()
    {
        GameObject particle = spawnParticle(touchDownEffectF);
        Vector3 posTmp = currentTouchDownEffectPos + particle.transform.position; 
        particle.transform.position = new Vector3 (posTmp.x, posTmp.y, 0);
        CurrentTouchDownEffect = particle;
    }

    GameObject spawnParticle(Object inputPaticles)
    {
        GameObject particles = Instantiate(inputPaticles) as GameObject;
		particles.transform.position = new Vector3(0,particles.transform.position.y,0);
		#if UNITY_3_5
			particles.SetActiveRecursively(true);
		#else
			particles.SetActive(true);
			for(int i = 0; i < particles.transform.childCount; i++)
				particles.transform.GetChild(i).gameObject.SetActive(true);
		#endif
		
		return particles;
    }

    IEnumerator RunTouchClickEffect()
    {        
        if (Input.GetMouseButton(0) == true && touchClickOn == false)
        {
            touchClickOn = true;
            while (touchClickOn == true)
            {
                currentTouchDownEffectPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                GenTouchClickParticle();
                yield return new WaitForSeconds(intervalOfTouchClickEffect);
            }  
        }        
    }

    void GenTouchClickParticle()
    {
        GameObject particle = spawnParticle(touchClickEffectF);
        Vector3 posTmp = currentTouchDownEffectPos + particle.transform.position; 
        particle.transform.position = new Vector3 (posTmp.x, posTmp.y, 0);
        CurrentTouchClickEffect = particle;
    }

    void RunTouchUp()
    {
        if (Input.GetMouseButtonUp(0) == true)
        {
            if (touchClickOn == true)
            {
                touchClickOn = false;
            }
        }
    }
}
