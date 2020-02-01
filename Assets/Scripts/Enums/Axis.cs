namespace Assets.Scripts.Enums
{
    using System.ComponentModel;

    using UnityEngine;

    /// <summary>
    /// The axis.
    /// </summary>
    internal enum Axis
    {
        [Description("Vertical")]
        Vertical,

        [Description("Horizontal")]
        Horizontal,

        [Description("Fire1")]
        Fire1,

        [Description("Fire2")]
        Fire2,

        [Description("Fire3")]
        Fire3,

        [Description("Jump")]
        Jump,

        [Description("Mouse X")]
        MouseX,

        [Description("Mouse Y")]
        MouseY,

        [Description("Mouse ScrollWheel")]
        MouseScrollWheel,

        [Description("Submit")]
        Submit,

        [Description("Cancel")]
        Cancel
    }
}