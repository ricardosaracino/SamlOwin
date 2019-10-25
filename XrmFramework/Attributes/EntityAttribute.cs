﻿using System;

namespace XrmFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class EntityAttribute : Attribute
    {
        public EntityAttribute(string entityName)
        {
            EntityName = entityName;
        }

        public string EntityName { get; }
    }
}