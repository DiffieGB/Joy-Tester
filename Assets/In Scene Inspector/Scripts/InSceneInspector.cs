using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Reflection;
using System.Text.RegularExpressions;

public class InSceneInspector : MonoBehaviour {

	public MonoBehaviour inspectee;
    public String componentName;
    public float fieldSpacing = 30f;
    public GameObject stringPrefab;
    public GameObject boolPrefab;

	// Use this for initialization
	void Start () 
    {
        List<Component> components = new List<Component> { };
        inspectee.GetComponents<Component>(components);

		foreach (Component component in components) 
        {
            if (component.GetType().Name.Equals(componentName))
            {
                //FieldInfo[] fields = component.GetType().BaseType.GetFields( //TODO for base type as well
                FieldInfo[] fields = component.GetType().GetFields(
                         BindingFlags.Public | 
                         BindingFlags.NonPublic |
                         BindingFlags.Instance);

                int fieldsDrawn = 0;
                foreach (FieldInfo field in fields)
                {
                    //if (field.IsPublic || !field.IsNotSerialized)
                    {
                        GameObject clone;

                        if (field.FieldType.Equals(typeof(Boolean)))
                        {
                            clone = (GameObject)Instantiate(boolPrefab, Vector3.zero, Quaternion.identity);

                            bool value = (bool)field.GetValue(component);
                            Transform toggleTransform = clone.transform.Find("Toggle");
                            Toggle toggle = toggleTransform.GetComponent<Toggle>();
                            toggle.isOn = value;

                            int componentId = component.GetInstanceID();
                            Type componentType = component.GetType();
                            string fieldName = field.Name;
                            toggle.onValueChanged.AddListener(delegate(bool call)
                            {
                                SetFieldValue(componentId, componentType, fieldName, call);
                            });
                        }
                        else
                        {
                            clone = (GameObject)Instantiate(stringPrefab, Vector3.zero, Quaternion.identity);

                            object value = field.GetValue(component);
                            Transform inputField = clone.transform.Find("Input Field");
                            Transform placeholder = inputField.Find("Placeholder");
                            Text text = placeholder.GetComponent<Text>();
                            text.text = value.ToString();
                            InputField inputFieldComponent = inputField.GetComponent<InputField>();

                            int componentId = component.GetInstanceID();
                            Type componentType = component.GetType();
                            string fieldName = field.Name;
                            Type fieldType = field.FieldType;
                            inputFieldComponent.onEndEdit.AddListener(delegate(string str)
                            {
                                object convertedValue = str;

                                if (fieldType.Equals(typeof(Vector2)))
                                {
                                    string[] parsedString = str.Split(new char[] { ' ', ',', ')', '(' }, StringSplitOptions.RemoveEmptyEntries);
                                    convertedValue = new Vector2(float.Parse(parsedString[0]), float.Parse(parsedString[1]));
                                }
                                else if (fieldType.Equals(typeof(Vector3)))
                                {
                                    string[] parsedString = str.Split(new char[] { ' ', ',', ')', '(' }, StringSplitOptions.RemoveEmptyEntries);
                                    convertedValue = new Vector3(float.Parse(parsedString[0]), float.Parse(parsedString[1]), float.Parse(parsedString[2]));
                                }

                                SetFieldValue(componentId, componentType, fieldName, convertedValue);
                            });
                        }

                        clone.transform.SetParent(transform, false);
                        clone.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, -fieldSpacing * fieldsDrawn - 10);
                        clone.transform.Find("Field Name").GetComponent<Text>().text = GetInspectorStyleName(field.Name);

                        fieldsDrawn++;
                    }
                }

                //PropertyInfo[] properties = component.GetType().GetProperties();

                //int propertiesDrawn = fieldsDrawn;
                //foreach (PropertyInfo property in properties)
                //{
                //    if (property.CanWrite)
                //    {
                //        GameObject clone;

                //        if (property.PropertyType.Equals(typeof(Boolean)))
                //        {
                //            clone = (GameObject)Instantiate(boolPrefab, Vector3.zero, Quaternion.identity);

                //            bool value = (bool)property.GetValue(component, null);
                //            Transform toggleTransform = clone.transform.Find("Toggle");
                //            Toggle toggle = toggleTransform.GetComponent<Toggle>();
                //            toggle.isOn = value;

                //            int componentId = component.GetInstanceID();
                //            Type componentType = component.GetType();
                //            string propertyName = property.Name;
                //            toggle.onValueChanged.AddListener(delegate(bool call)
                //            {
                //                SetPropertyValue(componentId, componentType, propertyName, call);
                //            });
                //        }
                //        else
                //        {
                //            clone = (GameObject)Instantiate(stringPrefab, Vector3.zero, Quaternion.identity);

                //            object value = property.GetValue(component, null);
                //            Transform inputField = clone.transform.Find("Input Field");
                //            Transform placeholder = inputField.Find("Placeholder");
                //            Text text = placeholder.GetComponent<Text>();
                //            text.text = value.ToString();
                //            InputField inputFieldComponent = inputField.GetComponent<InputField>();

                //            int componentId = component.GetInstanceID();
                //            Type componentType = component.GetType();
                //            string propertyName = property.Name;
                //            Type propertyType = property.PropertyType;
                //            inputFieldComponent.onEndEdit.AddListener(delegate(string str) {
                //                object convertedValue = str;
                                
                //                if (propertyType.Equals(typeof(Vector2)))
                //                {
                //                    string[] parsedString = str.Split(new char[] { ' ', ',', ')', '(' }, StringSplitOptions.RemoveEmptyEntries);
                //                    convertedValue = new Vector2(float.Parse(parsedString[0]), float.Parse(parsedString[1]));
                //                }
                //                else if (propertyType.Equals(typeof(Vector3)))
                //                {
                //                    string[] parsedString = str.Split(new char[] { ' ', ',', ')', '(' }, StringSplitOptions.RemoveEmptyEntries);
                //                    convertedValue = new Vector3(float.Parse(parsedString[0]), float.Parse(parsedString[1]), float.Parse(parsedString[2]));
                //                }

                //                SetPropertyValue(componentId, componentType, propertyName, convertedValue);
                //            });
                //        }

                //        clone.transform.SetParent(transform, false);
                //        clone.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, -fieldSpacing * propertiesDrawn - 10);
                //        clone.transform.Find("Field Name").GetComponent<Text>().text = GetInspectorStyleName(property.Name);

                //        propertiesDrawn++;
                //    }
                //}
            }
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static string GetInspectorStyleName(string str)
    {
        str = Regex.Replace(str, @"_", "");

        return Regex.Replace(
            Regex.Replace(
                str,
                @"(\P{Ll})(\P{Ll}\p{Ll})",
                "$1 $2"
            ),
            @"(\p{Ll})(\P{Ll})",
            "$1 $2"
        );
    }

    private void SetPropertyValue(int componentId, Type componentType, string propertyName, object value)
    {
        Component[] components = (Component[])Component.FindObjectsOfType(componentType);
        foreach (Component component in components)
        {
            if (component.GetInstanceID() == componentId)
            {
                PropertyInfo property = component.GetType().GetProperty(propertyName);
                Debug.Log("Setting " + component.ToString() + " property " + propertyName.ToString() + " to (" + property.PropertyType.ToString() + ") " + value.ToString());
                property.SetValue(component, Convert.ChangeType(value, property.PropertyType), null);
            }
        }
    }

    private void SetFieldValue(int componentId, Type componentType, string fieldName, object value)
    {
        Component[] components = (Component[])Component.FindObjectsOfType(componentType);
        foreach (Component component in components)
        {
            if (component.GetInstanceID() == componentId)
            {
                FieldInfo field = component.GetType().GetField(fieldName,
                         BindingFlags.Public |
                         BindingFlags.NonPublic |
                         BindingFlags.Instance);
                Debug.Log("Setting " + component.ToString() + " field " + fieldName.ToString() + " to (" + field.FieldType.ToString() + ") " + value.ToString());
                field.SetValue(component, Convert.ChangeType(value, field.FieldType));
            }
        }
    }
}
