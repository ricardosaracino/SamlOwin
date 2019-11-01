using System;

namespace SamlOwin.Models
{
    /// <summary>
    /// Maps a Microsoft.Xrm.Sdk.EntityReference
    /// </summary>
    public class ReferenceResponse
    {
        /// <summary>
        /// Entity Primary Id Attribute
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Entity Primary Name Attribute
        /// </summary>
        public string Name { get; set; }
    }
}