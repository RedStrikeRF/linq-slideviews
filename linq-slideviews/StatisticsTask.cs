using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class StatisticsTask
{
    public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
    {
        return visits.OrderBy(x => x.DateTime)
                .GroupBy(x => x.UserId)
                .SelectMany(group =>
                {
                    var result = group.Bigrams();
                    return result.Where(i => (i.Item1.SlideType == slideType));
                })
                .Select(x => x.Item2.DateTime.Subtract(x.Item1.DateTime).TotalMinutes)
                .Where(i => (i >= 1 && i <= 120))
                .DefaultIfEmpty(0)
                .Median();
    }
}