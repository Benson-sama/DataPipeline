//-------------------------------------------------------------------------------
// <copyright file="ConnectionsChangedEventArgs.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the ConnectionsChangedEventArgs class.</summary>
//-------------------------------------------------------------------------------
namespace DataPipeline.Model
{
    using System;
    using System.Collections.Generic;
    using DataPipeline.Model.ReflectedDataUnits;

    /// <summary>
    /// Represents the <see cref="ConnectionsChangedEventArgs"/> class.
    /// </summary>
    public class ConnectionsChangedEventArgs
    {
        /// <summary>
        /// The change text of this <see cref="ConnectionsChangedEventArgs"/>.
        /// </summary>
        private string change;

        /// <summary>
        /// The affected <see cref="KeyValuePair{TKey, TValue}"/>.
        /// </summary>
        private KeyValuePair<ReflectedDataUnit, ReflectedDataUnit> keyValuePair;

        /// <summary>
        /// Initialises a new instance of the <see cref="ConnectionsChangedEventArgs"/> class.
        /// </summary>
        /// <param name="change">The change text.</param>
        /// <param name="keyValuePair">The affected <see cref="KeyValuePair{TKey, TValue}"/>.</param>
        public ConnectionsChangedEventArgs(string change, KeyValuePair<ReflectedDataUnit, ReflectedDataUnit> keyValuePair)
        {
            this.Change = change;
            this.KeyValuePair = keyValuePair;
        }

        /// <summary>
        /// Gets the change text of this <see cref="ConnectionsChangedEventArgs"/>.
        /// </summary>
        /// <value>The change text of this <see cref="ConnectionsChangedEventArgs"/>.</value>
        public string Change
        {
            get => this.change;

            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(value), "The specified value cannot be nul or empty.");
                }

                this.change = value;
            }
        }

        /// <summary>
        /// Gets the affected <see cref="KeyValuePair{TKey, TValue}"/>.
        /// </summary>
        /// <value>The affected <see cref="KeyValuePair{TKey, TValue}"/>.</value>
        public KeyValuePair<ReflectedDataUnit, ReflectedDataUnit> KeyValuePair
        {
            get => this.keyValuePair;

            private set
            {
                if (value.Key == null || value.Value == null)
                {
                    throw new ArgumentNullException("The specified key value pair cannot contain null values.");
                }

                this.keyValuePair = value;
            }
        }
    }
}
