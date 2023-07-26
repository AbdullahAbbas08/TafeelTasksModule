using System;
using System.Collections.Generic;
using System.Linq;

namespace CommiteeAndMeetings.Service.Sevices
{
    public static class DictionaryExtension
    {
        public static T GetValue<T>(this Dictionary<string, object> args, string key)
        {
            object value = args.Where(x => x.Key == key).Select(x => x.Value).FirstOrDefault();

            var t = typeof(T);

            if ((t.FullName == "System.Boolean" && value == null) || (t.IsConstructedGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>))))
            {
                if (value == null || Convert.ToBoolean(value) == false)
                {
                    return default(T);
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return (T)Convert.ChangeType(value, t);
        }
    }
}
