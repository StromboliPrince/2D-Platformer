﻿using Hero.Command;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour, IHeroCommand
{
    private bool Active;
    private const float DURATION = 0.3f;
    private float ElapsedTime;
    private GameObject Player;
    private PolygonCollider2D SwordCollider;
    private PolygonCollider2D SwordColliderRev;
    private PolygonCollider2D AirSwordCollider;
    private PolygonCollider2D AirSwordColliderRev;
    private int Counter = 0;

    private void Start()
    {
        this.Active = false;
        this.ElapsedTime = 0.0f;
        //SwordCollider.enabled = false;
        //SwordColliderRev.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (this.Active)
        {
            this.ElapsedTime += Time.deltaTime;

            if (!Player.GetComponent<SpriteRenderer>().flipX)
            {
                var contacts = new Collider2D[6];
                if (Player.GetComponent<HeroController>().ground)
                {
                SwordCollider.enabled = true;
                    this.SwordCollider.GetContacts(contacts);
                }
                else
                {
                    AirSwordCollider.enabled = true;
                    this.AirSwordCollider.GetContacts(contacts);
                }
                foreach (var col in contacts)
                {
                    Counter += 1;
                    if (Counter > 5)
                    {
                        Counter = 0;
                        break;
                    }
                        if (col.gameObject.tag == "skeleton")
                    {
                        var doer = col.gameObject.GetComponent<SkeletonController>();
                        doer.SkeletonHit(this.gameObject.GetComponent<HeroController>().weapon);
                        doer.SkeletonKnock(this.gameObject);
                        this.Active = false;
                    }
                    if (col.gameObject.tag == "hound")
                    {
                        var doer = col.gameObject.GetComponent<HoundController>();
                        doer.HoundHit(this.gameObject.GetComponent<HeroController>().weapon);
                        doer.HoundKnock(this.gameObject);
                        this.Active = false;
                    }
                    if (col.gameObject.tag == "skull")
                    {
                        var eoer = col.gameObject.GetComponent<FireSkullController>();
                        eoer.SkullHit(this.gameObject.GetComponent<HeroController>().weapon);
                        this.Active = false;
                    }
                    if (col.gameObject.tag == "boss")
                    {
                        var doer = col.gameObject.GetComponent<BossController>();
                        doer.BossHit(this.gameObject.GetComponent<HeroController>().weapon);
                        this.Active = false;
                    }
                    //break;
                }
                if (this.ElapsedTime > DURATION || !this.Active)
                {
                    this.Active = false;
                    SwordCollider.enabled = false;
                    AirSwordCollider.enabled = false;
                }
                return;
            }

            if (Player.GetComponent<SpriteRenderer>().flipX)
            {
                var contacts = new Collider2D[6];
                if (Player.GetComponent<HeroController>().ground)
                {
                SwordColliderRev.enabled = true;
                    this.SwordColliderRev.GetContacts(contacts);
                }
                else
                {
                    AirSwordColliderRev.enabled = true;
                    this.AirSwordColliderRev.GetContacts(contacts);
                }
                foreach (var col in contacts)
                {
                    Counter += 1;
                    if (Counter > 5)
                    {
                        Counter = 0;
                        break;
                    }
                    if (col.gameObject.tag == "skeleton")
                    {
                        var doer = col.gameObject.GetComponent<SkeletonController>();
                        doer.SkeletonHit(this.gameObject.GetComponent<HeroController>().weapon);
                        doer.SkeletonKnock(this.gameObject);
                        this.Active = false;
                    }
                    if (col.gameObject.tag == "hound")
                    {
                        var doer = col.gameObject.GetComponent<HoundController>();
                        doer.HoundHit(this.gameObject.GetComponent<HeroController>().weapon);
                        doer.HoundKnock(this.gameObject);
                        this.Active = false;
                    }
                    if (col.gameObject.tag == "skull")
                    {
                        var doer = col.gameObject.GetComponent<FireSkullController>();
                        doer.SkullHit(this.gameObject.GetComponent<HeroController>().weapon);
                        this.Active = false;
                    }
                    //break;
                }

                if (this.ElapsedTime > DURATION || !this.Active)
                {
                    this.Active = false;
                    SwordColliderRev.enabled = false;
                    AirSwordColliderRev.enabled = false;
                }
                return;
            }
        }

    }

    public void Execute(GameObject gameObject)
    {
        if (!this.Active)
        {
            this.Active = true;
            this.Player = gameObject;
            this.SwordCollider = this.Player.transform.Find("SwordHitBox").GetComponent<PolygonCollider2D>();
            this.SwordColliderRev = this.Player.transform.Find("SwordHitBoxRev").GetComponent<PolygonCollider2D>();
            this.AirSwordCollider = this.Player.transform.Find("AirSwordHitBox").GetComponent<PolygonCollider2D>();
            this.AirSwordColliderRev = this.Player.transform.Find("AirSwordHitBoxRev").GetComponent<PolygonCollider2D>();
            this.SwordCollider.enabled = false;
            this.SwordColliderRev.enabled = false;
        }
    }
}
