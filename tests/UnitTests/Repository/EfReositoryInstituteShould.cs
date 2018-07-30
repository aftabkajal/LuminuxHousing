using AppCore.Entities;
using AppInfrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Repository
{
   public class EfReositoryInstituteShould
    {
        private LuminuxContext _dbContext;

        private static DbContextOptions<LuminuxContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<LuminuxContext>();
            builder.UseInMemoryDatabase("Luminux")
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
        /// <summary>
        /// This method returns a fresh repository every time it is called
        /// </summary>
        /// <returns></returns>
        private EfRepository<Plots> GetRepository()
        {
            var options = CreateNewContextOptions();
            _dbContext = new LuminuxContext(options);
            return new EfRepository<Plots>(_dbContext);
        }

        [Fact]
        public void AddPlotAndSetId()
        {
            var repository = GetRepository();
            var plots = new Plots();
            repository.Add(plots);

            var newPlots = repository.ListAll().FirstOrDefault();

            Assert.Equal(plots, newPlots);
            Assert.NotEqual(Guid.Empty, newPlots.Id);
        }

        [Fact]
        public void GetItemUsingId()
        {
            var repository = GetRepository();
            var initialName = Guid.NewGuid().ToString();
            var plots = new Plots()
            {
                Name = initialName
            };

            repository.Add(plots);
            var initialId = plots.Id;
            Assert.NotEqual(Guid.Empty, initialId);

            // detach
            _dbContext.Entry(plots).State = EntityState.Detached;

            var newPlot = repository.GetById(initialId);
            Assert.NotNull(newPlot);
            Assert.NotSame(plots, newPlot);
            Assert.Equal(initialName, newPlot.Name);
        }

        [Fact]
        public async Task GetItemUsingIdAsync()
        {
            var repository = GetRepository();
            var initialName = Guid.NewGuid().ToString();
            var plots = new Plots()
            {
                Name = initialName
            };

            await repository.AddAsync(plots);
            var initialId = plots.Id;
            Assert.NotEqual(Guid.Empty, initialId);

            // detach
            _dbContext.Entry(plots).State = EntityState.Detached;

            var newPlots = await repository.GetByIdAsync(initialId);
            Assert.NotNull(newPlots);
            Assert.NotSame(plots, newPlots);
            Assert.Equal(initialName, newPlots.Name);
        }

        [Fact]
        public async Task AddPlotAndSetIdAsync()
        {
            var repository = GetRepository();
            var plots = new Plots();
            await repository.AddAsync(plots);

            var newPlots = (await repository.ListAllAsync()).FirstOrDefault();

            Assert.Equal(plots, newPlots);
            Assert.NotEqual(Guid.Empty, newPlots.Id);
        }

        [Fact]
        public void UpdatePlotAfterAddingIt()
        {
            var repository = GetRepository();
            var initialName = Guid.NewGuid().ToString();
            var initialOwnerName = Guid.NewGuid().ToString();
            var plots = new Plots()
            {
                Name = initialName,
                OwnerName = initialOwnerName
            };

            repository.Add(plots);

            // detach the plots so we get a different instace
            _dbContext.Entry(plots).State = EntityState.Detached;

            var newPlots = repository.ListAll().FirstOrDefault();
            Assert.NotSame(plots, newPlots);

            // We can use AutoFixure to generate random string
            // need to check and implement if relevant 
            var newName = Guid.NewGuid().ToString();
            var newOwnerName = Guid.NewGuid().ToString();

            newPlots.Name = newName;
            newPlots.OwnerName = newOwnerName;

            repository.Update(newPlots);


            // Detach the newPlots so we get a different instace
            _dbContext.Entry(newPlots).State = EntityState.Detached;

            var updatedPlots = repository.ListAll().FirstOrDefault();
            Assert.NotSame(newPlots, updatedPlots);
            Assert.NotEqual(newPlots.Name, updatedPlots.Name);
            Assert.NotEqual(newPlots.OwnerName, updatedPlots.OwnerName);
        }

        [Fact]
        public async Task UpdatePlotsAfterAddingItAsync()
        {
            var repository = GetRepository();
            var initialName = Guid.NewGuid().ToString();
            var initialOwnerName = Guid.NewGuid().ToString();
            var plots = new Plots()
            {
                Name = initialName,
                OwnerName = initialOwnerName
            };

            await repository.AddAsync(plots);

            // detach the plots so we get a different instace
            _dbContext.Entry(plots).State = EntityState.Detached;

            var newPlots = (await repository.ListAllAsync()).FirstOrDefault();
            Assert.NotSame(plots, newPlots);

            // We can use AutoFixure to generate random string
            // need to check and implement if relevant 
            var newName = Guid.NewGuid().ToString();
            var newOwnerName = Guid.NewGuid().ToString();

            newPlots.Name = newName;
            newPlots.OwnerName = newOwnerName;

            await repository.UpdateAsync(newPlots);


            // Detach the newPlots so we get a different instace
            _dbContext.Entry(newPlots).State = EntityState.Detached;

            var updatedPlots = (await repository.ListAllAsync()).FirstOrDefault();
            Assert.NotSame(newPlots, updatedPlots);
            Assert.NotEqual(newPlots.Name, updatedPlots.Name);
            Assert.NotEqual(newPlots.OwnerName, updatedPlots.OwnerName);
        }

        [Fact]
        public void DeletePlotsAfterAddingIt()
        {
            var repository = GetRepository();
            var initialName = Guid.NewGuid().ToString();
            var plots = new Plots()
            {
                Name = initialName
            };
            repository.Add(plots);

            repository.Delete(plots);

            Assert.DoesNotContain(repository.ListAll(), i => i.Name == initialName);

        }

        [Fact]
        public async Task DeletePlotsAfterAddingItAsyc()
        {
            var repository = GetRepository();
            var initialName = Guid.NewGuid().ToString();
            var plots = new Plots()
            {
                Name = initialName
            };
            await repository.AddAsync(plots);

            await repository.DeleteAsync(plots);

            Assert.DoesNotContain(await repository.ListAllAsync(), i => i.Name == initialName);

        }
    }
}
