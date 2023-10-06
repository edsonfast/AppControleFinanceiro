using AppControleFinanceiro.Models;
using AppControleFinanceiro.Repo;
using CommunityToolkit.Mvvm.Messaging;

namespace AppControleFinanceiro.Views;

public partial class TransactionList : ContentPage {
    //private TransactionAdd _transactionAdd;
    //private TransactionEdit _transactionEdit;
    private ITransactionRepo _repo;

    // Assim a instância é criado uma vez só pq fica na tela principal TransactionList mantendo os dados
    //public TransactionList(TransactionAdd transactionAdd, TransactionEdit transactionEdit, ITransactionRepo repo)
    public TransactionList(ITransactionRepo repo) {
        //this._transactionAdd = transactionAdd;
        //this._transactionEdit = transactionEdit;
        this._repo = repo;
        InitializeComponent();

        Reload();

        // Só roda quando é notificado
        WeakReferenceMessenger.Default.Register<string>(this, (e, msg) => {
            Reload();
        });
    }

    private void Reload() {
        var lista = _repo.GetAll();
        CollectionViewTransaction.ItemsSource = lista;

        double income = lista.Where(x => x.Type == Models.TransactionType.Income).Sum(x => x.Value);
        double expense = lista.Where(x => x.Type == Models.TransactionType.Expenses).Sum(x => x.Value);
        double balance = income - expense;

        LabelIncome.Text = income.ToString("C");
        LabelExpense.Text = expense.ToString("C");
        LabelBalance.Text = balance.ToString("C");

    }

    private void OnButtonClickedToTransactionAdd(object sender, EventArgs e) {

        // Publisher - Subscribers
        // TransactionAdd Publisher > Cadastro
        // Subsceribers -> TransactionList

        //App.Current.MainPage = new TransactionAdd();
        //Navigation.PushModalAsync(new TransactionAdd());

        // Assim usa a mesma mantêndo os dados
        // Navigation.PushModalAsync(_transactionAdd);

        // Assim cria uma nova toda vez que clicar
        var transactionAdd = Handler.MauiContext.Services.GetService<TransactionAdd>();
        Navigation.PushModalAsync(transactionAdd);
    }

    private void TapGestureRecognizerTappedToEdit(object sender, TappedEventArgs e) {

        var grid = (Grid)sender;
        var gesture = (TapGestureRecognizer)grid.GestureRecognizers[0];
        Transaction transaction = (Transaction)gesture.CommandParameter;

        //App.Current.MainPage = new TransactionEdit();
        //Navigation.PushModalAsync(new TransactionEdit());
        //Navigation.PushModalAsync(_transactionEdit);
        var transactionEdit = Handler.MauiContext.Services.GetService<TransactionEdit>();
        transactionEdit.SetTransactionToEdit(transaction);
        Navigation.PushModalAsync(transactionEdit);
    }

    private async void TapGestureRecognizerTappedToDelete(object sender, TappedEventArgs e) {
        await AnimationBorder((Border)sender, true);
        bool result = await App.Current.MainPage.DisplayAlert("Excluir!", "Tem certeza de que deseja excluir?", "Sim", "Não");

        if (result) {
            Transaction transaction = (Transaction)e.Parameter;
            _repo.Delete(transaction);
            Reload();
        } else {
            await AnimationBorder((Border)sender, false);
        }
    }

    private Color _borderOriginalBackgroundColor;
    private string _labelOriginalText;
    private async Task AnimationBorder(Border border, bool IsDeleteAnimation) {
        
        var label = (Label)border.Content;

        if (IsDeleteAnimation) {
            _borderOriginalBackgroundColor = border.BackgroundColor;
            _labelOriginalText = label.Text;

            await border.RotateYTo(90, 200);
            
            border.BackgroundColor = Colors.Red;
            label.TextColor = Colors.White;
            label.Text = "X";

            await border.RotateYTo(180, 200);
        } else {
            await border.RotateYTo(90, 200);

            border.BackgroundColor =_borderOriginalBackgroundColor;
            label.TextColor = Colors.Black;
            label.Text = _labelOriginalText;

            await border.RotateYTo(0, 200);

        }
    }
}