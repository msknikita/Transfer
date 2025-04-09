namespace TransferApp;

public class Account(string name, decimal initialAmount = 0)
{
    private decimal _amount = initialAmount;
    private readonly SemaphoreSlim _lock = new(1, 1);
    public string Name { get; } = name;

    public decimal Balance => _amount;

    public async Task LockAsync() => await _lock.WaitAsync();
    public void Unlock() => _lock.Release();

    public bool CanWithdraw(decimal amount) => _amount >= amount;
    public void Withdraw(decimal amount) => _amount -= amount;
    public void Deposit(decimal amount) => _amount += amount;
}
