using OldenEraTemplateEditor.Common;
using OldenEraTemplateEditor.Models;

namespace OldenEraTemplateEditor.Views
{
    public class GlobalTabPage : EditorTabPage
    {
        private PropertyGrid propertyGrid;
        private TextBox nameTextBox;
        private ComboBox gameModeComboBox;
        private ComboBox winConditionComboBox;
        private ComboBox sizeComboBox;
        private NumericUpDown heroCountMinUpDown;
        private NumericUpDown heroCountMaxUpDown;
        private NumericUpDown heroCountIncrementUpDown;
        private Button applyBtn;

        public GlobalTabPage(Rmg rmg) : base(rmg)
        {
            Text = "Global";
            InitUI();
            RefreshData();
        }

        public override void RefreshData()
        {
            propertyGrid.SelectedObject = rmg.rmgTemplate;
            LoadFromTemplate();
        }

        private void LoadFromTemplate()
        {
            nameTextBox.Text = rmg.rmgTemplate.Name;
            gameModeComboBox.Text = rmg.rmgTemplate.GameMode ?? "";
            winConditionComboBox.Text = rmg.rmgTemplate.DisplayWinCondition ?? "";

            var size = rmg.rmgTemplate.SizeX;
            if (size > 0 && size == rmg.rmgTemplate.SizeZ)
            {
                var label = KnownValues.MapSizeLabel(size);
                var option = $"{label}({size}×{size})";
                int idx = Array.IndexOf(Constant.MapSizeOptions, option);
                if (idx >= 0)
                    sizeComboBox.SelectedIndex = idx;
                else
                    sizeComboBox.Text = option;
            }
            else
            {
                sizeComboBox.SelectedIndex = Constant.MapSizeOptions.Length - 1; // C
            }

            heroCountMinUpDown.Value = rmg.rmgTemplate.GameRules?.HeroCountMin ?? 1;
            heroCountMaxUpDown.Value = rmg.rmgTemplate.GameRules?.HeroCountMax ?? 1;
            heroCountIncrementUpDown.Value = rmg.rmgTemplate.GameRules?.HeroCountIncrement ?? 1;
        }

        void InitUI()
        {
            var split = new SplitContainer();
            split.Dock = DockStyle.Fill;
            split.SplitterDistance = 70;

            // === Left: Quick Edit Panel ===
            var leftPanel = new Panel { Dock = DockStyle.Fill };
            int y = 15;

            var nameLabel = new Label { Text = "Name:", Left = 20, Top = y, AutoSize = true };
            nameTextBox = new TextBox { Left = 150, Top = y - 3, Width = 130 };
            leftPanel.Controls.Add(nameLabel);
            leftPanel.Controls.Add(nameTextBox);
            y += 30;

            var gameModeLabel = new Label { Text = "GameMode:", Left = 20, Top = y, AutoSize = true };
            gameModeComboBox = new ComboBox { Left = 150, Top = y - 3, Width = 130, DropDownStyle = ComboBoxStyle.DropDownList };
            gameModeComboBox.Items.AddRange(KnownValues.GameModes);
            leftPanel.Controls.Add(gameModeLabel);
            leftPanel.Controls.Add(gameModeComboBox);
            y += 30;

            var winCondLabel = new Label { Text = "WinCondition:", Left = 20, Top = y, AutoSize = true };
            winConditionComboBox = new ComboBox { Left = 150, Top = y - 3, Width = 130, DropDownStyle = ComboBoxStyle.DropDownList };
            winConditionComboBox.Items.AddRange(KnownValues.VictoryConditionLabels);
            leftPanel.Controls.Add(winCondLabel);
            leftPanel.Controls.Add(winConditionComboBox);
            y += 30;

            var sizeLabel = new Label { Text = "Size:", Left = 20, Top = y, AutoSize = true };
            sizeComboBox = new ComboBox { Left = 150, Top = y - 3, Width = 130, DropDownStyle = ComboBoxStyle.DropDownList };
            sizeComboBox.Items.AddRange(Constant.MapSizeOptions);
            leftPanel.Controls.Add(sizeLabel);
            leftPanel.Controls.Add(sizeComboBox);
            y += 40;

            var heroLabel = new Label { Text = "Hero Rules:", Left = 20, Top = y, AutoSize = true, Font = new Font(Control.DefaultFont, FontStyle.Bold) };
            leftPanel.Controls.Add(heroLabel);
            y += 25;

            var minLabel = new Label { Text = "HeroCountMin:", Left = 20, Top = y, AutoSize = true };
            heroCountMinUpDown = new NumericUpDown { Left = 150, Top = y - 3, Width = 130, Minimum = 1, Maximum = 20 };
            leftPanel.Controls.Add(minLabel);
            leftPanel.Controls.Add(heroCountMinUpDown);
            y += 30;

            var maxLabel = new Label { Text = "HeroCountMax:", Left = 20, Top = y, AutoSize = true };
            heroCountMaxUpDown = new NumericUpDown { Left = 150, Top = y - 3, Width = 130, Minimum = 1, Maximum = 20 };
            leftPanel.Controls.Add(maxLabel);
            leftPanel.Controls.Add(heroCountMaxUpDown);
            y += 30;

            var incLabel = new Label { Text = "CountIncrement:", Left = 20, Top = y, AutoSize = true };
            heroCountIncrementUpDown = new NumericUpDown { Left = 150, Top = y - 3, Width = 130, Minimum = 1, Maximum = 10 };
            leftPanel.Controls.Add(incLabel);
            leftPanel.Controls.Add(heroCountIncrementUpDown);
            y += 40;

            applyBtn = new Button { Text = "Apply", Left = 150, Top = y, Width = 130 };
            applyBtn.Click += (s, e) =>
            {
                var sizeValue = Constant.MapSizeOptionToValue(sizeComboBox.Text);
                var dto = new GlobalDto
                {
                    name = nameTextBox.Text,
                    gameMode = gameModeComboBox.Text,
                    winCondition = winConditionComboBox.SelectedIndex >= 0 ? winConditionComboBox.Text : null,
                    sizeX = sizeValue,
                    sizeZ = sizeValue,
                    heroCountMin = (int)heroCountMinUpDown.Value,
                    heroCountMax = (int)heroCountMaxUpDown.Value,
                    heroCountIncrement = (int)heroCountIncrementUpDown.Value,
                };
                rmg.ApplyGlobalDto(dto);
                RefreshData();
            };
            leftPanel.Controls.Add(applyBtn);

            split.Panel1.Controls.Add(leftPanel);

            // === Right: PropertyGrid ===
            propertyGrid = new PropertyGrid();
            propertyGrid.Dock = DockStyle.Fill;
            propertyGrid.PropertySort = PropertySort.Categorized;
            split.Panel2.Controls.Add(propertyGrid);

            Controls.Add(split);
        }
    }

    public class GlobalDto
    {
        public string name { get; set; } = "";
        public string? gameMode { get; set; }
        public string? winCondition { get; set; }
        public int sizeX { get; set; }
        public int sizeZ { get; set; }
        public int heroCountMin { get; set; }
        public int heroCountMax { get; set; }
        public int heroCountIncrement { get; set; }
    }
}
