using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public static class ExtensionsTask
{
	/// <summary>
	/// Медиана списка из нечетного количества элементов — это серединный элемент списка после сортировки.
	/// Медиана списка из четного количества элементов — это среднее арифметическое 
    /// двух серединных элементов списка после сортировки.
	/// </summary>
	/// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
	public static double Median(this IEnumerable<double> items)
	{
        var givenList = items.ToList();

        if (givenList.Count == 0)
            throw new InvalidOperationException();

        givenList.Sort();
        if (givenList.Count % 2 != 0)
            return givenList[givenList.Count / 2];

        var result = (givenList[givenList.Count / 2] + givenList[(givenList.Count / 2) - 1]) / 2;
        return result;
    }

    /// <returns>
    /// Возвращает последовательность, состоящую из пар соседних элементов.
    /// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
    /// </returns>
    public static IEnumerable<(T First, T Second)> Bigrams<T>(this IEnumerable<T> items)
	{   
        T tempFirst = default;
        var isTempTupleEmpty = true;

        foreach (var item in items)
        {
            if (isTempTupleEmpty)
            {
                tempFirst = item;
                isTempTupleEmpty = false;
            }
            else
            {   
                yield return (tempFirst, item);
                tempFirst = item;
            }
        }
    }
}