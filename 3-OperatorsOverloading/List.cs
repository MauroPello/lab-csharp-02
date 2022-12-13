using System;

namespace OperatorsOverloading
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The class models an immutable linked list.
    /// </summary>
    /// <typeparam name="TValue">the type of the items of the list.</typeparam>
    public abstract class List<TValue>
    {
        /// <summary>
        /// Gets the first element of the list, which is called "head" of the list.
        /// </summary>
        public abstract TValue Head { get; }

        /// <summary>
        /// Gets the other elements of the list, which are called "tail" of the list.
        /// </summary>
        public abstract List<TValue> Tail { get; }

        /// <summary>
        /// Gets a value indicating whether the list is empty or not.
        /// </summary>
        public abstract bool IsNil { get; }

        /// <summary>
        /// Gets the length of the list.
        /// </summary>
        public abstract int Length { get; }

        /// <summary>
        /// Converts the given array into a new list implicitly.
        /// </summary>
        /// <param name="enumerable">the array of elements to put on the list.</param>
        /// <returns>a new list with the given elements.</returns>
        public static implicit operator List<TValue>(TValue[] enumerable) => List.From(enumerable.AsEnumerable<TValue>());

        /// <summary>
        /// Converts the given element into a new list implicitly.
        /// </summary>
        /// <param name="element">the element to put on the list.</param>
        /// <returns>a new list with only the given element.</returns>
        public static implicit operator List<TValue>(TValue element) => List.Of(element);

        /// <summary>
        /// Converts the given list into a new array explicitly.
        /// </summary>
        /// <param name="list">the list to transform.</param>
        /// <returns>an array containing the elements of the list.</returns>
        public static explicit operator TValue[](List<TValue> list) => list.ToFlat().ToArray();

        /// <summary>
        /// Determines whether two lists are equal by comparing each of the elements of the lists.
        /// </summary>
        /// <param name="list1">a list to compare to <paramref name="list2"/>.</param>
        /// <param name="list2">a list to compare to <paramref name="list1"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the two source lists are of equal length
        /// and their corresponding elements are equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(List<TValue> list1, List<TValue> list2)
        {
            if (list1 is null || list2 is null) 
            {
                return ReferenceEquals(list1, list2);
            }
            return Enumerable.SequenceEqual(list1.ToFlat(), list2.ToFlat());
        }

        /// <summary>
        /// Determines whether two lists are not equal by comparing each of the elements of the lists.
        /// </summary>
        /// <param name="list1">a list to compare to <paramref name="list2"/>.</param>
        /// <param name="list2">a list to compare to <paramref name="list1"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the two source lists are not equal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(List<TValue> list1, List<TValue> list2) => !(list1 == list2);

        /// <summary>
        /// Determines whether the <see cref="Length"/> of the <paramref name="list1"/> is greater or equal
        /// of the <paramref name="list2"/>'s.
        /// </summary>
        /// <param name="list1">a list to compare to <paramref name="list2"/>.</param>
        /// <param name="list2">a list to compare to <paramref name="list1"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the first list is longer or equal to the second,
        /// <see langword="false"/> otherwise.
        /// </returns>
        public static bool operator >=(List<TValue> list1, List<TValue> list2) => list1.Length >= list2.Length;

        /// <summary>
        /// Determines whether the <see cref="Length"/> of the <paramref name="list1"/> is lower or equal
        /// of the <paramref name="list2"/>'s.
        /// </summary>
        /// <param name="list1">a list to compare to <paramref name="list2"/>.</param>
        /// <param name="list2">a list to compare to <paramref name="list1"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the first list is shorter or equal to the second,
        /// <see langword="false"/> otherwise.
        /// </returns>
        public static bool operator <=(List<TValue> list1, List<TValue> list2) => list1.Length <= list2.Length;

        /// <summary>
        /// Determines whether the <paramref name="list1"/> is shorter than the <paramref name="list2"/>.
        /// </summary>
        /// <param name="list1">a list to compare to <paramref name="list2"/>.</param>
        /// <param name="list2">a list to compare to <paramref name="list1"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the first list is shorter than the second, <see langword="false"/> otherwise.
        /// </returns>
        public static bool operator <(List<TValue> list1, List<TValue> list2) => list1.Length < list2.Length;

        /// <summary>
        /// Determines whether the <paramref name="list1"/> is longer than the <paramref name="list2"/>.
        /// </summary>
        /// <param name="list1">a list to compare to <paramref name="list2"/>.</param>
        /// <param name="list2">a list to compare to <paramref name="list1"/>.</param>
        /// <returns>
        /// <see langword="true"/> if the first list is longer than the second, <see langword="false"/> otherwise.
        /// </returns>
        public static bool operator >(List<TValue> list1, List<TValue> list2) => list1.Length > list2.Length;

        /// <summary>
        /// Chains the two lists together by appending <paramref name="list2"/> to <paramref name="list1"/>.
        /// </summary>
        /// <param name="list1">the first list.</param>
        /// <param name="list2">the second list.</param>
        /// <returns>the result list.</returns>
        public static List<TValue> operator +(List<TValue> list1, List<TValue> list2) => List.Append(list1, list2);

        /// <summary>
        /// Returns a list which contains only the items of <paramref name="list1"/>
        /// which are not included in <paramref name="list2"/>.
        /// </summary>
        /// <param name="list1">the first list.</param>
        /// <param name="list2">the second list.</param>
        /// <returns>the result list.</returns>
        public static List<TValue> operator -(List<TValue> list1, List<TValue> list2) 
        {
            // non funziona perchè Except rimuove anche le ripetizioni degli elementi
            // List.From(list1.ToFlat().Except(list2.ToFlat()));
            return List.From(list1.ToFlat().Where(i => !list2.ToFlat().Contains(i)));
        }

        /// <summary>
        /// Converts this list into a list of lists, which are each one the tail of the previous one.
        /// </summary>
        /// <returns>a list of lists</returns>
        public IEnumerable<List<TValue>> Flatten()
        {
            for (List<TValue> curr = this; !curr.IsNil; curr = curr.Tail)
            {
                yield return curr;
            }
        }

        /// <summary>
        /// Flatten this list into an enumeration of each item.
        /// </summary>
        /// <returns>an enumeration of each item of this list.</returns>
        public IEnumerable<TValue> ToFlat() =>
            this.Flatten().Select(ht => ht.Head);

        /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
        public bool Equals(List<TValue> other)
        {
            return this == other;
        }

        /// <inheritdoc cref="object.Equals(object?)" />
        public override bool Equals(object obj)
        {
            return obj is List<TValue> list && this == list;
        }

        /// <inheritdoc cref="object.GetHashCode" />
        public override int GetHashCode()
        {
            return HashCode.Combine(this.Head, this.Tail, this.IsNil, this.Length);
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() =>
            "[ " + string.Join(", ", this.Flatten().Select(l => l.Head)) + " ]";

        /// <inheritdoc cref="List{TValue}"/>
        internal sealed class HeadTail<THead> : List<THead>
        {
            /// <inheritdoc cref="List{TValue}.Head"/>
            public override THead Head { get; }

            /// <inheritdoc cref="List{TValue}.Tail"/>
            public override List<THead> Tail { get; }

            public HeadTail(THead head, List<THead> tail)
            {
                this.Head = head;
                this.Tail = tail ?? new List<THead>.Empty<THead>();
            }

            /// <inheritdoc cref="List{TValue}.IsNil"/>
            public override bool IsNil => false;

            /// <inheritdoc cref="List{TValue}.Length"/>
            public override int Length => 1 + this.Tail.Length;
        }

        /// <summary>
        /// The class models an empty list.
        /// </summary>
        /// <inheritdoc cref="List{TValue}"/>
        internal sealed class Empty<TList> : List<TList>
        {
            /// <inheritdoc cref="List{TValue}.Head"/>
            public override TList Head => default;

            /// <inheritdoc cref="List{TValue}.Tail"/>
            public override List<TList> Tail => null;

            /// <inheritdoc cref="List{TValue}.IsNil"/>
            public override bool IsNil => true;

            /// <inheritdoc cref="List{TValue}.Length"/>
            public override int Length => 0;
        }
    }

    /// <summary>
    /// Utility class for <see cref="List{TValue}"/>.
    /// </summary>
    public static class List
    {
        /// <summary>
        /// Build a new list from the given element.
        /// </summary>
        /// <typeparam name="TItem">the type of the items of the list.</typeparam>
        /// <param name="head">the element to use as head of the list.</param>
        /// <returns>a new list with only the given element.</returns>
        public static List<TItem> Of<TItem>(TItem head) => List.Of(head, Nil<TItem>());

        /// <summary>
        /// Build a new list from the given elements.
        /// </summary>
        /// <typeparam name="TItem">the type of the items of the list.</typeparam>
        /// <param name="head">the element to use as head of the list.</param>
        /// <param name="tail">the elements to use as tail of the list.</param>
        /// <returns>a new list with the given elements.</returns>
        public static List<TItem> Of<TItem>(TItem head, List<TItem> tail) =>
            new List<TItem>.HeadTail<TItem>(head, tail);

        /// <summary>
        /// Build a new empty list.
        /// </summary>
        /// <typeparam name="TItem">the type of the items of the list.</typeparam>
        /// <returns>a new empty list.</returns>
        public static List<TItem> Nil<TItem>() => new List<TItem>.Empty<TItem>();

        /// <summary>
        /// Build a new list from the given enumeration of elements.
        /// </summary>
        /// <param name="items">the elements to put on the list.</param>
        /// <typeparam name="TItem">the type of the items of the list.</typeparam>
        /// <returns>a new list with the given elements.</returns>
        public static List<TItem> From<TItem>(IEnumerable<TItem> items)
        {
            var elems = items.Reverse().ToArray();
            List<TItem> curr = List.Of(elems.First());

            foreach (var e in elems.Skip(1))
            {
                curr = List.Of(e, curr);
            }

            return curr;
        }

        /// <summary>
        /// Build a new list from one or more elements.
        /// </summary>
        /// <param name="item1">the first element to put on the list.</param>
        /// <param name="items">the other elements to put on the list, if any.</param>
        /// <typeparam name="TItem">the type of the items of the list.</typeparam>
        /// <returns>a new list with the given elements.</returns>
        public static List<TItem> From<TItem>(TItem item1, params TItem[] items) =>
            List.From(Enumerable.Repeat(item1, 1).Concat(items));

        /// <summary>
        /// Append <paramref name="list2"/> to <paramref name="list1"/>.
        /// </summary>
        /// <param name="list1">the first list.</param>
        /// <param name="list2">the second list.</param>
        /// <typeparam name="TItem">the type of the items of the list.</typeparam>
        /// <returns>the result list.</returns>
        public static List<TItem> Append<TItem>(List<TItem> list1, List<TItem> list2)
        {
            List<TItem> curr;

            if (list1.IsNil)
            {
                curr = list2;
            }
            else if (list2.IsNil)
            {
                curr = list1;
            }
            else
            {
                var elems = list1.Flatten().Select(l => l.Head).Reverse().ToArray();
                curr = List.Of(elems.First(), list2);

                foreach (var e in elems.Skip(1))
                {
                    curr = List.Of(e, curr);
                }
            }

            return curr;
        }
    }
}
