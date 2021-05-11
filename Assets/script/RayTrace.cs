using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public partial class RayTrace 
{
    static RayTrace _instance;
    public static RayTrace instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new RayTrace();
            }
            return _instance;
        }

    }

    public void prepare(string name)
    {
        Config cfg = Config.s_load(name);

        MyCamera.instance.Init(cfg.m_lookfrom, cfg.m_lookat, cfg.m_vup,
            cfg.m_deg_vfov, cfg.m_aspect_ratio, cfg.m_aperture, cfg.m_dist_to_focus);

        World.instance.Clear();
        foreach (var s in cfg.m_allSphere)
        {
            Hittable hit = new Sphere(s.m_center, s.m_radius);
            switch (s.m_matType)
            {
                case MaterialType.lambertion:
                    {
                        Lambertion mat = new Lambertion(s.m_matColor);
                        hit.SetMat(mat);
                    }

                    break;
                case MaterialType.metal:
                    {
                        Metal mat = new Metal(s.m_matColor, s.m_matFuzz);
                        hit.SetMat(mat);
                    }
                    break;
                case MaterialType.dielectric:
                    {
                        Dielectric mat = new Dielectric(s.m_matIndexOfRefraction);
                        hit.SetMat(mat);
                    }
                    break;
            }
            //
            World.instance.Add(hit);
        }
    }
}
 