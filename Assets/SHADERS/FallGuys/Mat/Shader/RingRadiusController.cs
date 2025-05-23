﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingRadiusController : MonoBehaviour
{
    Material m_material;
    private float m_time;
    [SerializeField] private float m_speed;
    private float m_radius = 0;
    private float m_randomValue;

    // Start is called before the first frame update
    void Start()
    {
        m_material = this.gameObject.GetComponent<MeshRenderer>().material;
        m_material.SetFloat("_radius_A", m_radius);

        float _random = Random.Range(0.01f, 0.04f);
        m_material.SetFloat("_radius_B", _random);
    }

    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime * m_speed;
        if (m_time < 1)
        {
            m_radius = Mathf.Lerp(0, 1, m_time);
            m_material.SetFloat("_radius_A", m_radius);
        }

        if(m_radius >= 0.65f)
        {
            Destroy(this.gameObject);
        }
    }
}
