//-------------------------------------------------------------------------
// <copyright file="RandomNumberGenerator.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the RandomNumberGenerator class.</summary>
//-------------------------------------------------------------------------
namespace RandomNumberGenerator
{
    using System;
    using System.Threading;
    using DataPipeline.Model.Attributes;
    using DataUnits;

    /// <summary>
    /// Represents the <see cref="RandomNumberGenerator"/> class.
    /// </summary>
    [DataUnitInformation(
        name: "Random number generator",
        Description = "This data unit is used to generate random numbers every two seconds.",
        InputDatatype = typeof(void),
        InputDescription = "This data unit has no data input.",
        OutputDatatype = typeof(int),
        OutputDescription = "Outputs a number between 1 and 100, including borders.")]
    public class RandomNumberGenerator
    {
        /// <summary>
        /// The <see cref="Random"/> instance used to generate random numbers.
        /// </summary>
        private readonly Random random;

        /// <summary>
        /// The worker <see cref="Thread"/>.
        /// </summary>
        private Thread thread;

        /// <summary>
        /// The arguments for the worker <see cref="Thread"/>.
        /// </summary>
        private RandomNumberGeneratorThreadArguments threadArguments;

        /// <summary>
        /// Initialises a new instance of the <see cref="RandomNumberGenerator"/> class.
        /// </summary>
        public RandomNumberGenerator()
        {
            this.random = new Random();
            this.threadArguments = new RandomNumberGeneratorThreadArguments();
        }

        /// <summary>
        /// The event that gets fired when a new value got generated.
        /// </summary>
        [DataOutput]
        public event EventHandler<ValueOutputEventArgs<int>> ValueGenerated;

        /// <summary>
        /// Starts this data unit.
        /// </summary>
        public void Start()
        {
            if (this.thread != null && this.thread.IsAlive)
            {
                return;
            }

            this.threadArguments.Exit = false;
            this.thread = new Thread(this.Worker);
            this.thread.Start(this.threadArguments);
        }

        /// <summary>
        /// Stops this data unit.
        /// </summary>
        public void Stop()
        {
            if (this.thread == null || !this.thread.IsAlive)
            {
                return;
            }

            this.threadArguments.Exit = true;
        }

        /// <summary>
        /// Represents the worker <see cref="Thread"/>.
        /// </summary>
        /// <param name="data">The given <see cref="RandomNumberGeneratorThreadArguments"/>.</param>
        private void Worker(object data)
        {
            if (!(data is RandomNumberGeneratorThreadArguments))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(data),
                    $"The specified data must be an instance of the {nameof(RandomNumberGeneratorThreadArguments)} class.");
            }

            RandomNumberGeneratorThreadArguments args = (RandomNumberGeneratorThreadArguments)data;

            while (!args.Exit)
            {
                ValueOutputEventArgs<int> valueOutputEventArgs = new ValueOutputEventArgs<int>(this.random.Next(1, 101));
                this.ValueGenerated?.Invoke(this, valueOutputEventArgs);
                Thread.Sleep(2000);
            }
        }
    }
}
