using ImGuiNET;
using ImGuiScene;
using System.Numerics;
using System.Diagnostics;
using Newtonsoft.Json;
using UIDev.lib;

namespace UIDev
{
  class UITest : IPluginUIMock
  {
    public static void Main(string[] args)
    {
      UIBootstrap.Inititalize(new UITest());
    }

    private TextureWrap? goatImage;
    private SimpleImGuiScene? scene;

    //  MainPlugin definitions
    private Orchestrator orchestrator;

    public class Configuration
    {
      public bool LockPosition = true;
      public bool EnablePlanner = false;
      public string JobAbbr = "MCH";
      public string JobName = "Machinist";

    }
    private Configuration configuration;

    public void Initialize(SimpleImGuiScene scene)
    {
      // scene is a little different from what you have access to in dalamud
      // but it can accomplish the same things, and is really only used for initial setup here

      // eg, to load an image resource for use with ImGui 
      this.goatImage = scene.LoadImage("goat.png");
      this.configuration = new Configuration();
      this.orchestrator = new Orchestrator();

      scene.OnBuildUI += Draw;

      this.Visible = true;

      // saving this only so we can kill the test application by closing the window
      // (instead of just by hitting escape)
      this.scene = scene;
    }

    public void Dispose()
    {
      this.goatImage?.Dispose();
    }

    // You COULD go all out here and make your UI generic and work on interfaces etc, and then
    // mock dependencies and conceivably use exactly the same class in this testbed and the actual plugin
    // That is, however, a bit excessive in general - it could easily be done for this sample, but I
    // don't want to imply that is easy or the best way to go usually, so it's not done here either
    private void Draw()
    {
      DrawMainWindow();
      DrawSettingsWindow();

      if (!Visible)
      {
        this.scene!.ShouldQuit = true;
      }
    }

    #region Nearly a copy/paste of PluginUI
    private bool visible = false;
    public bool Visible
    {
      get { return this.visible; }
      set { this.visible = value; }
    }

    private bool settingsVisible = false;
    public bool SettingsVisible
    {
      get { return this.settingsVisible; }
      set { this.settingsVisible = value; }
    }

    public void DrawMainWindow()
    {
      if (!Visible)
      {
        return;
      }

      Trace.WriteLine("Configuration:" + JsonConvert.SerializeObject(this.configuration));

      var lockedSettings = ImGuiWindowFlags.NoScrollbar
          | ImGuiWindowFlags.NoScrollWithMouse
          | ImGuiWindowFlags.NoBackground
          | ImGuiWindowFlags.NoMove
          | ImGuiWindowFlags.NoTitleBar;

      var unlockedSettings = ImGuiWindowFlags.NoScrollbar
          | ImGuiWindowFlags.NoScrollWithMouse
          | ImGuiWindowFlags.NoBackground;

      ImGui.SetNextWindowSize(new Vector2(375, 330), ImGuiCond.FirstUseEver);
      ImGui.SetNextWindowSizeConstraints(new Vector2(375, 330), new Vector2(float.MaxValue, float.MaxValue));
      if (ImGui.Begin("EpiPlan", ref this.visible, this.configuration.LockPosition ? lockedSettings : unlockedSettings)) {
        
        //  Push the content down the height of the title bar
        //    To maintain user positioning assumption
        if (this.configuration.LockPosition)
        {
          var headerSize = new Vector2(0, 19);
          ImGui.Dummy(headerSize);
        }
        ImGui.Text($"Epi Plan is {(this.configuration.EnablePlanner ? "Enabled" : "Disabled")}");

        if (ImGui.Button("Settings"))
        {
          SettingsVisible = true;
        }

        ImGui.Spacing();

        //  Content
        // ImGui.Indent(55);
        // ImGui.Image(this.goatImage!.ImGuiHandle, new Vector2(this.goatImage.Width, this.goatImage.Height));

        ImGui.Text($"Job: {this.configuration.JobAbbr}");

        this.orchestrator.run();

        //  ImGui.Unindent(55);
      }
      ImGui.End();
    }

    public void DrawSettingsWindow()
    {
      if (!SettingsVisible)
      {
        return;
      }

      ImGui.SetNextWindowSize(new Vector2(232, 120), ImGuiCond.Always);
      if (ImGui.Begin("EpiPlan Settings", ref this.settingsVisible,
          ImGuiWindowFlags.AlwaysAutoResize
          | ImGuiWindowFlags.NoResize
          | ImGuiWindowFlags.NoCollapse
          | ImGuiWindowFlags.NoScrollbar
          | ImGuiWindowFlags.NoScrollWithMouse))
      {
        var enablePlannerValue = this.configuration.EnablePlanner;
        if (ImGui.Checkbox("Enable Planner", ref enablePlannerValue))
        {
          this.configuration.EnablePlanner = enablePlannerValue;
        }

        var lockPositionValue = this.configuration.LockPosition;
        if (ImGui.Checkbox("Lock Position", ref lockPositionValue))
        {
          this.configuration.LockPosition = lockPositionValue;
        }
      }
      ImGui.End();
    }
    #endregion
  }
}
