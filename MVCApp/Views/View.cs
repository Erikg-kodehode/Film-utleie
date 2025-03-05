using System;
using System.Collections.Generic;
using MovieAPI.Models;

namespace MVCApp.Views
{
    public class View
    {
        public void DisplayMovies(List<Movie> movies)
        {
            Console.WriteLine("\nLokale Filmer:");
            if (movies.Count == 0)
            {
                Console.WriteLine("Ingen filmer funnet.");
            }
            else
            {
                int index = 1;
                foreach (var movie in movies)
                {
                    Console.WriteLine($"{index}. {movie.Title} ({movie.Year}) - {movie.Genre}");
                    index++;
                }
            }
        }


        public void DisplayOMDbMovies(List<string> movies)
        {
            Console.WriteLine("\nSøkeresultater fra OMDb:");
            if (movies.Count == 0)
            {
                Console.WriteLine("Ingen resultater funnet.");
            }
            else
            {
                foreach (var movie in movies)
                {
                    Console.WriteLine($"- {movie}");
                }
            }
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
