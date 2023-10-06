using AppControleFinanceiro.Models;

namespace AppControleFinanceiro.Repo {
    public interface ITransactionRepo {
        void Add(Transaction transaction);
        void Delete(Transaction transaction);
        List<Transaction> GetAll();
        void Update(Transaction transaction);
    }
}