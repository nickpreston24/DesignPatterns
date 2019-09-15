namespace ExampleMocking.TestClasses
{
    public class Account
    {
        private decimal _balance;
        private string _currency;

        public string Currency { get => _currency; internal set => _currency = value; }
        public decimal Balance { get => _balance; internal set => _balance = value; }

        public Account(int balance, string currency)
        {
            _balance = balance;
            _currency = currency;
        }

        public void Withdraw(decimal amount)
        {
            _balance -= amount;
        }

        internal void Deposit(decimal convertedAmount)
        {
            _balance += convertedAmount;
        }
    }
}