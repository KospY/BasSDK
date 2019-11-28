using System.Collections;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class EffectParticleAttractorLinear : MonoBehaviour {
	ParticleSystem ps;
	ParticleSystem.Particle[] m_Particles;
	public Transform target;
	public float speed = 5f;
    public float maxDistance = 0.5f;
    int numParticlesAlive;
	void Start () {
		ps = GetComponent<ParticleSystem>();
		if (!GetComponent<Transform>()){
			GetComponent<Transform>();
		}
	}
	void Update () {

        float sqrDistance = Vector3.SqrMagnitude(target.position - transform.position);
        if (sqrDistance > maxDistance) { return; }

		m_Particles = new ParticleSystem.Particle[ps.main.maxParticles];
		numParticlesAlive = ps.GetParticles(m_Particles);
        float step = speed * Time.deltaTime;


        for (int i = 0; i < numParticlesAlive; i++) {
			m_Particles[i].position = Vector3.LerpUnclamped(m_Particles[i].position, target.position, step);
		}
		ps.SetParticles(m_Particles, numParticlesAlive);
	}
}
