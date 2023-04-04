using System;
using System.Collections.Generic;
using UnityEngine;

public class GenericSharedVariable<ContainedType> : SharedVariable, ISerializationCallbackReceiver
{
    public virtual event Action<ContainedType> OnValueChange = delegate { };

    public virtual ContainedType CurrentValue
    {
        get { return currentValue; }
        set
        {
            if (IsSameValue(value, currentValue) == false)
            {
                currentValue = value;
                OnValueChange(value);
            }
        }
    }

    [field: SerializeField]
    protected ContainedType InitialValue { get; set; }

    protected ContainedType currentValue;

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        CurrentValue = InitialValue;
    }

    public void RevertToInitialValue()
    {
        CurrentValue = InitialValue;
    }

    private bool IsSameValue(ContainedType firstValue, ContainedType secondValue)
    {
        return EqualityComparer<ContainedType>.Default.Equals(firstValue, secondValue);
    }
}
