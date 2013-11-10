﻿// This file is part of HangFire.
// Copyright © 2013 Sergey Odinokov.
// 
// HangFire is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// HangFire is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with HangFire.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Concurrent;
using System.Linq;

namespace HangFire.Filters
{
    /// <summary>
    /// Represents the base class for job filter attributes.
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public abstract class JobFilterAttribute : Attribute, IJobFilter
    {
        private static readonly ConcurrentDictionary<Type, bool> MultiuseAttributeCache = new ConcurrentDictionary<Type, bool>();
        private int _order = JobFilter.DefaultOrder;

        public bool AllowMultiple
        {
            get { return AllowsMultiple(GetType()); }
        }

        public int Order
        {
            get { return _order; }
            set
            {
                if (value < JobFilter.DefaultOrder)
                {
                    throw new ArgumentOutOfRangeException("value", "The Order value should be more that -1");
                }
                _order = value;
            }
        }

        private static bool AllowsMultiple(Type attributeType)
        {
            return MultiuseAttributeCache.GetOrAdd(
                attributeType,
                type => type.GetCustomAttributes(typeof(AttributeUsageAttribute), true)
                            .Cast<AttributeUsageAttribute>()
                            .First()
                            .AllowMultiple);
        }
    }
}