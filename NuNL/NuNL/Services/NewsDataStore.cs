using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using NuNL.Models;

using Xamarin.Forms;

[assembly: Dependency(typeof(NuNL.Services.NewsDataStore))]
namespace NuNL.Services
{
    public class NewsDataStore : IDataStore<Item>
    {
        bool isInitialized;
        List<Item> items;

        public async Task<bool> AddItemAsync(Item item)
        {
            await InitializeAsync();

            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            await InitializeAsync();

            var _item = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(Item item)
        {
            await InitializeAsync();

            var _item = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            await InitializeAsync();

            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            await InitializeAsync();

            return await Task.FromResult(items);
        }

        public Task<bool> PullLatestAsync()
        {
            return Task.FromResult(true);
        }


        public Task<bool> SyncAsync()
        {
            return Task.FromResult(true);
        }

        public async Task InitializeAsync()
        {
            if (isInitialized)
                return;

            items = new List<Item>();

            string feedUri = "http://www.nu.nl/rss/Algemeen";
            using (var webClient = new HttpClient())
            {
                string result = await webClient.GetStringAsync(feedUri);
                XDocument document = XDocument.Parse(result);
                var temp = (from u in document.Descendants("item")
                            select new Item
                            {
                                Title = u.Element("title")?.Value,
                                Description = u.Element("description")?.Value,
                                Link = new Uri(u.Element("link")?.Value),
                                PubDate = DateTime.Parse(u.Element("pubDate")?.Value),
                                EncloseUrl = new Uri(u.Element("enclosure")?.Attribute("url").Value)
                            }).ToList();
                items.AddRange(temp.OrderByDescending(x => x.PubDate));
            }

            isInitialized = true;
        }
    }
}
