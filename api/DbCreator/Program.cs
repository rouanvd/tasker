using DbCreator.Infra;

namespace DbCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new CreateDbOptions()
            {
                DropDb       = true,
                LoadSeedData = true,
                LoadTestData = true
            };

            var dbCreator = new Infra.DbCreator();
            dbCreator.CreateDb(options);
        }
    }
}
