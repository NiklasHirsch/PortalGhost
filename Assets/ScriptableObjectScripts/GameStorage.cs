using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Transform Container", menuName
= "ScriptableObjects/Game Storage")]
public class GameStorage : ScriptableObject, ISerializationCallbackReceiver
{
    public Quaternion init_portal_wall_base_rotation;

    [NonSerialized]
	public Quaternion portal_wall_base_rotation;

    public void OnAfterDeserialize()
    {
        portal_wall_base_rotation = init_portal_wall_base_rotation;
    }

    public void OnBeforeSerialize(){}
}