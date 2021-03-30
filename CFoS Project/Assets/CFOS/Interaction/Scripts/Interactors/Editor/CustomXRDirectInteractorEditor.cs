using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.XR.Interaction.Toolkit;
using UnityEngine;

namespace CFoS.Interaction
{
    [CustomEditor(typeof(CustomXRDirectInteractor), true), CanEditMultipleObjects]
    public class InteractorEditor : XRDirectInteractorEditor
    {

        protected SerializedProperty m_ControllerRef;
        protected SerializedProperty m_TriggerRef;

        public static readonly string missingRequiredController = "This component requires a reference to an XR Controller component. Add one to ensure this component can respond to user input.";


        protected override void OnEnable()
        {
            base.OnEnable();

            m_ControllerRef = serializedObject.FindProperty("m_ControllerRef");
            m_TriggerRef = serializedObject.FindProperty("m_TriggerRef");
        }

        protected override void DrawProperties()
        {
            EditorGUILayout.PropertyField(m_ControllerRef);
            EditorGUILayout.PropertyField(m_TriggerRef);
            EditorGUILayout.Space(10);

            base.DrawProperties();
        }


        protected override void VerifyControllerPresent()
        {
            foreach (var targetObject in serializedObject.targetObjects)
            {
                var interactor = (CustomXRDirectInteractor) targetObject;
                if (interactor.ControllerRef == null)
                {
                    EditorGUILayout.HelpBox(missingRequiredController, MessageType.Warning, true);
                    break;
                }
            }
        }

    }
}