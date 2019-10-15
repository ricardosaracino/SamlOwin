﻿using System;

namespace XrmFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityAttribute : Attribute
    {
        public EntityAttribute(string entityName)
        {
            this.EntityName = entityName;
        }

        public virtual string EntityName { get; }
    }
}