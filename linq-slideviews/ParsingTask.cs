using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class ParsingTask
{
	/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
	/// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
	/// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
	public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
	{
        return lines.Aggregate(new Dictionary<int, SlideRecord>(), (current, item) =>
        {
            var parts = item.Split(';');

            if (parts.Length != 3) return current;

            if (!int.TryParse(parts[0], out var id) || !Enum.TryParse(parts[1], true, out SlideType type))
                return current;

            current.Add(id, new SlideRecord(id, type, parts[2]));
            return current;
        });
    }

	/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
	/// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
	/// Такой словарь можно получить методом ParseSlideRecords</param>
	/// <returns>Список информации о посещениях</returns>
	/// <exception cref="FormatException">Если среди строк есть некорректные</exception>
	public static IEnumerable<VisitRecord> ParseVisitRecords(
		IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
	{
        return lines.Aggregate(new List<VisitRecord>(), (current, item) =>
        {
            if (item.Equals("UserId;SlideId;Date;Time")) return current;

            var parts = item.Split(';');

            if (parts.Length != 4 
            || (!int.TryParse(parts[0], out var userId)
            || !int.TryParse(parts[1], out var slideId)
            || !DateTime.TryParse($"{parts[2]} {parts[3]}", out var dateTime))
            || (!slides.TryGetValue(slideId, out var slidesValue)))
            {
                throw new FormatException($"Wrong line [{item}]");
            }

            current.Add(new VisitRecord(userId, slideId, dateTime, slidesValue.SlideType));
            return current;
        });
    }
}