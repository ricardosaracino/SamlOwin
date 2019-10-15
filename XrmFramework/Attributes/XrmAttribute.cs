﻿using System;
using System.Reflection;

namespace XrmFramework.Attributes
{
    public class XrmAttribute : Attribute
    {
        public static EntityAttribute GetEntityAttribute(Type type)
        {
            return (EntityAttribute)GetCustomAttribute(type, typeof(EntityAttribute));
        }

        public static string GetEntityName(Type type)
        {
            EntityAttribute entityAttribute = GetEntityAttribute(type);

            if (entityAttribute == null)
            {
                return null;
            }

            return entityAttribute.EntityName;
        }

        public static ColumnAttribute GetColumnAttribute(PropertyInfo property)
        {
            return (ColumnAttribute)GetCustomAttribute(property, typeof(ColumnAttribute));
        }

        public static string GetLogicalName(PropertyInfo property)
        {
            ColumnAttribute columnAttribute = GetColumnAttribute(property);

            if (columnAttribute == null)
            {
                return null;
            }

            return columnAttribute.LogicalName;
        }

        public static IdAttribute GetIdAttribute(PropertyInfo element)
        {
            return (IdAttribute)GetCustomAttribute(element, typeof(IdAttribute));
        }

        public static LinkAttribute GetLinkAttribute(PropertyInfo property)
        {
            return (LinkAttribute)GetCustomAttribute(property, typeof(LinkAttribute));
        }

        public static NameAttribute GetNameAttribute(PropertyInfo element)
        {
            return (NameAttribute)GetCustomAttribute(element, typeof(NameAttribute));
        }

        public static PropertyInfo FindFirstIdProperty(Type element)
        {
            foreach (PropertyInfo property in element.GetProperties())
            {
                if (GetIdAttribute(property) != null)
                {
                    return property;
                }
            }

            return null;
        }

        public static PropertyInfo FindFirstNameProperty(Type element)
        {
            foreach (PropertyInfo property in element.GetProperties())
            {
                if (GetNameAttribute(property) != null)
                {
                    return property;
                }
            }

            return null;
        }
    }
}