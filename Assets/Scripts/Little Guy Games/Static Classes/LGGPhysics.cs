using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*============================================================
**
** Class:  LGGPhysics
**
** Purpose: Static class for Physics-specific functions.
**
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/

namespace LittleGuyGames
{
public static class LGGPhysics
{
    public static readonly WaitForFixedUpdate WAIT_PHYSICS_THREAD;  /// <summary>WaitForFixedUpdate static reference.</summary>

    /// <summary>Static Constructor.</summary>
    static LGGPhysics()
    {
        WAIT_PHYSICS_THREAD = new WaitForFixedUpdate();
    }

    /// <summary>Sets Physics' Delta Time according to provided Frame-Rate.</summary>
    /// <param name="frameRate">Frame-Rate.</param>
    public static void SetPhysicsFrameRate(int frameRate)
    {
        float dt = 1.0f / (float)frameRate;
        Time.fixedDeltaTime = dt;
    }

    /// <summary>Calculates a projectile's projection given a time t [pf = (g * (t^2/2)) + (v0 * t) + p0].</summary>
    /// <param name="t">Time t.</param>
    /// <param name="v0">Initial Velocity.</param>
    /// <param name="p0">Initial Position.</param>
    /// <param name="g">Gravity.</param>
    /// <returns>Projectile's Projection given time t.</returns>
    public static Vector3 ProjectileProjection(float t, Vector3 v0, Vector3 p0, Vector3 g)
    {
        return g * (0.5f * t * t) + (v0 * t) + p0;
    }

    public static float ProjectileTime(Vector3 v0, Vector3 p0, Vector3 pf, Vector3 g)
    {
        return ProjectileTime(v0.y, p0.y, pf.y, g.y); 
    }

    public static float ProjectileTime(float v0, float p0, float pf, float g)
    {
        return Mathf.Sqrt(2.0f * (pf - p0 - v0 / g));
    }

    public static float CalculateTime(Vector3 p0, Vector3 pf, Vector3 g)
    {
        // Solve for t using the quadratic formula
        float a = g.sqrMagnitude / 2.0f;
        float b = Vector3.Dot(pf - p0, g);
        float c = Vector3.Distance(pf, p0);

        float discriminant = b * b - 4.0f * a * c;
        
        if (discriminant < 0)
        {
            // No real solutions (object never reaches pf)
            return float.NaN;
        } else if (discriminant == 0.0f)
        {
            // One real solution (object reaches pf at one time)
            float t = -b / (2.0f * a);
            return t;
        } else
        {
            // Two real solutions (object reaches pf at two times)
            float t1 = (-b + Mathf.Sqrt(discriminant)) / (2.0f * a);
            float t2 = (-b - Mathf.Sqrt(discriminant)) / (2.0f * a);
            // Return the smaller positive solution
            return Mathf.Min(t1, t2);
        }
    }

#region SphereCast:
    /// <summary>Cast a Sphere along a direction and stores the result to a given hit information.</summary>
    /// <param name="ray">Ray.</param>
    /// <param name="r">Sphere's Radius.</param>
    /// <param name="hit">Hit Information.</param>
    /// <param name="l">Ray's Length.</param>
    /// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
    /// <param name="interactions">Hit Interactions to Allow.</param>
    /// <returns>True if the Sphere cast detected a Collider.</returns>
    public static bool SphereCast(Ray ray, float r, out RaycastHit hit, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, QueryTriggerInteraction interactions = QueryTriggerInteraction.UseGlobal)
    {
        ray.origin = ray.origin - (ray.direction * r);

        return Physics.SphereCast(ray, r, out hit, l, mask, interactions);
    }

    /// <summary>Cast a Sphere along a direction and stores the result to a given hit information.</summary>
    /// <param name="o">Ray's Origin.</param>
    /// <param name="r">Sphere's Radius.</param>
    /// <param name="d">Cast's Direction.</param>
    /// <param name="hit">Hit Information.</param>
    /// <param name="l">Ray's Length.</param>
    /// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
    /// <param name="interactions">Hit Interactions to Allow.</param>
    /// <returns>True if the Sphere cast detected a Collider.</returns>
    public static bool SphereCast(Vector3 o, float r, Vector3 d, out RaycastHit hit, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, QueryTriggerInteraction interactions = QueryTriggerInteraction.UseGlobal)
    {
        Vector3 origin = o - (d * r);
        Ray ray = new Ray(origin, d);

        return Physics.SphereCast(ray, r, out hit, l, mask, interactions);
    }

