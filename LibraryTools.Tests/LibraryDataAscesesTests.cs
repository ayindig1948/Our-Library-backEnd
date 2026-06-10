using System;
using System.Threading.Tasks;
using LibraryTools;
using LibraryTools.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace LibraryTools.Tests;

public class LibraryDataAscesesTests
{
    private class MockDbExecutor : IDbExecutor
    {
        public Task<IEnumerable<T>> QueryAsync<T>(string storedProcedure, object? param = null)
        {
            // default empty
            return Task.FromResult<IEnumerable<T>>(Array.Empty<T>());
        }

        public Task<IEnumerable<TResult>> QueryAsync<TFirst, TSecond, TResult>(string storedProcedure, Func<TFirst, TSecond, TResult> map, object? param = null)
        {
            return Task.FromResult<IEnumerable<TResult>>(Array.Empty<TResult>());
        }

        public Task ExecuteAsync(string storedProcedure, object? param = null)
        {
            return Task.CompletedTask;
        }
    }

    [Fact]
    public void Constructor_With_MissingConnection_Throws()
    {
        var logger = NullLogger<LibraryDataAsceses>.Instance;
        Assert.Throws<ArgumentNullException>(() => new LibraryDataAsceses(new MockDbExecutor(), logger));
    }

    [Fact]
    public async Task GetAllBooks_Returns_Empty_When_No_Data()
    {
        var logger = NullLogger<LibraryDataAsceses>.Instance;
        var svc = new LibraryDataAsceses(new MockDbExecutor(), logger);
        var res = await svc.GetAllBooks();
        Assert.NotNull(res);
        Assert.Empty(res);
    }

    [Fact]
    public async Task GetAviBooks_Returns_Empty_When_No_Data()
    {
        var logger = NullLogger<LibraryDataAsceses>.Instance;
        var svc = new LibraryDataAsceses(new MockDbExecutor(), logger);
        var res = await svc.GetAviBooks();
        Assert.NotNull(res);
        Assert.Empty(res);
    }

    [Fact]
    public async Task SearchByCategory_Returns_Empty_When_No_Data()
    {
        var logger = NullLogger<LibraryDataAsceses>.Instance;
        var svc = new LibraryDataAsceses(new MockDbExecutor(), logger);
        var res = await svc.SearchByCategory("x");
        Assert.NotNull(res);
        Assert.Empty(res);
    }
}
