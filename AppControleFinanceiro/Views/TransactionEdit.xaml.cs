using AppControleFinanceiro.Libraries.Utils.FixBugs;
using AppControleFinanceiro.Models;
using AppControleFinanceiro.Repo;
using CommunityToolkit.Mvvm.Messaging;
using System.Text;

namespace AppControleFinanceiro.Views;

public partial class TransactionEdit : ContentPage
{	
	private Transaction _transaction;
    private ITransactionRepo _repo;
    public TransactionEdit(ITransactionRepo repo)
	{
		InitializeComponent();
        this._repo = repo;
    }

	public void SetTransactionToEdit(Transaction transaction) {
        _transaction = transaction;

		if (_transaction.Type == TransactionType.Income) {
			RadioIncome.IsChecked = true;
			//RadioExpense.IsChecked = false;
		} else {
			//RadioIncome.IsChecked = false;
			RadioExpense.IsChecked = true;
		}

		EntryName.Text = _transaction.Name;
		DatePickerDate.Date = _transaction.Date.Date;
		EntryValue.Text = _transaction.Value.ToString();
    }

    private void TapGestureRecognizerTappedToClose(object sender, TappedEventArgs e) {
        Close();
    }

    private void OnButtonClickedSave(object sender, EventArgs e) {
        if (IsValidData() == false) {
            return;
        }

        Transaction transaction = new Transaction() {
            Id = _transaction.Id,
            Type = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expenses,
            Name = EntryName.Text,
            Date = DatePickerDate.Date,
            Value = Math.Abs(double.Parse(EntryValue.Text))
        };

        // Sem injeção de dependência:
        //var repo = this.Handler.MauiContext.Services.GetService<ITransactionRepo>();
        //repo.Add(transaction);
        // Com injeção:
        _repo.Update(transaction);

        Close();

        // Publisher para list atualizar
        WeakReferenceMessenger.Default.Send<string>(string.Empty);
    }

    private void Close() {
        KeyboardFixBugs.HideKeyboard();
        Navigation.PopModalAsync();
    }

    private bool IsValidData() {
        bool valid = true;
        StringBuilder sb = new StringBuilder();

        if (string.IsNullOrEmpty(EntryName.Text) || string.IsNullOrWhiteSpace(EntryName.Text)) {
            sb.AppendLine("O campo 'Nome' deve ser preenchido!");
            valid = false;
        }

        double result;
        if (string.IsNullOrEmpty(EntryValue.Text) || string.IsNullOrWhiteSpace(EntryValue.Text)) {
            sb.AppendLine("O campo 'Valor' deve ser preenchido!");
            valid = false;
        } else if (!double.TryParse(EntryValue.Text, out result)) {
            sb.AppendLine("O campo 'Valor' é inválido!");
            valid = false;
        }

        if (valid == false) {
            LabelError.Text = sb.ToString();
            LabelError.IsVisible = true;
        } else {
            LabelError.IsVisible = false;
        }

        return valid;
    }
}