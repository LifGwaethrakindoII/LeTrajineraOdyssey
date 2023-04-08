using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif 

namespace LittleGuyGames
{
public enum OrganizeMode
{
    Line,
    Radial,
    CuadraticBeizer
}

[ExecuteInEditMode]
public class GameObjectOrganizer : MonoBehaviour
{
    [SerializeField] private OrganizeMode mode;                                         /// <summary>Organization's Mode.</summary>
    [SerializeField] private GameObject prefab;                                         /// <summary>Prefab's reference.</summary>
    [SerializeField] private int reproductions;                                         /// <summary>Reproductions.</summary>
    [SerializeField] private bool drawHandles;                                          /// <summary>Draw Handles?.</summary>
    [SerializeField] private Space space;                                               /// <summary>Space-relativenes for the points.</summary>
    [Space(5f)]
    [SerializeField] private Vector3 _a;                                                /// <summary>Point A.</summary>
    [SerializeField] private Vector3 _b;                                                /// <summary>Point B.</summary>
    [SerializeField] private Vector3 _c;                                                /// <summary>Point C.</summary>
    [Space(5f)]
    [Header("Radial's Attributes:")]
    [SerializeField] private float radius;                                              /// <summary>Radial Distribution's Radius.</summary>
    [SerializeField][Range(0.0f, 1.0f)] private float radialFill;                       /// <summary>Radial Fill's Percentage.</summary>
    [HideInInspector][SerializeField] private List<GameObject> prefabReproductions;     /// <summary>Prefab's reproductions.</summary>

    /// <summary>Gets a property.</summary>
    public Vector3 a
    {
        get { return space == Space.World ? _a : transform.TransformPoint(_a); }
        set { _a = value; }
    }

    /// <summary>Gets b property.</summary>
    public Vector3 b
    {
        get { return space == Space.World ? _b : transform.TransformPoint(_b); }
        set { _b = value; }
    }

    /// <summary>Gets c property.</summary>
    public Vector3 c
    {
        get { return space == Space.World ? _c : transform.TransformPoint(_c); }
        set { _c = value; }
    }

    /// <summary>Callback invoked when GameObjectOrganizer's instance is enabled.</summary>
    private void OnEnable()
    {
#if UNITY_EDITOR
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        SceneView.onSceneGUIDelegate += OnSceneGUI;
#endif
    }

    /// <summary>Callback invoked when GameObjectOrganizer's instance is disabled.</summary>
    private void OnDisable()
    {
#if UNITY_EDITOR
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
#endif
    }

    /// <summary>Draws Gizmos on Editor mode when GameObjectOrganizer's instance is selected.</summary>
    private void OnDrawGizmosSelected()
    {
        switch(mode)
        {
            case OrganizeMode.Line:
                Gizmos.DrawLine(a, b);       
            break;

            case OrganizeMode.CuadraticBeizer:
                LGGGizmos.DrawCuadraticBeizerCurve(a, b, c);
            break;
        }

        float r = (float)reproductions;
        float s = 1.3f;

        for(float i = 0.0f; i < r; i++)
        {
            float t = mode == OrganizeMode.Radial ? i / r : i / (r - 1.0f);
            float t2 = 0.0f;

            switch(mode)
            {
                case OrganizeMode.Line:
                    Gizmos.DrawSphere(Vector3.Lerp(a, b, t), s);
                break;

                case OrganizeMode.CuadraticBeizer:
                    Gizmos.DrawSphere(LGGVector3.CuadraticBeizer(a, b, c, t), s);
                break;

                case OrganizeMode.Radial:
                    t *= radialFill;
                    t2 = ((i + 1.0f) / r) * radialFill;

                    Vector3 p = RadialPoint(t);
                    Gizmos.DrawSphere(p, s);
                    Gizmos.DrawLine(p, RadialPoint(t2));
                break;
            }
        }
    }

    /// <summary>Calculates a radial point along the local up axis.</summary>
    /// <param name="t">Normalized time t.</param>
    private Vector3 RadialPoint(float t)
    {
        return a + (Quaternion.AngleAxis(360.0f * t, transform.up) * transform.right * radius);
    }

    [Button("Reproduce")]
    /// <summary>Reproduces Prefab.</summary>
    private void Reproduce()
    {
        if(prefab == null || reproductions <= 0) return;

        if(prefabReproductions == null) prefabReproductions = new List<GameObject>();
        else ClearReproductions();

        float r = (float)reproductions;

        for(float i = 0.0f; i < r; i++)
        {
            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;
            float t = mode == OrganizeMode.Radial ? i / r : i / (r - 1.0f);
            float t2 = mode == OrganizeMode.Radial ? (i + 1.0f) / r : (i + 1.0f) / (r - 1.0f);

            switch(mode)
            {
                case OrganizeMode.Line:
                    position = Vector3.Lerp(a, b, t);
                    rotation = Quaternion.LookRotation(b - a);
                break;

                case OrganizeMode.CuadraticBeizer:
                    position = LGGVector3.CuadraticBeizer(a, b, c, t);
                    rotation = Quaternion.LookRotation(LGGVector3.CuadraticBeizer(a, b, c, t2) - position);
                break;

                case OrganizeMode.Radial:
                    t *= radialFill;
                    t2 *= radialFill;

                    position = RadialPoint(t);
                    rotation = Quaternion.LookRotation(RadialPoint(t2) - position);
                break;
            }

            GameObject newGameObject = null;

#if UNITY_EDITOR
            newGameObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            newGameObject.transform.position = position;
            newGameObject.transform.rotation = rotation;
#else
            newGameObject = Instantiate(prefab, position, rotation);
#endif

            newGameObject.transform.SetParent(transform);
            prefabReproductions.Add(newGameObject);    
        }
    }

    [Button("Clear Reproductions")]
    /// <summary>Clears Reproductions.</summary>
    private void ClearReproductions()
    {
        if(prefabReproductions == null) return;

        foreach(GameObject obj in prefabReproductions)
        {
            if(obj != null) obj.Destroy();
        }

        prefabReproductions.Clear();
    }


#if UNITY_EDITOR
    /// <summary>Enables the Editor to handle an event in the Scene view.</summary>
    /// <param name="_view">Scene's View.</param>
    private void OnSceneGUI(SceneView _view)
    {
        if(!drawHandles) return;

        Quaternion rotation = space == Space.World ? Quaternion.identity : transform.rotation;
        Vector3 tempA = a;
        Vector3 tempB = b;
        Vector3 tempC = c;

        switch(mode)
        {
            case OrganizeMode.Line:
                tempA = Handles.PositionHandle(tempA, rotation);
                tempB = Handles.PositionHandle(tempB, rotation);
            break;

            case OrganizeMode.CuadraticBeizer:
                tempA = Handles.PositionHandle(tempA, rotation);
                tempB = Handles.PositionHandle(tempB, rotation);
                tempC = Handles.PositionHandle(tempC, rotation);
            break;
        }

        if(space == Space.Self)
        {
            _a = transform.InverseTransformPoint(tempA);
            _b = transform.InverseTransformPoint(tempB);
            _c = transform.InverseTransformPoint(tempC);
        }

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(this, "Position Handle Change");
        }
    }
#endif
}
}