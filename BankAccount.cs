using System;
using System.Collections.Generic;

namespace PracticeA
{
    public class Transaction
    {
        public int FromAccount { get; set; }
        public int ToAccount { get; set; }
        public double Amount { get; set; }
    }

    public class BankAccount
    {
        public int AccountNumber { get; set; }
        public double Balance { get; private set; }
        public string AccountHolder { get; set; }

        public BankAccount(int accountNumber, string accountHolder)
        {
            AccountNumber = accountNumber;
            AccountHolder = accountHolder;
            Balance = 0;
        }

        public List<Transaction> TransactionHistory { get; private set; } = new List<Transaction>();

        public void RecordTransaction(int fromAccount, int toAccount, double amount)
        {
            TransactionHistory.Add(new Transaction { FromAccount = fromAccount, ToAccount = toAccount, Amount = amount });
        }

        public Transaction GetLastTransaction()
        {
            if (TransactionHistory.Count > 0)
            {
                return TransactionHistory[TransactionHistory.Count - 1];
            }
            return null;
        }

        public void RemoveLastTransaction()
        {
            if (TransactionHistory.Count > 0)
            {
                TransactionHistory.RemoveAt(TransactionHistory.Count - 1);
            }
        }

        public void Deposit(double amount)
        {
            if (amount > 0)
            {
                Balance += amount;
            }
        }

        public bool Withdraw(double amount)
        {
            if (amount > 0 && amount <= Balance)
            {
                Balance -= amount;
                return true;
            }
            return false;
        }
    }

    public class Bank
    {
        private Dictionary<int, BankAccount> accounts = new Dictionary<int, BankAccount>();
        private int nextAccountNumber = 1000;

        // 1
        public BankAccount GetAccount(int accountNumber)
        {
            if (accounts.ContainsKey(accountNumber))
            {
                return accounts[accountNumber];
            }
            return null;
        }

        // 2
        public bool Transfer(int fromAccountNumber, int toAccountNumber, double amount)
        {
            var fromAccount = GetAccount(fromAccountNumber);
            var toAccount = GetAccount(toAccountNumber);

            if (fromAccount != null && toAccount != null && fromAccount.Withdraw(amount))
            {
                toAccount.Deposit(amount);
                fromAccount.RecordTransaction(fromAccountNumber, toAccountNumber, amount);
                toAccount.RecordTransaction(fromAccountNumber, toAccountNumber, amount);
                return true;
            }
            return false;
        }

        // 3
        public bool CancelLastTransaction(int accountNumber)
        {
            var account = GetAccount(accountNumber);
            var lastTransaction = account?.GetLastTransaction();

            if (lastTransaction != null)
            {
                var toAccount = GetAccount(lastTransaction.ToAccount);
                if (toAccount != null && toAccount.Withdraw(lastTransaction.Amount))
                {
                    account.Deposit(lastTransaction.Amount);
                    account.RemoveLastTransaction();
                    toAccount.RemoveLastTransaction();
                    return true; 
                }
            }
            return false; 
        }

        // 4
        public double CheckBalance(int accountNumber)
        {
            var account = GetAccount(accountNumber);
            return account != null ? account.Balance : 0;
        }

        // 5
        public void PrintStatement(int accountNumber)
        {
            var account = GetAccount(accountNumber);
            if (account != null)
            {
                Console.WriteLine($"Account Number: {account.AccountNumber}, Balance: {account.Balance}, Account Holder: {account.AccountHolder}");
            }
            else
            {
                Console.WriteLine("Account not found.");
            }
        }

        // 6
        public BankAccount OpenAccount(string accountHolder)
        {
            var account = new BankAccount(nextAccountNumber++, accountHolder);
            accounts.Add(account.AccountNumber, account);
            return account; 
        }

        // 7
        public bool CloseAccount(int accountNumber)
        {
            return accounts.Remove(accountNumber);
        }

        // 8
        public bool RequestLoan(int accountNumber, double loanAmount)
        {
            var account = GetAccount(accountNumber);
            if (account != null && loanAmount > 0)
            {
                account.Deposit(loanAmount); 
                return true;
            }
            return false;
        }

        // 9
        public void PayLoan(int accountNumber, double amount)
        {
            var account = GetAccount(accountNumber);
            if (account != null && amount > 0)
            {
                account.Withdraw(amount); 
            }
        }

        // 10
        public void UpdateAccountHolderInfo(int accountNumber, string newName)
        {
            var account = GetAccount(accountNumber);
            if (account != null)
            {
                account.AccountHolder = newName; 
            }
        }
    }
}
