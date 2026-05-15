using OldenEraTemplateEditor.Common;
using OldenEraTemplateEditor.Models;

namespace OldenEraTemplateEditor.Views.Dialog
{
    public enum MandatoryContentDialogMode { Group, Item }

    public class MandatoryContentDialog : Form
    {
        private TextBox groupNameTextBox;
        private RadioButton sidRadio;
        private RadioButton includeListsRadio;
        private ComboBox sidComboBox;
        private CheckedListBox includeListsCheckedListBox;
        private CheckBox isGuardedCheckBox;
        private Button okBtn;
        private Button cancelBtn;

        public MandatoryContentDto dto;

        public MandatoryContentDialog(MandatoryContentDialogMode mode)
        {
            this.dto = new MandatoryContentDto();
            dto.mode = mode;
            Text = dto.mode == MandatoryContentDialogMode.Group ? "Add Group" : "Add Item";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            if (dto.mode == MandatoryContentDialogMode.Group)
            {
                Width = 400;
                Height = 160;
                InitGroupControls();
            }
            else
            {
                Width = 400;
                Height = 420;
                InitItemControls();
            }

            okBtn = new Button { Text = "OK", Width = 80, Left = 190, Top = ClientSize.Height - 45 };
            cancelBtn = new Button { Text = "Cancel", Width = 80, Left = 280, Top = ClientSize.Height - 45, DialogResult = DialogResult.Cancel };

            okBtn.Click += (s, e) =>
            {
                if (dto.mode == MandatoryContentDialogMode.Item)
                {
                    if (sidRadio.Checked)
                    {
                        dto.Sid = sidComboBox.Text;
                        dto.IncludeLists = null;
                    }
                    else
                    {
                        dto.Sid = null;
                        dto.IncludeLists = includeListsCheckedListBox.CheckedItems.Cast<string>().ToList();
                    }
                    dto.IsGuarded = isGuardedCheckBox.Checked;
                }
                else
                {
                    dto.GroupName = groupNameTextBox.Text;
                }
                DialogResult = DialogResult.OK;
                Close();
            };

            Controls.Add(okBtn);
            Controls.Add(cancelBtn);
            AcceptButton = okBtn;
            CancelButton = cancelBtn;
        }

        private void InitGroupControls()
        {
            var groupNameLabel = new Label { Text = "Group Name:", Left = 20, Top = 20, AutoSize = true };
            groupNameTextBox = new TextBox { Left = 120, Top = 17, Width = 240, Text = "new_group" };
            Controls.Add(groupNameLabel);
            Controls.Add(groupNameTextBox);
        }

        private void InitItemControls()
        {
            int top = 20;

            // RadioButton
            sidRadio = new RadioButton { Text = "SID", Left = 20, Top = top, Checked = true, AutoSize = true };
            includeListsRadio = new RadioButton { Text = "IncludeLists", Left = 100, Top = top, AutoSize = true };
            Controls.Add(sidRadio);
            Controls.Add(includeListsRadio);

            // SID ComboBox
            sidComboBox = new ComboBox
            {
                Left = 20,
                Top = top + 30,
                Width = 340,
                DropDownStyle = ComboBoxStyle.DropDown,
            };
            sidComboBox.Items.AddRange(Constant.Sids);
            Controls.Add(sidComboBox);

            // IncludeLists
            var ilLabel = new Label { Text = "IncludeLists:", Left = 20, Top = top + 60, AutoSize = true };
            Controls.Add(ilLabel);

            includeListsCheckedListBox = new CheckedListBox
            {
                Left = 20,
                Top = top + 80,
                Width = 340,
                Height = 200,
                CheckOnClick = true,
            };
            includeListsCheckedListBox.Items.AddRange(Constant.ContentLists);
            Controls.Add(includeListsCheckedListBox);

            // isGuarded
            isGuardedCheckBox = new CheckBox { Text = "isGuarded", Left = 20, Top = top + 290, AutoSize = true };
            Controls.Add(isGuardedCheckBox);

            UpdateItemEnabledState();
            sidRadio.CheckedChanged += (s, e) => UpdateItemEnabledState();
            includeListsRadio.CheckedChanged += (s, e) => UpdateItemEnabledState();
        }

        private void UpdateItemEnabledState()
        {
            bool sidMode = sidRadio.Checked;
            sidComboBox.Enabled = sidMode;
            includeListsCheckedListBox.Enabled = !sidMode;
        }
    }

    public class MandatoryContentDto
    {
        public MandatoryContentDialogMode mode;
        public string GroupName;
        public string? Sid;
        public List<string>? IncludeLists;
        public bool IsGuarded;
    }
}
