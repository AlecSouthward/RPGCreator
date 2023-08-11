// the interface used for all interactibles
using UnityEngine;
using UnityEngine.Events;

public interface Interactible
{
    /// <summary>
    /// Returns 1 if passed true or -1 if passed false.
    /// </summary>
    /// 
    /// <remarks>
    /// HEHEHAHA
    /// </remarks>
    /// 
    /// <param name="myVar">Parameter value to pass.</param>
    /// 
    /// <returns>Returns an integer based on the passed value.</returns>
    public void Interact();

    // really need to think of a way to serialize
    // these UnityEvents (maybe try Action?)
    public UnityEvent onInteractFinish { get; set; }
}