using Microsoft.Xrm.Sdk.Query;
using System;

namespace XrmFramework.Attributes
{
    public class LinkAttribute : Attribute
    {
        // string linkFromEntityName
        // string linkToEntityName
        // string linkFromAttributeName
        // string linkToAttributeName
        // JoinOperator joinOperator

        public virtual string LinkToAttributeName { get; set; } = null;

        public virtual JoinOperator JoinOperator { get; set; }
    }
}