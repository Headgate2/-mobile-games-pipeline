using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Event/Event Trigger2D While Inside")]
[RequireComponent(typeof(Trigger2D), typeof(IntervalEvent))]
public class Trigger2DWhileInside : TriggerWhileInsideBase<Trigger2D, Collider2D, Trigger2D.ColliderEvent>
{
}
