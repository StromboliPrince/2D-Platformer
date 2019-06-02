﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    [SerializeField] private float Speed = 5.0f;

    [SerializeField] private float AttackDuration = 0.25f;
    [SerializeField] private AnimationCurve AttackCurve;
    private float AttackTimer = 0.0f;

    [SerializeField] private float DecayDuration = 0.25f;
    [SerializeField] private AnimationCurve DecayCurve;
    private float DecayTimer = 0.0f;

    [SerializeField] private AnimationCurve SustainCurve;

    [SerializeField] private float ReleaseDuration = 0.25f;
    [SerializeField] private AnimationCurve ReleaseCurve;
    private float ReleaseTimer = 0.0f;

    private float InputDirection = 0.0f;

    private enum Phase { Attack, Decay, Sustain, Release, None };

    private Phase CurrentPhase = Phase.None;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
        {
            this.ResetTimers();
            this.CurrentPhase = Phase.Attack;
            this.InputDirection = 1.0f;
        }
        else if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
        {
            this.ResetTimers();
            this.CurrentPhase = Phase.Attack;
            this.InputDirection = -1.0f;
        }

        if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
        {
            this.InputDirection = 1.0f;
        }
        else if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
        {
            this.InputDirection = -1.0f;
        }

        if (this.CurrentPhase == Phase.Sustain && !Input.GetButton("Horizontal"))
        {
            this.CurrentPhase = Phase.Release;
        }

        if (this.CurrentPhase != Phase.None)
        {
            var position = this.gameObject.transform.position;
            position.x += this.InputDirection * this.Speed * this.ADSREnvelope() * Time.deltaTime;
            this.gameObject.transform.position = position;
        }
    }

    float ADSREnvelope()
    {
        float value = 0.0f;

        if (Phase.Attack == this.CurrentPhase)
        {
            value = this.AttackCurve.Evaluate(this.AttackTimer / this.AttackDuration);
            this.AttackTimer += Time.deltaTime;
            if (this.AttackTimer > this.AttackDuration)
            {
                this.CurrentPhase = Phase.Decay;
            }
        }
        else if (Phase.Decay == this.CurrentPhase)
        {
            value = this.DecayCurve.Evaluate(this.DecayTimer / this.DecayDuration);
            this.DecayTimer += Time.deltaTime;
            if (this.DecayTimer > this.DecayDuration)
            {
                this.CurrentPhase = Phase.Sustain;
            }
        }
        else if (Phase.Sustain == this.CurrentPhase)
        {
            value = 1.0f;
        }
        else if (Phase.Release == this.CurrentPhase)
        {
            value = this.ReleaseCurve.Evaluate(this.ReleaseTimer / this.ReleaseDuration);
            this.ReleaseTimer += Time.deltaTime;
            if (this.ReleaseTimer > this.ReleaseDuration)
            {
                this.CurrentPhase = Phase.None;
            }
        }

        return value;
    }

    private void ResetTimers()
    {
        this.AttackTimer = 0.0f;
        this.DecayTimer = 0.0f;
        this.ReleaseTimer = 0.0f;
    }
}