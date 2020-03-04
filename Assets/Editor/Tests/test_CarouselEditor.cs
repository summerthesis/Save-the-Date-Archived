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
    float size = 1;

    protected virtual void OnSceneGUI() {
        if (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout) {
            Transform transform = ((expendable_Carousel)target).transform;
            int numberOfPlatforms = ((expendable_Carousel)target).NumberOfPlatforms;
            float angle = 2 * PI / numberOfPlatforms;
            float radius = ((expendable_Carousel)target).CarouselRadius;

            Handles.color = Color.grey;
            Handles.CubeHandleCap(0, transform.position, transform.rotation, size, EventType.Repaint);
            Handles.color = Color.white;
            for (int i = 0; i < numberOfPlatforms; ++i) {
                float angleDiv = angle*i;
                Vector3 offset = transform.TransformDirection(
                    new Vector3(Mathf.Cos(angleDiv), 0,
                                Mathf.Sin(angleDiv)));
                Vector3 platPosition = transform.position + offset*radius;
                Handles.SphereHandleCap(0, platPosition, transform.rotation, size, EventType.Repaint);
                Handles.DrawDottedLine(transform.position, platPosition, 10);
            }
            Handles.color = new Color(0.75f, 0.5f, 0.0f);
            Handles.Disc(0, Quaternion.identity, transform.position, transform.up, radius, false, 0.01f);
        }
    }
}
