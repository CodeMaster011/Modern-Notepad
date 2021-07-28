using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Material_Notepad.ViewModels
{
    public class FindUserControlViewModel : BindableBase
    {
        private int matchIndex = 0;
        private string matchIndexSearchText;

        public bool IsReplaceVisible { get; set; }
        public string FindText { get; set; }
        public string ReplaceText { get; set; }

        public TextBox DocumentTextBox { get; set; }
        public Control ParentView { get; set; }

        public StandardCommand ShowOrHideReplaceLineCommand { get; set; }
        public RelayCommand FindNextCommand { get; set; }
        public StandardCommand CloseCommand { get; set; }

        public FindUserControlViewModel()
        {
            ShowOrHideReplaceLineCommand = new StandardCommand(_ => IsReplaceVisible = !IsReplaceVisible);
            FindNextCommand = new RelayCommand(findNextCmd, _ => !string.IsNullOrEmpty(DocumentTextBox?.Text));
            CloseCommand = new StandardCommand(_ =>
            {
                if (ParentView != null) ParentView.Visibility = System.Windows.Visibility.Collapsed;
            });
        }

        private void findNextCmd(object obj)
        {
            var match = GetMatch();
            if (match == null) return;
            DocumentTextBox.Select(match.Index, match.Length);
            DocumentTextBox.Focus();
            if(!string.IsNullOrEmpty(DocumentTextBox.Text))
                DocumentTextBox.ScrollToLine(DocumentTextBox.Text.Substring(0, match.Index).Split('\n').Length - 1);
        }

        public Match GetMatch()
        {
            var regex = new Regex(FindText, RegexOptions.IgnoreCase);
            var matches = regex.Matches(DocumentTextBox.Text);
            if (matches?.Count == 0) return null;
            if (matchIndexSearchText == FindText)
            {
                return matches[(matchIndex++) % matches.Count];
            }
            matchIndex = 1;
            matchIndexSearchText = FindText;
            return matches.FirstOrDefault();
        }
    }
}
