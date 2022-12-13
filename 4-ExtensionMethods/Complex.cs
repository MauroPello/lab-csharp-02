namespace ExtensionMethods
{
    using System;

    /// <inheritdoc cref="IComplex"/>
    public class Complex : IComplex
    {
        private const double TOLERANCE = 0.0000001;

        private readonly double re;
        private readonly double im;

        /// <summary>
        /// Initializes a new instance of the <see cref="Complex"/> class.
        /// </summary>
        /// <param name="re">the real part.</param>
        /// <param name="im">the imaginary part.</param>
        public Complex(double re, double im)
        {
            this.re = re;
            this.im = im;
        }

        /// <inheritdoc cref="IComplex.Real"/>
        public double Real
        {
            get => this.re;
        }

        /// <inheritdoc cref="IComplex.Imaginary"/>
        public double Imaginary
        {
            get => this.im;
        }

        /// <inheritdoc cref="IComplex.Modulus"/>
        public double Modulus
        {
            get => Math.Sqrt(this.re * this.re + this.im * this.im);
        }

        /// <inheritdoc cref="IComplex.Phase"/>
        public double Phase
        {
            get => Math.Atan2(this.im, this.re);
        }

        /// <inheritdoc cref="IComplex.ToString"/>
        public override string ToString()
        {
            string real = this.Real == 0 ? "" : this.Real.ToString();
            string sign = this.Imaginary > 0 ? "+ i" : "- i";
            string imaginary = Math.Abs(this.Imaginary).ToString();

            if (this.Real != 0)
            {
                sign = this.Imaginary > 0 ? " + i" : " - i";
            }
            else
            {
                sign = this.Imaginary > 0 ? "i" : "-i";
            }

            if (Math.Abs(this.Imaginary) == 1 || this.Imaginary == 0)
            {
                imaginary = "";
                if (this.Imaginary == 0)
                {
                    sign = "";
                    if (this.Real == 0)
                    {
                        real = "0";
                    }
                }
            }

            return real + sign + imaginary;
        }

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        // non funziona in quanto le divisioni calcolate dal PC non sono infinitamente precise (ovviamente!)
        // public bool Equals(IComplex other) => Real.Equals(other.Real) && Imaginary.Equals(other.Imaginary);
        public bool Equals(IComplex other) {
            return other != null && Math.Abs(Real - other.Real) < TOLERANCE
                   && Math.Abs(Imaginary - other.Imaginary) < TOLERANCE;
        }
        /// <inheritdoc cref="object.Equals(object?)"/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Complex)obj);
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode() => HashCode.Combine(Real, Imaginary);
    }
}
