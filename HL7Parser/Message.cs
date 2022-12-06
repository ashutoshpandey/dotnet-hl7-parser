using System;
using System.Text;
using System.Collections.Generic;

namespace HL7Parser
{
    public class Message
    {
        private const string MSH = "MSH";
        private const int MSH_MSG_CONTROL_ID = 10;

        public List<Segment> Segments { get; set; }

        public Message()
        {
            Initialize();
        }

        private void Initialize()
        {
            Segments = new List<Segment>();
        }

        protected Segment Header()
        {
            return (Segments.Count == 0 || Segments[0].Name != MSH) ? null : Segments[0];
        }

        public string MessageControlId()
        {
            Segment msh = Header();

            return msh == null ? string.Empty : msh.Field(MSH_MSG_CONTROL_ID);
        }

        public void Add(Segment segment)
        {
            if (!string.IsNullOrEmpty(segment.Name) && segment.Name.Length == 3)
            {
                Segments.Add(segment);

                Console.WriteLine("=========================");
                foreach(var field in segment.Fields)
                {
                    Console.WriteLine(field.Key + ": " + field.Value);
                }
            }
        }

        public void DeSerializeMessage(string msg)
        {
            Initialize();

            char[] separator = { '\r' };
            var tokens = msg.Split(separator, StringSplitOptions.None);

            foreach (var item in tokens)
            {
                var segment = new Segment();
                segment.DeSerializeSegment(item.Trim('\n'));
                Add(segment);
            }
        }

        public string SerializeMessage()
        {
            var builder = new StringBuilder();
            char[] separators = { '\r', '\n' };

            foreach (var segment in Segments)
            {
                builder.Append(segment.SerializeSegment());
                builder.Append("\r\n");
            }
            return builder.ToString().TrimEnd(separators);
        }
    }
}
