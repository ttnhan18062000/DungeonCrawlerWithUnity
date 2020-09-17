using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : MonoBehaviour
{

    public bool isToggled = false;
    public bool isDashed = false;
    public float dashSpeed;
    public float dashTime;
    public float dashStartTime;
    public float dashDelay;
    public float dashStartDelay;
    public Rigidbody2D body;
    public Vector2 direction;
    public GameObject character;
    // Start is called before the first frame update

    public void LoadDashStats()
    {
        CharacterStatus character = gameObject.GetComponent<CharacterStatus>();
        dashDelay = 1.25f / (1f + character.character.GetStats("Reflexes") / 2);
        dashSpeed = character.character.GetStats("Speed") * 3;
    }
    private void Awake()
    {

        body = gameObject.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        LoadDashStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashed == true)
        {
            if (dashStartTime > 0)
            {
                body.velocity += direction * dashSpeed;
                if (body.velocity.magnitude > dashSpeed)
                    body.velocity = body.velocity.normalized * dashSpeed;
                dashStartTime -= Time.deltaTime;
                character.tag = "Untargetable";
            }
            else
            {
                body.velocity = Vector2.zero;
                isDashed = false;
                isToggled = false;
                character.tag = "Character";
                dashStartDelay = dashDelay;
            }
        }
        if (dashStartDelay <= 0)
        {
            if (isDashed == false && direction != Vector2.zero && isToggled == true)
            {
                dashStartTime = dashTime;
                isDashed = true;
            }
        }
        else
        {
            isToggled = false;
            dashStartDelay -= Time.deltaTime;
        }

    }
}
