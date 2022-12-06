using System;
using System.Text;
using System.Collections.Generic;

namespace HL7Parser
{
    public class Segment
    {
        public Dictionary<int, string> Fields { get; set; }

        public Segment()
        {
            Fields = new Dictionary<int, string>(100);
        }

        public Segment(string name)
        {
            Fields = new Dictionary<int, string>(100)
            {
                { 0, name }
            };
        }

        /// <summary>
        /// First field of a segment is the name of the segment
        /// </summary>
        public string Name
        {
            get
            {
                if (!Fields.ContainsKey(0))
                {
                    return string.Empty;
                }
                return Fields[0];
            }
        }

        /// <summary>
        /// Only | (vertical bar) is used as field separator character
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Field(int key)
        {
            if (Name == "MSH" && key == 1) return "|";

            if (!Fields.ContainsKey(key))
            {
                return string.Empty;
            }
            return Fields[key];
        }

        /// <summary>
        /// Only | (vertical bar) is used as field separator character
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public void Field(int key, string value)
        {
            if (Name == "MSH" && key == 1) return;

            if (!string.IsNullOrEmpty(value))
            {
                if (Fields.ContainsKey(key))
                {
                    Fields.Remove(key);
                }

                Fields.Add(key, value);
            }
        }

        /// <summary>
        /// Only | (vertical bar) is used as field separator character
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        public void DeSerializeSegment(string segment)
        {
            int count = 0;
            char[] separators = { '|' };

            string temp = segment.Trim('|');
            string[] fields = temp.Split(separators, StringSplitOptions.None);

            foreach (var field in fields)
            {
                Field(count, field);
                if (field == "MSH")
                {
                    // The delimiter after the MSH segment name counts as a field. So consider this as a special case.
                    ++count;
                }
                ++count;
            }
        }

        public string SerializeSegment()
        {
            int max = 0;
            foreach (var field in Fields)
            {
                if (max < field.Key)
                {
                    max = field.Key;
                }
            }

            StringBuilder temp = new StringBuilder();

            for (int index = 0; index <= max; index++)
            {
                if (Fields.ContainsKey(index))
                {
                    temp.Append(Fields[index]);

                    // The delimiter after the MSH segment name counts as a field. So consider this as a special case.
                    if (index == 0 && Name == "MSH")
                    {
                        ++index;
                    }
                }
                if (index != max) temp.Append("|");
            }

            return temp.ToString();
        }
    }
}
