using System;

[Serializable]
public struct Date {
    public string Day;
    public string Month;
    public string Year;

    public static int Age(Date date) {
        var currentDate = DateTime.Now;
        var (day, month, year) = (int.Parse(date.Day), int.Parse(date.Month), int.Parse(date.Year));

        var baseAge = currentDate.Year - year;
        if (month > currentDate.Month) baseAge--;
        else if (month == currentDate.Month && day > currentDate.Day) baseAge--;

        return baseAge;
    }
}