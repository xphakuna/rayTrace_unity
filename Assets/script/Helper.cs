using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Helper
{
    public static Vector3 random_in_unit_sphere()
    {
        while (true)
        {
            var p = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            if (p.sqrMagnitude >= 1) continue;
            return p;
        }
    }

    public static Vector3 random_in_unit_disk()
    {
        while (true)
        {
            var p = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
            if (p.sqrMagnitude >= 1) continue;
            return p;
        }
    }

    public static Vector3 random_unit_vector()
    {
        return random_in_unit_sphere().normalized;
    }

    public static Vector3 random_in_hemisphere(Vector3 normal)
    {
        Vector3 in_unit_sphere = random_in_unit_sphere();
        if (Vector3.Dot(in_unit_sphere, normal) > 0.0) // In the same hemisphere as the normal
            return in_unit_sphere;
        else
            return -in_unit_sphere;
    }


    public static bool isVec3_nearZero(Vector3 v)
    {
        float s = 1e-8f;
        return Mathf.Abs(v.x) < s && Mathf.Abs(v.y) < s && Mathf.Abs(v.z) < s;
    }


    public static Vector3 refract(Vector3 uv, Vector3 n, float etai_over_etat)
    {
        float cos_theta = Mathf.Min(Vector3.Dot(-uv, n), 1.0f);
        Vector3 r_out_perp = etai_over_etat * (uv + cos_theta * n);
        Vector3 r_out_parallel = -Mathf.Sqrt(Mathf.Abs(1.0f - r_out_perp.sqrMagnitude)) * n;
        return r_out_perp + r_out_parallel;
    }
}