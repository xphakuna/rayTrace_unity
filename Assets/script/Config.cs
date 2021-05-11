using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SphereConfig
{
    public string m_name;
    public Vector3 m_center;
    [Range(0.1f,1000)]
    public float m_radius;
    public MaterialType m_matType;
    public Color m_matColor;
    [Tooltip("for metal")]
    public float m_matFuzz; // for metal
    [Tooltip("for refraction")]
    public float m_matIndexOfRefraction; // for refraction
}

[CreateAssetMenu(menuName = "CreateConfig")]
public class Config : ScriptableObject
{
    public string m_lable1 = "camera begin------------";
    public Vector3 m_lookfrom = new Vector3(0, 0, 0);
    public Vector3 m_lookat = new Vector3(0, 0, -1);
    public Vector3 m_vup = new Vector3(0, 1, 0);
    [Tooltip("vertical field-of-view in degrees")]
    public float m_deg_vfov = 20;
    public float m_dist_to_focus = 10;
    public float m_aperture = 0.1f;
    public float m_aspect_ratio = (float)(APP.s_width)/APP.s_height;
    public string m_lable2 = "camera end------------";
    public SphereConfig[] m_allSphere;

    public static Config s_load(string name)
    {
        Config cfg = Resources.Load<Config>("config/"+name);
        if (cfg == null)
        {
            Debug.LogError("not find " + name);
        }
        return cfg;
    }
}
 