using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mine.Models;

namespace Mine.Services
{
    public class MockDataStore : IDataStore<ItemModel>
    {
        readonly List<ItemModel> items;

        public MockDataStore()
        {
            items = new List<ItemModel>()
            {
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Tabby Cat", Description="Gives you nine lives.", Value=9},
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Obama", Description="Great public speaking skills.", Value=5 },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Baked Beans", Description="On toast.", Value=3 },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Fun Mushrooms", Description="Make opponents hallucinate.", Value=7 },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Spotted Toad", Description="Is he a prince?", Value=4 }
          
            };
        }

        public async Task<bool> CreateAsync(ItemModel item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(ItemModel item)
        {
            var oldItem = items.Where((ItemModel arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((ItemModel arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<ItemModel> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<ItemModel>> ReadAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}