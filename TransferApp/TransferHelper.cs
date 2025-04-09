namespace TransferApp;

public class TransferHelper
{
    public async Task<bool> TransferAsync(Account from, Account to, decimal amount)
    {
        var (first, second) = string.CompareOrdinal(from.Name, to.Name) < 0
            ? (from, to)
            : (to, from);

        await first.LockAsync();
        await second.LockAsync();

        try
        {
            if (!from.CanWithdraw(amount))
                return false;

            from.Withdraw(amount);
            to.Deposit(amount);
            return true;
        }
        finally
        {
            second.Unlock();
            first.Unlock();
        }
    }
}
