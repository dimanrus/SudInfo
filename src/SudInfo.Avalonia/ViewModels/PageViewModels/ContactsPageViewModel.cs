namespace SudInfo.Avalonia.ViewModels.PageViewModels;

public partial class ContactsPageViewModel : BaseRoutableViewModel
{
    #region Private Variables

    private readonly EventHandler _eventHandlerClosedWindowDialog;

    #endregion

    #region Initialization

    public ContactsPageViewModel(
        ContactService contactService,
        NavigationService navigationService) {
        #region Services Initialization

        _contactService = contactService;

        _navigationService = navigationService;

        #endregion

        _eventHandlerClosedWindowDialog = async (s, e) => await LoadContacts();
    }

    #endregion

    #region Properties

    [Reactive]
    public partial Contact? SelectedContact { get; set; }

    #endregion

    #region Collections

    [Reactive]
    public partial IReadOnlyCollection<Contact>? Contacts { get; set; }

    #endregion

    #region Services

    private readonly ContactService _contactService;

    private readonly NavigationService _navigationService;

    #endregion

    #region Public Methods

    public async Task CreateExcelTable() {
        if (Contacts == null || Contacts.Count == 0)
            return;
        await ExcelService.CreateExcelTableFromEntity(Contacts);
    }

    public async Task LoadContacts() {
        Contacts = await _contactService.Get();
    }

    public async Task RemoveContact() {
        if (SelectedContact == null)
            return;
        var dialogResult = await DialogService.ShowQuestionMessageBox("Вы действительно хотите удалить контакт?");
        if (dialogResult == ButtonResult.No)
            return;
        var removeContactResult = await _contactService.Remove(SelectedContact.Id);
        if (!removeContactResult.Success) {
            await DialogService.ShowErrorMessageBox(removeContactResult.Message);
            return;
        }
        await LoadContacts();
    }

    public async Task OpenEditContactWindow() {
        if (SelectedContact == null)
            return;
        await _navigationService.ShowContactWindowDialog(WindowType.Edit, _eventHandlerClosedWindowDialog, SelectedContact.Id);
    }

    public async Task OpenAddContactWindow() {
        await _navigationService.ShowContactWindowDialog(WindowType.Add, _eventHandlerClosedWindowDialog);
    }

    #endregion
}