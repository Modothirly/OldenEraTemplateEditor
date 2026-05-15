using OldenEraTemplateEditor.Models;
using OldenEraTemplateEditor.Views;

namespace OldenEraTemplateEditor
{
    public class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }

    public class MainForm : Form
    {
        private ToolStrip toolStrip;
        private TabControl tabControl;
        private Rmg rmg = new();

        public MainForm()
        {
            this.Text = "Olden Era Template Editor";
            this.Size = new Size(1280, 800);
            InitToolBar();
        }
        private void InitToolBar()
        {
            // 工具栏容器
            toolStrip = new ToolStrip();

            // =====================
            // Template group
            // =====================
            ToolStripFile TemplateToolStrip = new ToolStripFile("Template", rmg, () => this.RefreshData());
            TemplateToolStrip.CreateButtonGroup(toolStrip);

            toolStrip.Dock = DockStyle.Top;


            tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;

            tabControl.TabPages.Add(new GlobalTabPage(rmg));
            tabControl.TabPages.Add(new MandatoryContentTabPage(rmg));
            tabControl.TabPages.Add(new ContentCountLimitsTabPage(rmg));
            tabControl.TabPages.Add(new ZoneLayoutsTabPage(rmg));
            tabControl.TabPages.Add(new ZonesTabPage(rmg));

            Controls.Add(tabControl);
            Controls.Add(toolStrip);
        }

        public void RefreshData()
        {
            foreach (TabPage page in tabControl.TabPages)
            {
                if (page is EditorTabPage editorTabPage)
                {
                    editorTabPage.RefreshData();
                }
            }
        }

    }
}
