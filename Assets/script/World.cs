using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class World
{
    static World _instance;
    public static World instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new World();
            }
            return _instance;
        }

    }

    List<Hittable> m_allMesh = new List<Hittable>();

    public void Clear()
    {
        m_allMesh.Clear();
    }

    public void Add(Hittable mesh)
    {
        m_allMesh.Add(mesh);
    }



    public bool Hit(Ray ray, float t_min, float t_max, ref Hit_record rec)
    {
        Hit_record temp_rec = new Hit_record();
        bool hit_anything = false;
        float closest_so_far = t_max;

        foreach(var mesh in m_allMesh)
        {
            if (mesh.Hit(ray, t_min, closest_so_far, ref temp_rec))
            {
                hit_anything = true;
                closest_so_far = temp_rec.m_t;

                
                rec.CopyFrom(temp_rec);
                rec.m_mat = mesh.GetMat();
            }
        }

        return hit_anything;
    }
}