using System.Collections.Generic;

namespace MSBuildProjectOrganizer.Utilities
{

    /// <summary>
    /// A Maybe-type extension for having a Value or an exceptional value
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class Either<T1, T2>
    {
        /// <summary>
        /// The left-side value of this Either
        /// </summary>
        /// <value></value>
        public T1 Value { get; set; }
        /// <summary>
        /// The right-side value of this Either
        /// </summary>
        public T2 Exception { get; set; }

        /// <summary>
        /// Whether this Either has a value
        /// </summary>
        public bool HasValue => !EqualityComparer<T1>.Default.Equals(Value, default(T1));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="exception"></param>
        public Either(T1 value, T2 exception)
        {
            Value = value;
            Exception = exception;
        }

        /// <summary>
        /// Returns an either with only a right-side value (exceptional value)
        /// </summary>
        /// <param name="exception"></param>
        public static Either<T1, T2> Or(T2 exception)
        {
            return new Either<T1, T2>(default(T1), exception);
        }

        /// <summary>
        /// Returns an Either with only a left-side value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Either<T1, T2> WithoutException(T1 value)
        {
            var result = new Either<T1, T2>(value, default(T2));
            return result;
        }
    }
}