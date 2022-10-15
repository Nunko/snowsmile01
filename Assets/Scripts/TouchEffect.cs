using System.Collections;
using UnityEngine;

public class TouchEffect : MonoBehaviour
{
    public Object touchDownEffectF;
    public Object touchClickEffectF;
    GameObject CurrentTouchDownEffect;
    Vector3 currentTouchDownEffectPos;
    GameObject CurrentTouchClickEffect;
    public Camera mainCamera;
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
        RunTouchDownEffect();
        StartCoroutine("RunTouchClickEffect");
        RunTouchUp();
    }

    void RunTouchDownEffect()
    {
        if (Input.GetMouseButtonDown(0) == true)
        {                        
            if (mainCamera.enabled == true)
            {
                Camera naniUICamera = GameObject.Find("UICamera").GetComponent<Camera>();                        
                if (naniUICamera.enabled == false)
                {                    
                    CheckCurrentTouchDownEffect();
                    currentTouchDownEffectPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    GenTouchDownParticle();
                }
            }
        }
    }

    IEnumerator RunTouchClickEffect()
    {        
        if (Input.GetMouseButton(0) == true && touchClickOn == false)
        {
            touchClickOn = true;
            if (mainCamera.enabled == true)
            {
                Camera naniUICamera = GameObject.Find("UICamera").GetComponent<Camera>();                        
                if (naniUICamera.enabled == false)
                {
                    while (touchClickOn == true)
                    {
                        currentTouchDownEffectPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                        GenTouchClickParticle();
                        yield return new WaitForSeconds(intervalOfTouchClickEffect);
                    }  
                }                              
            }
        }        
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

    void GenTouchClickParticle()
    {
        GameObject particle = spawnParticle(touchClickEffectF);
        Vector3 posTmp = currentTouchDownEffectPos + particle.transform.position; 
        particle.transform.position = new Vector3 (posTmp.x, posTmp.y, 0);
        CurrentTouchClickEffect = particle;
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
}
