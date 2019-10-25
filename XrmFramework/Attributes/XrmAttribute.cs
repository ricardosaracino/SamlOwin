﻿using System;
 using System.Linq;
 using System.Reflection;

namespace XrmFramework.Attributes
{
    public static class XrmAttribute 
    {
        public static EntityAttribute GetEntityAttribute(Type type)
        {
            return (EntityAttribute)Attribute.GetCustomAttribute(type, typeof(EntityAttribute));
        }

        public static string GetEntityName(Type type)
        {
            var entityAttribute = GetEntityAttribute(type);

            return entityAttribute?.EntityName;
        }

        public static ColumnAttribute GetColumnAttribute(PropertyInfo property)
        {
            return (ColumnAttribute)Attribute.GetCustomAttribute(property, typeof(ColumnAttribute));
        }

        public static string GetLogicalName(PropertyInfo property)
        {
            var columnAttribute = GetColumnAttribute(property);

            return columnAttribute?.LogicalName;
        }

        public static IdAttribute GetIdAttribute(PropertyInfo element)
        {
            return (IdAttribute)Attribute.GetCustomAttribute(element, typeof(IdAttribute));
        }

        public static LinkAttribute GetLinkAttribute(PropertyInfo property)
        {
            return (LinkAttribute)Attribute.GetCustomAttribute(property, typeof(LinkAttribute));
        }

        public static NameAttribute GetNameAttribute(PropertyInfo element)
        {
            return (NameAttribute)Attribute.GetCustomAttribute(element, typeof(NameAttribute));
        }

        public static PropertyInfo FindFirstIdProperty(Type element)
        {
            return element.GetProperties().FirstOrDefault(property => GetIdAttribute(property) != null);
        }

        public static PropertyInfo FindFirstNameProperty(Type element)
        {
            return element.GetProperties().FirstOrDefault(property => GetNameAttribute(property) != null);
        }
    }
}