using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HittableType
{
    sphere,
}

public class Hit_record
{
    public Vector3 m_point;
    public Vector3 m_normal;
    public float m_t;
    public bool m_isFrontFace;
    public IMaterial m_mat;

    public void CopyFrom(Hit_record a)
    {
        m_point = a.m_point;
        m_normal = a.m_normal;
        m_t = a.m_t;
        m_isFrontFace = a.m_isFrontFace;
    }

    public void set_face_normal(Ray r, Vector3 outward_normal)
    {
        m_isFrontFace = Vector3.Dot(r.direction, outward_normal) < 0;
        m_normal = m_isFrontFace ? outward_normal : -outward_normal;
    }
}

public class Hittable
{
    public HittableType m_type;
    IMaterial m_mat;

    public virtual bool Hit(Ray r, float t_min, float t_max, ref Hit_record rec)
    {
        return false;
    }

    public void SetMat(IMaterial mat)
    {
        m_mat = mat;
    }
    public IMaterial GetMat()
    {
        return m_mat;
    }
}


public class Sphere : Hittable
{
    public Vector3 m_center;
    public float m_radius;
    public Sphere(Vector3 c, float r)
    {
        m_type = HittableType.sphere;
        m_center = c;
        m_radius = r;
    }



    public override bool Hit(Ray ray, float t_min, float t_max, ref Hit_record rec)
    {
        var oc = ray.origin - m_center;
        var a = Vector3.Dot(ray.direction, ray.direction);
        var half_b = Vector3.Dot(oc, ray.direction);
        var c = Vector3.Dot(oc, oc) - m_radius * m_radius;
        var discriminant = half_b*half_b - a * c;
        if (discriminant < 0)
            return false;

        var sqrtd = Mathf.Sqrt(discriminant);
        var root = (-half_b - sqrtd) / a;
        if (root<t_min || t_max<root)
        {
            root = (-half_b + sqrtd) / a;
            if (root < t_min || t_max < root)
                return false;
        }

        rec.m_t = root;
        rec.m_point = ray.GetPoint(rec.m_t);
        var outward_normal = (rec.m_point - m_center) / m_radius;
        rec.set_face_normal(ray, outward_normal);

        return true;
    }
}