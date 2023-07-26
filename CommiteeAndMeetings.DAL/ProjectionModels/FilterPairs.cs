using System;

namespace Models.ProjectionModels
{
    public class FilterPairs : IEquatable<FilterPairs>
    {
        public int key { get; set; }
        public string Value { get; set; }

        public bool Equals(FilterPairs other)
        {
            if (other is null)
            {
                return false;
            }

            return key == other.key && Value == other.Value;
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj as FilterPairs);
        }

        public override int GetHashCode()
        {
            return (key, Value).GetHashCode();
        }

    }
}
