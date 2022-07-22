using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Transform Container", menuName
= "ScriptableObjects/Selected Object")]
public class SelectedObject : ScriptableObject, ISerializationCallbackReceiver
{
	public GameObject initSelectedGameObject = null;

	[NonSerialized]
	public GameObject selectedGameObject;

	public void OnAfterDeserialize()
	{
		selectedGameObject = initSelectedGameObject;
	}

	public void OnBeforeSerialize() { }
}