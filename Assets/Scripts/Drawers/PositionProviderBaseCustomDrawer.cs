using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

[CustomPropertyDrawer(typeof(PositionProviderBase), true)]
public class BaseClassDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float totalHeight = EditorGUIUtility.singleLineHeight;

        if (property.managedReferenceValue != null)
        {
            var serializedObject = new SerializedObject(property.serializedObject.targetObject);
            var derivedClassProperty = serializedObject.FindProperty(property.propertyPath);

            var childProperty = derivedClassProperty.Copy();
            childProperty.NextVisible(true);

            while (childProperty.NextVisible(false))
            {
                totalHeight += EditorGUI.GetPropertyHeight(childProperty, true) + EditorGUIUtility.standardVerticalSpacing;
            }
        }
        return totalHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var derivedTypes = Assembly.GetAssembly(typeof(PositionProviderBase)).GetTypes()
            .Where(t => t.IsSubclassOf(typeof(PositionProviderBase)) && !t.IsAbstract).ToArray();

        string[] typeNames = derivedTypes.Select(t => t.Name).ToArray();

        Type currentType = property.managedReferenceValue?.GetType();
        int currentIndex = Array.IndexOf(derivedTypes, currentType);

        Rect dropdownRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        int newIndex = EditorGUI.Popup(dropdownRect, label.text, currentIndex, typeNames);

        if (newIndex != currentIndex)
        {
            property.managedReferenceValue = Activator.CreateInstance(derivedTypes[newIndex]);
        }

        if (property.managedReferenceValue != null)
        {
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();

            var childProperty = property.Copy();
            childProperty.NextVisible(true);

            Rect fieldPosition = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, position.width, EditorGUIUtility.singleLineHeight);

            while (childProperty.NextVisible(true))
            {
                if (childProperty.propertyType == SerializedPropertyType.Generic)
                {
                    childProperty.isExpanded = EditorGUI.Foldout(fieldPosition, childProperty.isExpanded, childProperty.displayName);

                    if (childProperty.isExpanded)
                    {
                        EditorGUI.indentLevel++;
                        SerializedProperty nestedProperty = childProperty.Copy();
                        SerializedProperty endProperty = nestedProperty.GetEndProperty();

                        while (nestedProperty.NextVisible(true) && !SerializedProperty.EqualContents(nestedProperty, endProperty))
                        {
                            fieldPosition.y += EditorGUI.GetPropertyHeight(nestedProperty, true) + EditorGUIUtility.standardVerticalSpacing;
                            EditorGUI.PropertyField(fieldPosition, nestedProperty, true);
                        }
                        EditorGUI.indentLevel--;
                    }
                }
                else
                {
                    EditorGUI.PropertyField(fieldPosition, childProperty, true);
                }

                fieldPosition.y += EditorGUI.GetPropertyHeight(childProperty, true) + EditorGUIUtility.standardVerticalSpacing;
                EditorGUILayout.Space(20f);
            }
        }

        EditorGUI.EndProperty();
    }
}
