using System;

namespace Paia.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class InjectAttribute : Attribute
    {
    }
}
