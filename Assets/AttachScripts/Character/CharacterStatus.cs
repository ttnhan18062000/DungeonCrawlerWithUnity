using Assets.Scripts;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    public Character character;
    public float speed;
    public float maxHP;
    public float currentHP;
    public string tittle;
    public int Strength;
    public int Endurance;
    public int Dexterity;
    public int Perception;
    public bool dead;

    public List<string> onDeathImpact;
    // Start is called before the first frame update
    void Start()
    {
        dead = false;
        try
        {
            character.SetAttribute("Strength", Strength);
            character.SetAttribute("Endurance", Endurance);
            character.SetAttribute("Dexterity", Dexterity);
            character.SetAttribute("Perception", Perception);
            character.UpdateStats();
            UpdateCharacterScaleAttribute();
            LoadCharacter();
        }
        catch { }
    }

    public void UpdateCharacterScaleAttribute()
    {
        gameObject.GetComponent<CharacterScript>().LoadCharacterStats();
        gameObject.GetComponent<ShootingScript>().LoadWeaponStats();
        gameObject.GetComponent<DashAbility>().LoadDashStats();
    }

    private void Explosion()
    {
        gameObject.GetComponent<Explosion>().Explode();
    }

    private void DropStuff()
    {
        gameObject.GetComponent<EnemyDropStuffScript>().EnemyDropStuff(gameObject);
    }

    private void OnDeath()
    {
        foreach (string s in onDeathImpact)
            Invoke(s, 0f);
        transform.GetChild(1).GetComponent<Animator>().SetBool("isDead", true);
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
        Destroy(gameObject, 0.5f);
    }

    public void LoadCharacter()
    {
        speed = character.GetStats("Speed");
        maxHP = character.GetStats("Health");
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        if (currentHP <= 0 && dead == false)
        {
            OnDeath();
            GetComponent<Collider2D>().enabled = false;
            transform.GetChild(1).gameObject.SetActive(false);
            dead = true;
        }
    }
}
