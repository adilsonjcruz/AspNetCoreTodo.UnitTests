using System;
using System.Threading.Tasks;
using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Xunit;

namespace AspNetCoreTodo.UnitTests
{

    public class TodoItemServiceShould
    {
        [Fact]
        public async Task AddNewItem()
        {

            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDataBase(databaseName: "Test_AddNewItem").Options;

            using (var inMemoryContext = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(inMemoryContext);

                var fakeUser = new ApplicationUser
                {
                    Id = "fake-000",
                    UserName = "fake@fake"
                };

                await service.AddItemAsync(new NewTodoItem { Title = "Testing?" }, fakeUser);

            }

            using (var inMemoryContext = new ApplicationDbContext(options))
            {

                Assert.Equal(1, await inMemoryContext.Items.CountAsync());

                var item = await inMemoryContext.Items.FirstAsync();

                Assert.Equal("Testing?", item.Title);
                Assert.Equal(false, item.isDone);

                Assert.True(DateTimeOffset.Now.AddDays(3) - item.DueAt < TimeSpan.FromSeconds(1));

            }

        }

    }

}