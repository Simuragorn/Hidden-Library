using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticleVisibilityManager : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private float initialAlpha;
    void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Stop();
        initialAlpha = particleSystem.main.startColor.color.a;
        Color32 noAlphaColor = particleSystem.main.startColor.color;
        noAlphaColor.a = 0;
        var particleSystemSettings = particleSystem.main;
        particleSystemSettings.startColor = new MinMaxGradient(noAlphaColor);
        particleSystem.Play();
    }
    private void OnParticleTrigger()
    {
        List<Particle> enteredParticles = new();
        List<Particle> exitedParticles = new();

        int enteredParticlesCount = particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enteredParticles);
        int exitedParticlesCount = particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exitedParticles);

        for (int i = 0; i < enteredParticlesCount; i++)
        {
            Particle particle = enteredParticles[i];
            Color newColor = particle.startColor;
            newColor.a = initialAlpha;
            particle.startColor = newColor;
            enteredParticles[i] = particle;
        }

        for (int i = 0; i < exitedParticlesCount; i++)
        {
            Particle particle = exitedParticles[i];
            Color newColor = particle.startColor;
            newColor.a = 0;
            particle.startColor = newColor;
            exitedParticles[i] = particle;
        }

        particleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enteredParticles);
        particleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Exit, exitedParticles);
    }
}
