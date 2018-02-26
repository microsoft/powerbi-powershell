using System;

namespace AzureADWindowsAuthenticator
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class ParameterAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        public string Name { get; }

        public ParameterAttribute(string name) => this.Name = name;

        public bool Mandatory { get; set; }
    }
}
