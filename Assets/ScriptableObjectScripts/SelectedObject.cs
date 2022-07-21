using UnityEngine;

[CreateAssetMenu(fileName = "New Transform Container", menuName
= "ScriptableObjects/Selected Object")]
public class SelectedObject : ScriptableObject
{
	public GameObject selectedGameObject;
}