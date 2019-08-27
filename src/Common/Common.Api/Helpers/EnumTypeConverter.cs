/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;

namespace Microsoft.PowerBI.Common.Api.Helpers
{
    public static class EnumTypeConverter
    {
        public static T ConvertTo<T,V>(V value) 
            where T : struct, IConvertible
            where V : struct, IConvertible
        {
            if(!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an Enum");
            }

            if (!typeof(V).IsEnum)
            {
                throw new ArgumentException("V must be an Enum");
            }

            string valueAsString = value.ToString();
            return (T)Enum.Parse(typeof(T), valueAsString);
        }

        public static T ConvertTo<T, V>(V value, T returnIfParseFails)
            where T : struct, IConvertible
            where V : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an Enum");
            }

            if (!typeof(V).IsEnum)
            {
                throw new ArgumentException("V must be an Enum");
            }

            string valueAsString = value.ToString();
            if(Enum.TryParse<T>(valueAsString, out T result))
            {
                return result;
            }

            return returnIfParseFails;
        }

        public static Nullable<T> ConvertTo<T, V>(Nullable<V> value)
            where T : struct, IConvertible
            where V : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an Enum");
            }

            if (!typeof(V).IsEnum)
            {
                throw new ArgumentException("V must be an Enum");
            }

            if(!value.HasValue)
            {
                return null;
            }

            string valueAsString = value.ToString();
            if(string.IsNullOrEmpty(valueAsString))
            {
                return null;
            }

            return (T)Enum.Parse(typeof(T), valueAsString);
        }

        public static Nullable<T> ConvertTo<T, V>(Nullable<V> value, Nullable<T> returnIfParseFails)
            where T : struct, IConvertible
            where V : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an Enum");
            }

            if (!typeof(V).IsEnum)
            {
                throw new ArgumentException("V must be an Enum");
            }

            if (!value.HasValue)
            {
                return null;
            }

            string valueAsString = value.ToString();
            if (string.IsNullOrEmpty(valueAsString))
            {
                return null;
            }

            if(Enum.TryParse<T>(valueAsString, out T result))
            {
                return result;
            }

            return returnIfParseFails;
        }
    }
}
