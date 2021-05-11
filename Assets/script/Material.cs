using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class IMaterial
{
    public abstract bool Scatter(Ray r_in, Hit_record rec, ref Color attenuation, ref Ray scattered);
}

public enum MaterialType
{
    lambertion,
    metal,
    dielectric,
}


public class Lambertion : IMaterial
{
    public Color m_albedo;

    public Lambertion(Color c)
    {
        m_albedo = c;
    }

    public override bool Scatter(Ray r_in, Hit_record rec, ref Color colAttenuation, ref Ray scattered)
    {
        var scatter_direction = rec.m_normal + Helper.random_unit_vector();
        if (Helper.isVec3_nearZero(scatter_direction))
            scatter_direction = rec.m_normal;

        scattered.origin = rec.m_point;
        scattered.direction = scatter_direction.normalized;

        colAttenuation = m_albedo;

        return true;
    }
}

public class Metal : IMaterial
{
    public Color m_albedo;
    public float m_fuzz;

    public Metal(Color c, float fuzz)
    {
        m_albedo = c;
        m_fuzz = fuzz < 1 ? fuzz : 1;
    }


    public override bool Scatter(Ray r_in, Hit_record rec, ref Color colAttenuation, ref Ray scattered)
    {
        var reflected = Vector3.Reflect(r_in.direction.normalized, rec.m_normal);
        scattered.origin = rec.m_point;
        scattered.direction = reflected.normalized + m_fuzz*Helper.random_in_unit_sphere();

        colAttenuation = m_albedo;

        return Vector3.Dot(scattered.direction, rec.m_normal) > 0;
    }
}


public class Dielectric : IMaterial
{
    public float m_ir; // index of refraction

    public Dielectric( float ir)
    {
        m_ir = ir;
    }


    public override bool Scatter(Ray r_in, Hit_record rec, ref Color colAttenuation, ref Ray scattered)
    {
        colAttenuation = Color.white;
        //
        float refraction_ratio = rec.m_isFrontFace ? (1.0f / m_ir) : m_ir;

        Vector3 unit_direction = r_in.direction.normalized;

        float cos_theta = Mathf.Min(Vector3.Dot(-unit_direction, rec.m_normal), 1.0f);
        float sin_theta = Mathf.Sqrt(1.0f - cos_theta * cos_theta);

        bool cannot_refract = refraction_ratio * sin_theta > 1.0;
        Vector3 direction;

        if (cannot_refract)
            direction = Vector3.Reflect(unit_direction, rec.m_normal);
        else
            direction =  Helper.refract(unit_direction, rec.m_normal, refraction_ratio);

        scattered = new Ray(rec.m_point, direction);


        return true;
    }
}