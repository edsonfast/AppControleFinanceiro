using AppControleFinanceiro.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppControleFinanceiro.Repo {
    public class TransactionRepo : ITransactionRepo {
        private readonly LiteDatabase _database;
        private readonly string collectionName = "transactions";

        public TransactionRepo(LiteDatabase database) {
            _database = database;

        }
        public List<Transaction> GetAll() {
            return _database
                .GetCollection<Transaction>(collectionName)
                .Query()
                .OrderByDescending(x => x.Date)
                .ToList();
        }
        public void Add(Transaction transaction) {
            var coll = _database.GetCollection<Transaction>(collectionName);

            coll.Insert(transaction);
            coll.EnsureIndex(x => x.Date);
        }

        public void Update(Transaction transaction) {
            var coll = _database.GetCollection<Transaction>(collectionName);

            coll.Update(transaction);
        }

        public void Delete(Transaction transaction) {
            var coll = _database.GetCollection<Transaction>(collectionName);

            coll.Delete(transaction.Id);
        }

    }
}
