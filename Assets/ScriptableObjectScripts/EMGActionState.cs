using UnityEngine;

[CreateAssetMenu(fileName = "New Transform Container", menuName
= "ScriptableObjects/EMG Action State")]
public class EMGActionState : ScriptableObject
{
	public bool telekinesisAktive;
	public string currentTelekinesisAction;
}