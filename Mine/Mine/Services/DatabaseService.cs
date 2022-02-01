using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using Mine.Models;

namespace Mine.Services
{
    public class DatabaseService: IDataStore<ItemModel>
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public DatabaseService()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if(!Database.TableMappings.Any(m => m.MappedType.Name == typeof(ItemModel).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(ItemModel)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }
        /// <summary>
        /// Insert data in db
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(ItemModel item)
        {
            if(item == null)
            {
                return false;
            }

            var result = await Database.InsertAsync(item);
            if(result == 0)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Call to db for update item info
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(ItemModel item)
        {
            if(item == null)
            {
                return false;
            }

            var result = await Database.UpdateAsync(item);
            if(result == 0)
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Delete call to db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string id)
        {
            var data = await ReadAsync(id);

            if (data == null)
            {
                return false;
            }

            var result = await Database.DeleteAsync(data);
            if (result == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Read id from db to get item info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ItemModel> ReadAsync(string id)
        {
            if(id == null)
            {
                return null;
            }

            //call the db to read the id
            //use linq syntax to find the first recod that has the id that matches
            var result = Database.Table<ItemModel>().FirstOrDefaultAsync(m => m.Id.Equals(id));
            return result;
        }

        /// <summary>
        /// Calling to database for index page
        /// </summary>
        /// <param name="forceRefresh"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ItemModel>> IndexAsync(bool forceRefresh = false)
        {
            var result = await Database.Table<ItemModel>().ToListAsync();
            return result;
        }
    }
}