    /// <summary>Cast a Sphere along a direction.</summary>
    /// <param name="o">Ray's Origin.</param>
    /// <param name="r">Sphere's Radius.</param>
    /// <param name="d">Cast's Direction.</param>
    /// <param name="l">Ray's Length.</param>
    /// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
    /// <param name="interactions">Hit Interactions to Allow.</param>
    /// <returns>True if the Sphere cast detected a Collider.</returns>
    public static bool SphereCast(Ray ray, float r, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, QueryTriggerInteraction interactions = QueryTriggerInteraction.UseGlobal)
    {
        ray.origin = ray.origin - (ray.direction * r);

        return Physics.SphereCast(ray, r, l, mask, interactions);
    }

    /// <summary>Cast a Sphere along a direction and stores the result to a given hit information.</summary>
    /// <param name="o">Ray's Origin.</param>
    /// <param name="r">Sphere's Radius.</param>
    /// <param name="d">Cast's Direction.</param>
    /// <param name="hit">Hit Information.</param>
    /// <param name="l">Ray's Length.</param>
    /// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
    /// <param name="interactions">Hit Interactions to Allow.</param>
    /// <param name="additionalDirections">Additional Directions to cast the ray along.</param>
    /// <returns>True if the Sphere cast detected a Collider.</returns>
    public static bool SphereCast(Vector3 o, float r, Vector3 d, out RaycastHit hit, Quaternion q, float maxD = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, QueryTriggerInteraction i = QueryTriggerInteraction.UseGlobal, params Vector3[] additionalDirections)
    {
        d.Normalize();

        float diameter = r * 2.0f;
        Vector3 offsetOrigin = o - (q * d * diameter);
        if(maxD == Mathf.Infinity) maxD = diameter;

        if(Physics.SphereCast(offsetOrigin, r, d, out hit, maxD, mask, i)) return true;

        if(additionalDirections != null && additionalDirections.Length > 0)
        {
            foreach(Vector3 direction in additionalDirections)
            {
                direction.Normalize();
                offsetOrigin = o - (q * direction * diameter);
                if(Physics.SphereCast(offsetOrigin, r, d, out hit, maxD, mask, i)) return true;
            }
        }
        return false;
    }

    /// <summary>Cast a Sphere along a direction to get all the RaycastHits intersected.</summary>
    /// <param name="o">Ray's Origin.</param>
    /// <param name="r">Sphere's Radius.</param>
    /// <param name="d">Cast's Direction.</param>
    /// <param name="l">Ray's Length.</param>
    /// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
    /// <param name="interactions">Hit Interactions to Allow.</param>
    /// <returns>RaycastHits intersected on the cast.</returns>
    public static RaycastHit[] SphereCastAll(Ray ray, float r, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, QueryTriggerInteraction interactions = QueryTriggerInteraction.UseGlobal)
    {
        ray.origin = ray.origin - (ray.direction * r);

        return Physics.SphereCastAll(ray, r, l, mask, interactions);
    }
#endregion

#region BoxCast:
    /// <summary>Cast a Box along a direction and stores the result to a given hit information.</summary>
    /// <param name="o">Ray's Origin.</param>
    /// <param name="s">Box's Size.</param>
    /// <param name="d">Cast's Direction.</param>
    /// <param name="hit">Hit Information.</param>
    /// <param name="r">Box's Rotation.</param>
    /// <param name="l">Ray's Length.</param>
    /// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
    /// <param name="interactions">Hit Interactions to Allow.</param>
    /// <returns>True if the Box cast detected a Collider.</returns>
    public static bool BoxCast(Vector3 o, Vector3 s, Vector3 d, out RaycastHit hit, Quaternion r, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, QueryTriggerInteraction interactions = QueryTriggerInteraction.UseGlobal)
    {
        Vector3 origin = o - (d * s.z * 0.5f);
        Ray ray = new Ray(origin, d);

        return Physics.BoxCast(ray.origin, s, ray.direction, out hit, r, l, mask, interactions);
    }

