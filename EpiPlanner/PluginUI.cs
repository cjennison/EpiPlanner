using ImGuiNET;
using Dalamud.Logging;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState;
using System;
using System.Numerics;

namespace EpiPlan
{
  // It is good to have this be disposable in general, in case you ever need it
  // to do any cleanup
  class PluginUI : IDisposable
  {
    private Configuration configuration;
    private ClientState ClientState;

    private ImGuiScene.TextureWrap goatImage;

    // this extra bool exists for ImGui, since you can't ref a property
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

    // passing in the image here just for simplicity
    public PluginUI(Configuration configuration, ImGuiScene.TextureWrap goatImage, ClientState clientState)
    {
      this.configuration = configuration;
      this.goatImage = goatImage;
      this.ClientState = clientState;
    }

    public void Dispose()
    {
      this.goatImage.Dispose();
    }

    public void Draw()
    {
      // This is our only draw handler attached to UIBuilder, so it needs to be
      // able to draw any windows we might have open.
      // Each method checks its own visibility/state to ensure it only draws when
      // it actually makes sense.
      // There are other ways to do this, but it is generally best to keep the number of
      // draw delegates as low as possible.

      DrawMainWindow();
      DrawSettingsWindow();

      PluginLog.Debug("Hello World");
    }

    public void DrawMainWindow()
    {
      if (!Visible)
      {
        return;
      }

      ImGui.SetNextWindowSize(new Vector2(375, 330), ImGuiCond.FirstUseEver);
      ImGui.SetNextWindowSizeConstraints(new Vector2(375, 330), new Vector2(float.MaxValue, float.MaxValue));
      if (ImGui.Begin("EpiPlan", ref this.visible, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse))
      {
        ImGui.Text($"Epi Plan is {(this.configuration.EnablePlanner ? "Enabled" : "Disabled")}");

        var actionId   = this.ClientState.LocalPlayer.CastActionId;
        var actionType = this.ClientState.LocalPlayer.CastActionType;
        var playerName = this.ClientState.LocalPlayer.Name;

        if (ImGui.Button("Settings"))
        {
          SettingsVisible = true;
        }

        ImGui.Spacing();

        ImGui.Text($"{actionId} {actionType} {playerName}");
      }
      ImGui.End();
    }

    public void DrawSettingsWindow()
    {
      if (!SettingsVisible)
      {
        return;
      }

      ImGui.SetNextWindowSize(new Vector2(232, 75), ImGuiCond.Always);
      if (ImGui.Begin("EpiPlan Settings", ref this.settingsVisible,
          ImGuiWindowFlags.NoResize
              | ImGuiWindowFlags.NoCollapse
              | ImGuiWindowFlags.NoScrollbar
              | ImGuiWindowFlags.NoScrollWithMouse
      )) {
        // can't ref a property, so use a local copy
        var configValue = this.configuration.EnablePlanner;
        if (ImGui.Checkbox("Enable Planner", ref configValue)) {
          this.configuration.EnablePlanner = configValue;
          // can save immediately on change, if you don't want to provide a "Save and Close" button
          this.configuration.Save();
        }
      }
      ImGui.End();
    }
  }
}
