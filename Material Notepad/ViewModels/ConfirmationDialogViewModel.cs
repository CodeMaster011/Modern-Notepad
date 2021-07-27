namespace Material_Notepad.ViewModels
{
    public class ConfirmationDialogViewModel : BindableBase
    {
        public string Message { get; set; }
        public string Title { get; set; }
        public bool? IsConfirmed { get; set; }
    }
}
