
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Transactions;


class HotelCapacity
{
    static bool CheckCapacity(int maxCapacity, List<Guest> guests)//сколько можно разместить и список информация о гостях
    {
        var currentCapacity = new List<(DateTime date, int currentCapacity)>();//актуальные данные на сегодняшний день
        foreach (var guest in guests)
        {
            DateTime CheckIn = DateTime.Parse(guest.CheckIn);
            DateTime CheckOut = DateTime.Parse(guest.CheckOut);

            currentCapacity.Add((CheckIn, +1));
            currentCapacity.Add((CheckOut, -1));
        }

        currentCapacity.Sort((firstDate,secondDate) =>
        {
            int cmp = firstDate.date.CompareTo(secondDate.date);
            if (cmp != 0) return cmp;
            return secondDate.currentCapacity.CompareTo(secondDate.currentCapacity);
        });

        int currentQuantity = 0;

        foreach (var eventItem in currentCapacity){
            DateTime date = eventItem.date; 
            int changing = eventItem.currentCapacity;
            currentQuantity += changing;
            if (currentQuantity > maxCapacity) return false;
        }
        return true; 
    }



    class Guest
    {
        public string Name { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
    }


    static void Main()
    {
        int maxCapacity = int.Parse(Console.ReadLine());
        int n = int.Parse(Console.ReadLine());

        List<Guest> guests = new List<Guest>();

        for (int i = 0; i < n; i++)
        {
            string line = Console.ReadLine();
            Guest guest = ParseGuest(line);
            guests.Add(guest);
        }

        bool result = CheckCapacity(maxCapacity, guests);


        Console.WriteLine(result ? "True" : "False");
    }

    static Guest ParseGuest(string json)
    {
        var guest = new Guest();

        Match nameMatch = Regex.Match(json, "\"name\"\\s*:\\s*\"([^\"]+)\"");
        if (nameMatch.Success)
            guest.Name = nameMatch.Groups[1].Value;

        Match checkInMatch = Regex.Match(json, "\"check-in\"\\s*:\\s*\"([^\"]+)\"");
        if (checkInMatch.Success)
            guest.CheckIn = checkInMatch.Groups[1].Value;

        Match checkOutMatch = Regex.Match(json, "\"check-out\"\\s*:\\s*\"([^\"]+)\"");
        if (checkOutMatch.Success)
            guest.CheckOut = checkOutMatch.Groups[1].Value;


        return guest;
    }
}