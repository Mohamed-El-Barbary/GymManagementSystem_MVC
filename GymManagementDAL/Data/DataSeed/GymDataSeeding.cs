using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DataSeed
{
    public class GymDataSeeding
    {

        public static bool SeedData(GymDbContext dbContext)
        {

            try
            {
                var hasPlans = dbContext.Plans.Any();
                var hasCategories = dbContext.Categories.Any();

                if (hasPlans && hasCategories) return false;

                if (!hasPlans)
                {
                    var plans = LoadDataFromJsonFile<Plan>("plans.json");
                    if (plans.Any())
                        dbContext.AddRange(plans);
                }

                if (!hasCategories)
                {
                    var categories = LoadDataFromJsonFile<Category>("categories.json");
                    if (categories.Any())
                        dbContext.AddRange(categories);
                }

                return dbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed Seed {ex}");
                return false;
            }

        }

        private static List<T> LoadDataFromJsonFile<T>(string fileName)
        {

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", fileName);
            if (!File.Exists(filePath)) throw new FileNotFoundException();

            string data = File.ReadAllText(filePath);

            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<List<T>>(data, options) ?? new List<T>();

        }

    }
}
