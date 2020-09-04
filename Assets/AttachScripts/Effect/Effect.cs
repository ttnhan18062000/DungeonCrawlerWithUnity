using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Effect : MonoBehaviour
{
    public float duration;
    public CharacterStatus targetStatus;
    protected List<Collider2D> hits;
    public float currentTime;
    int count;
    public List<string> names;

    private void Awake()
    {
    }

    protected void effectCollection()
    {
        List<Collider2D> hits = new List<Collider2D>();
        Physics2D.OverlapPoint(transform.position, new ContactFilter2D().NoFilter(), hits);

        Collider2D[] effects = hits.Where(h => names.Contains(h.name)).ToArray();
        if(effects.Count() > 1)
        { 
            (effects[0].gameObject.GetComponent(names[0]) as Effect).currentTime = 0;
            (effects[1].gameObject.GetComponent(names[0]) as Effect).currentTime = 0;
            Destroy(gameObject);
        }
        else
        {
            targetStatus = hits.Where(h => h.isTrigger==false).First().gameObject.GetComponent<CharacterStatus>();
            ApplyEffect();
        }
    }

    private void Update()
    {
    }

    public virtual void ApplyEffect()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(targetStatus==null && collision.isTrigger==false)
            targetStatus = collision.gameObject.GetComponent<CharacterStatus>();

    }

    private void OnDestroy()
    {
        
    }

    void RefreshCameraEffectUI()
    {
        
    }

    protected void SetAnimatorCameraEffectUI(bool value)
    {
        GameObject.FindGameObjectWithTag("CameraEffectUI").transform.GetChild(0).GetComponent<Animator>().SetBool("isActivated", value);
        GameObject.FindGameObjectWithTag("CameraEffectUI").transform.GetChild(1).GetComponent<Animator>().SetBool("isActivated", value);
    }

}
