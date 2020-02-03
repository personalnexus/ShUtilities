using ShUtilities.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShUtilities.Time
{
    public class DateOffsets
    {
        private static readonly Dictionary<char, DateOffsetKind> DateOffsetKindsByShortName = Enum.GetValues(typeof(DateOffsetKind)).Cast<DateOffsetKind>().ToDictionary(x => Enum.GetName(typeof(DateOffsetKind), x)[0]);

        private readonly List<(int, DateOffsetKind)> _items = new List<(int, DateOffsetKind)>();

        public IBusinessCalendar Calendar { get; set; }
        public IReadOnlyList<(int, DateOffsetKind)> Items => _items;

        public DateOffsets(IBusinessCalendar calendar): this()
        {
            Calendar = calendar;
        }

        public DateOffsets()
        {
        }

        public void Add(DateOffsetKind kind, int count) => _items.Add((count, kind));

        public void AddDays(int count) => Add(DateOffsetKind.Days, count);

        public void AddWeekdays(int count) => Add(DateOffsetKind.Weekdays, count);

        public void AddBusinessDays(int count) => Add(DateOffsetKind.BusinessDays, count);

        public DateTime ApplyTo(DateTime date)
        {
            foreach ((int count, DateOffsetKind kind) in _items)
            {
                date = kind switch
                {
                    DateOffsetKind.Days => date.AddDays(count),
                    DateOffsetKind.Weekdays => date.AddWeekdays(count),
                    DateOffsetKind.BusinessDays => date.AddBusinessDays(count, Calendar)
                };
            }
            return date;
        }

        public static DateOffsets Parse(string input)
        {
            if (!TryParse(input, out DateOffsets result))
            {
                throw new ArgumentException($"{input} is not a valid DateOffsets string representation.");
            }
            return result;
        }

        public static bool TryParse(string input, out DateOffsets offsets) => TryParse(input, null, out offsets);

        public static bool TryParse(string input, IBusinessCalendar calendar, out DateOffsets offsets)
        {
            offsets = new DateOffsets(calendar);
            foreach (string part in input.Split(';'))
            {
                if (part.Length < 2)
                {
                    offsets = null;
                    break;
                }

                int count;
                string countString = part[0..^1];
                if (countString == "+")
                {
                    count = +1;
                }
                else if (countString == "-")
                {
                    count = -1;
                }
                else if (!int.TryParse(countString, out count))
                {
                    offsets = null;
                    break;
                }

                if (!DateOffsetKindsByShortName.TryGetValue(part[^1], out DateOffsetKind kind))
                {
                    offsets = null;
                    break;
                }

                offsets.Add(kind, count);
            }
            return offsets != null;
        }

        public override string ToString() => _items.Select(x => $"{x.Item1}{x.Item2.ToString()[0]}").ToDelimitedString(";");
    }
}
