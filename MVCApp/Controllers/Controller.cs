using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MVCApp.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MovieAPI.Models;

namespace MVCApp.Controllers
{
    public class Controller
    {
        private View view;
        private readonly HttpClient client = new HttpClient();
        private readonly string localApiBase = "http://localhost:5001/api/movies"; // ✅ Correct port from Docker

        private readonly string omdbApiKey = "20236ae"; // Your OMDb API Key
        private readonly string omdbApiBase = "http://www.omdbapi.com/";

        public Controller(View view)
        {
            this.view = view;
        }

        public async Task Run()
        {
            while (true)
            {
                Console.WriteLine("\nMeny:");
                Console.WriteLine("1. Legg til en lokal film");
                Console.WriteLine("2. Vis alle lokale filmer");
                Console.WriteLine("3. Rediger en lokal film");
                Console.WriteLine("4. Slett en lokal film");
                Console.WriteLine("5. Søk etter filmer fra OMDb");
                Console.WriteLine("6. Avslutt");
                Console.Write("Velg et alternativ: ");

                string choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        await AddMovie();
                        break;
                    case "2":
                        await DisplayLocalMovies();
                        break;
                    case "3":
                        await EditMovie();
                        break;
                    case "4":
                        await DeleteMovie();
                        break;
                    case "5":
                        await SearchOMDbMovies();
                        break;
                    case "6":
                        return;
                    default:
                        view.ShowMessage("Ugyldig valg, prøv igjen.");
                        break;
                }
            }
        }

        private async Task AddMovie()
        {
            Console.Write("Tittel: ");
            string title = Console.ReadLine();

            Console.Write("År: ");
            if (!int.TryParse(Console.ReadLine(), out int year))
            {
                view.ShowMessage("Ugyldig år, prøv igjen.");
                return;
            }

            Console.Write("Sjanger: ");
            string genre = Console.ReadLine();

            var movie = new { Title = title, Year = year, Genre = genre };
            var json = JsonConvert.SerializeObject(movie);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await client.PostAsync(localApiBase, content);
            view.ShowMessage($"Filmen \"{title}\" ({year}, {genre}) er lagt til!");
        }

        private async Task DisplayLocalMovies()
        {
            var response = await client.GetStringAsync(localApiBase);
            var movies = JsonConvert.DeserializeObject<List<Movie>>(response);


            view.DisplayMovies(movies);
        }

        private async Task EditMovie()
        {
            var response = await client.GetStringAsync(localApiBase);
            var movies = JsonConvert.DeserializeObject<List<Movie>>(response);

            if (movies.Count == 0)
            {
                view.ShowMessage("Ingen filmer å redigere.");
                return;
            }

            view.DisplayMovies(movies);
            Console.Write("Skriv inn nummeret på filmen du vil redigere: ");
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > movies.Count)
            {
                view.ShowMessage("Ugyldig valg, prøv igjen.");
                return;
            }

            Console.Write("Skriv inn nytt navn: ");
            string newTitle = Console.ReadLine();

            Console.Write("Skriv inn nytt år: ");
            if (!int.TryParse(Console.ReadLine(), out int newYear))
            {
                view.ShowMessage("Ugyldig år, prøv igjen.");
                return;
            }

            Console.Write("Skriv inn ny sjanger: ");
            string newGenre = Console.ReadLine();

            var updatedMovie = new { Title = newTitle, Year = newYear, Genre = newGenre };
            var json = JsonConvert.SerializeObject(updatedMovie);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await client.PutAsync($"{localApiBase}/{movies[index - 1].Id}", content);
            view.ShowMessage($"Filmen er oppdatert til \"{newTitle}\" ({newYear}, {newGenre})!");
        }

        private async Task DeleteMovie()
        {
            var response = await client.GetStringAsync(localApiBase);
            var movies = JsonConvert.DeserializeObject<List<Movie>>(response);

            if (movies.Count == 0)
            {
                view.ShowMessage("Ingen filmer å slette.");
                return;
            }

            view.DisplayMovies(movies);
            Console.Write("Skriv inn nummeret på filmen du vil slette: ");
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > movies.Count)
            {
                view.ShowMessage("Ugyldig valg, prøv igjen.");
                return;
            }

            await client.DeleteAsync($"{localApiBase}/{movies[index - 1].Id}");
            view.ShowMessage("Filmen er slettet!");
        }

        private async Task SearchOMDbMovies()
        {
            Console.Write("Skriv inn filmnavn for å søke i OMDb: ");
            string query = Console.ReadLine();

            string url = $"{omdbApiBase}?apikey={omdbApiKey}&s={Uri.EscapeDataString(query)}";
            var response = await client.GetStringAsync(url);
            JObject json = JObject.Parse(response);

            List<string> movies = new List<string>();
            if (json["Search"] != null)
            {
                foreach (var movie in json["Search"])
                {
                    movies.Add($"{movie["Title"]} ({movie["Year"]})");
                }
            }

            view.DisplayOMDbMovies(movies);
        }
    }
}
