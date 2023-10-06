using AppControleFinanceiro.Libraries.Utils.FixBugs;
using AppControleFinanceiro.Models;
using AppControleFinanceiro.Repo;
using CommunityToolkit.Mvvm.Messaging;
using System.Text;

namespace AppControleFinanceiro.Views;

public partial class TransactionAdd : ContentPage {
    private ITransactionRepo _repo;
    public TransactionAdd(ITransactionRepo repo) {
        InitializeComponent();
        this._repo = repo;
    }

    private void TapGestureRecognizerTappedToClose(object sender, TappedEventArgs e) {
        Close();
    }

    private void OnButtonClickedSave(object sender, EventArgs e) {
        if (IsValidData() == false) {
            return;
        }

        Transaction transaction = new Transaction() {
            Type = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expenses,
            Name = EntryName.Text,
            Date = DatePickerDate.Date,
            Value = Math.Abs(double.Parse(EntryValue.Text))
        };

        // Sem injeção de dependência:
        //var repo = this.Handler.MauiContext.Services.GetService<ITransactionRepo>();
        //repo.Add(transaction);
        // Com injeção:
        _repo.Add(transaction);

        Close();

        // Publisher para list atualizar
        WeakReferenceMessenger.Default.Send<string>(string.Empty);

        //var count = _repo.GetAll().Count();
        //App.Current.MainPage.DisplayAlert("Mensagem", $"Existem {count} registros no banco!", "OK");
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