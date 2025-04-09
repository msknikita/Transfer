var account1 = new Account("Nikita", 500);
var account2 = new Account("Petya", 300);

var helper = new TransferHelper();

var result = await helper.TransferAsync(account1, account2, 200);

Console.WriteLine($"Transfer result: {(result ? "success" : "failure")}");
Console.WriteLine($"Final balances: {account1.Name}: {account1.Balance}, {account2.Name}: {account2.Balance}");