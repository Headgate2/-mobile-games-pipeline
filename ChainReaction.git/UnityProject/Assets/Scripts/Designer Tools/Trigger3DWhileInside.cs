using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Event/Event Trigger3D While Inside")]
[RequireComponent(typeof(Trigger3D), typeof(IntervalEvent))]
public class Trigger3DWhileInside : TriggerWhileInsideBase<Trigger3D, Collider, Trigger3D.ColliderEvent>
{
}
