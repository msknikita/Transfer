using TransferApp;

public class TransferTests
{
    [Fact]
    public async Task SuccessfulTransfer_ChangesBalancesCorrectly()
    {
        // Arrange
        var a = new Account("Alice", 500);
        var b = new Account("Bob", 300);
        var helper = new TransferHelper();

        // Act
        var result = await helper.TransferAsync(a, b, 200);

        // Assert
        Assert.True(result);
        Assert.Equal(300, a.Balance);
        Assert.Equal(500, b.Balance);
    }

    [Fact]
    public async Task TransferFails_WhenInsufficientFunds()
    {
        // Arrange
        var a = new Account("Alice", 100);
        var b = new Account("Bob", 300);
        var helper = new TransferHelper();

        // Act
        var result = await helper.TransferAsync(a, b, 200);

        // Assert
        Assert.False(result);
        Assert.Equal(100, a.Balance);
        Assert.Equal(300, b.Balance);
    }

    [Fact]
    public async Task ParallelTransfers_DoNotCauseRaceConditions()
    {
        // Arrange
        var a = new Account("Alice", 1000);
        var b = new Account("Bob", 1000);
        var helper = new TransferHelper();

        // Act
        var task1 = helper.TransferAsync(a, b, 200);
        var task2 = helper.TransferAsync(b, a, 300);
        await Task.WhenAll(task1, task2);

        // Assert
        Assert.Equal(1100, a.Balance);
        Assert.Equal(900, b.Balance);
    }
    
    [Fact]
    public async Task ManyParallelTransfers_KeepBalanceConsistent()
    {
        // Arrange
        var a = new Account("A", 10000);
        var b = new Account("B", 10000);
        var helper = new TransferHelper();
        var tasks = new List<Task>();

        // Act
        for (int i = 0; i < 100; i++)
        {
            tasks.Add(Task.Run(() => helper.TransferAsync(a, b, 100)));
            tasks.Add(Task.Run(() => helper.TransferAsync(b, a, 50)));
        }

        await Task.WhenAll(tasks);

        // Assert
        Assert.Equal(20000, a.Balance + b.Balance);
    }
}