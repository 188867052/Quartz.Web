namespace Quartz.Javascript
{
    using System;
    using System.Globalization;
    using Quartz.Shared;

    /// <summary>
    /// Generates an identifier string.
    /// </summary>
    public class Identifier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Identifier" /> class.
        /// </summary>
        public Identifier()
        {
            this.Value = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
        }

        public static Identifier NewId => new Identifier();

        /// <summary>
        /// Gets the identifier value.
        /// </summary>
        public string Value { get; }

        public override string ToString()
        {
            Check.NotNull(this, nameof(Identifier));

            return this.Value;
        }
    }
}