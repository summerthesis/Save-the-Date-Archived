using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

using static UnityEngine.Mathf; //THIS IS A DIRECTIVE, NOT A STATEMENT

[CustomEditor(typeof(expendable_Carousel))]
[CanEditMultipleObjects]

public class test_CarouselEditor : Editor
{
    float handleSize = 1;
    float angle;
    int numberOfPlatforms;
    Transform transform;
    float carouselRadius;
    float angleDiv;
    Vector3 platPosition;
    protected virtual void OnSceneGUI() {
        if (Event.current.type == EventType.Repaint) {
            transform = ((expendable_Carousel)target).transform;
            numberOfPlatforms = ((expendable_Carousel)target).NumberOfPlatforms;
            angle = numberOfPlatforms > 0 ? 2 * PI / numberOfPlatforms : 0 ;
            carouselRadius = ((expendable_Carousel)target).CarouselRadius;

            Handles.color = Color.grey;
            Handles.CubeHandleCap(0, transform.position, transform.rotation, handleSize, EventType.Repaint);
            Handles.color = Color.white;
            for (int i = 0; i < numberOfPlatforms; ++i) {
                angleDiv = angle*i;
                platPosition = CalculateInitialPosition(angleDiv);
                Handles.DrawDottedLine(transform.position, platPosition, 10);
            }
            Handles.color = new Color(0.75f, 0.5f, 0.0f);
            Handles.Disc(0, Quaternion.identity, transform.position, transform.up, carouselRadius, false, 0.01f);
        }
    }

    #region Gizmo
    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected | GizmoType.Active)]
    static void DrawGizmosSelected(expendable_Carousel car, GizmoType gt)
    {
        float angle = car.NumberOfPlatforms > 0 ? 2*PI/car.NumberOfPlatforms : 0;
        float angleDiv;
        Vector3 targetPos;
        for (int i = 0; i < car.NumberOfPlatforms; ++i)
        {
            angleDiv = i * angle;
            targetPos = car.CalculateInitialPosition(angleDiv);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(targetPos, 1);
            //Handles.Label(targetPos, "Platform");
        }
    }
    #endregion

    private Vector3 CalculateInitialPosition(float angle) {
        return transform.position + transform.TransformDirection(
                    new Vector3(Mathf.Cos(angleDiv), 0, Mathf.Sin(angleDiv)))*
                    carouselRadius ;
    }
}