    /// <summary>Cast a Box along a direction.</summary>
    /// <param name="o">Ray's Origin.</param>
    /// <param name="s">Box's Size.</param>
    /// <param name="d">Cast's Direction.</param>
    /// <param name="r">Box's Rotation.</param>
    /// <param name="l">Ray's Length.</param>
    /// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
    /// <param name="interactions">Hit Interactions to Allow.</param>
    /// <returns>True if the Box cast detected a Collider.</returns>
    public static bool BoxCast(Vector3 o, Vector3 s, Vector3 d, Quaternion r, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, QueryTriggerInteraction interactions = QueryTriggerInteraction.UseGlobal)
    {
        Vector3 origin = o - (d * s.z * 0.5f);
        Ray ray = new Ray(origin, d);

        return Physics.BoxCast(ray.origin, s, ray.direction, r, l, mask, interactions);
    }

    /// <summary>Cast a Box along a direction to get all the RaycastHits intersected.</summary>
    /// <param name="o">Ray's Origin.</param>
    /// <param name="s">Box's Size.</param>
    /// <param name="d">Cast's Direction.</param>
    /// <param name="r">Box's Rotation.</param>
    /// <param name="l">Ray's Length.</param>
    /// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
    /// <param name="interactions">Hit Interactions to Allow.</param>
    /// <returns>RaycastHits intersected on the cast.</returns>
    public static RaycastHit[] BoxCastAll(Vector3 o, Vector3 s, Vector3 d, Quaternion r, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, QueryTriggerInteraction interactions = QueryTriggerInteraction.UseGlobal)
    {
        Vector3 origin = o - (d * s.z * 0.5f);
        Ray ray = new Ray(origin, d);

        return Physics.BoxCastAll(ray.origin, s, ray.direction, r, l, mask, interactions);
    }
#endregion

#region CapsuleCast:
    /// <summary>Cast a Capsule along a direction and stores the result to a given hit information.</summary>
    /// <param name="a">Point A [Ray's Origin].</param>
    /// <param name="b">Point B [Ray's end point].</param>
    /// <param name="r">Capsule's Radius.</param>
    /// <param name="d">Cast's Direction.</param>
    /// <param name="hit">Hit Information.</param>
    /// <param name="l">Ray's Length.</param>
    /// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
    /// <param name="interactions">Hit Interactions to Allow.</param>
    /// <returns>True if the Capsule cast detected a Collider.</returns>
    public static bool CapsuleCast(Vector3 a, Vector3 b, float r, Vector3 d, out RaycastHit hit, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, QueryTriggerInteraction interactions = QueryTriggerInteraction.UseGlobal)
    {
        Vector3 origin = a - (d * r);
        Ray ray = new Ray(origin, d);

        return Physics.CapsuleCast(a, b, r, ray.direction, out hit, l, mask, interactions);
    }

    /// <summary>Cast a Capsule along a direction.</summary>
    /// <param name="a">Point A [Ray's Origin].</param>
    /// <param name="b">Point B [Ray's end point].</param>
    /// <param name="r">Capsule's Radius.</param>
    /// <param name="d">Cast's Direction.</param>
    /// <param name="l">Ray's Length.</param>
    /// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
    /// <param name="interactions">Hit Interactions to Allow.</param>
    /// <returns>True if the Capsule cast detected a Collider.</returns>
    public static bool CapsuleCast(Vector3 a, Vector3 b, float r, Vector3 d, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, QueryTriggerInteraction interactions = QueryTriggerInteraction.UseGlobal)
    {
        Vector3 origin = a - (d * r);
        Ray ray = new Ray(origin, d);

        return Physics.CapsuleCast(a, b, r, ray.direction, l, mask, interactions);
    }

    /// <summary>Cast a Capsule along a direction to get all the RaycastHits intersected.</summary>
    /// <param name="a">Point A [Ray's Origin].</param>
    /// <param name="b">Point B [Ray's end point].</param>
    /// <param name="r">Capsule's Radius.</param>
    /// <param name="d">Cast's Direction.</param>
    /// <param name="l">Ray's Length.</param>
    /// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
    /// <param name="interactions">Hit Interactions to Allow.</param>
    /// <returns>RaycastHits intersected on the cast.</returns>
    public static RaycastHit[] CapsuleCastAll(Vector3 a, Vector3 b, float r, Vector3 d, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, QueryTriggerInteraction interactions = QueryTriggerInteraction.UseGlobal)
    {
        Vector3 origin = a - (d * r);
        Ray ray = new Ray(origin, d);

        return Physics.CapsuleCastAll(a, b, r, ray.direction, l, mask, interactions);
    }
#endregion   
}
}