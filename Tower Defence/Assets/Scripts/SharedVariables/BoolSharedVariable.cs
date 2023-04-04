using UnityEngine;

[CreateAssetMenu(menuName = "SharedVariables/BoolSharedVariable")]
public class BoolSharedVariable : GenericSharedVariable<bool>, ISerializationCallbackReceiver
{

}
