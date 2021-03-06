using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using System.Reflection;
using Dalamud.Game.ClientState;
using System.Diagnostics;

namespace EpiPlan
{
  public sealed class Plugin : IDalamudPlugin
  {
    public string Name => "EpiPlan";

    private const string commandName = "/epiplan";

    private DalamudPluginInterface PluginInterface { get; init; }
    private CommandManager CommandManager { get; init; }
    private Configuration Configuration { get; init; }
    private PluginUI PluginUi { get; init; }
    private ClientState ClientState { get; init; }

    public Plugin(
        [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
        [RequiredVersion("1.0")] CommandManager commandManager,
        ClientState clientState
      )
    {
      this.PluginInterface = pluginInterface;
      this.CommandManager = commandManager;
      this.ClientState = clientState;

      this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
      this.Configuration.Initialize(this.PluginInterface);

      // you might normally want to embed resources and load them from the manifest stream
      var assemblyLocation = Assembly.GetExecutingAssembly().Location;
      var imagePath = Path.Combine(Path.GetDirectoryName(assemblyLocation)!, "goat.png");
      var goatImage = this.PluginInterface.UiBuilder.LoadImage(imagePath);
      this.PluginUi = new PluginUI(this.Configuration, goatImage, clientState);

      this.CommandManager.AddHandler(commandName, new CommandInfo(OnCommand)
      {
        HelpMessage = "Opens the EpiPlan interface"
      });

      this.PluginInterface.UiBuilder.Draw += DrawUI;
      this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
    }

    public void Dispose()
    {
      this.PluginUi.Dispose();
      this.CommandManager.RemoveHandler(commandName);
    }

    private void OnCommand(string command, string args)
    {
      // in response to the slash command, just display our main ui
      this.PluginUi.Visible = true;
    }

    private void DrawUI()
    {
      this.PluginUi.Draw();
    }

    private void DrawConfigUI()
    {
      this.PluginUi.SettingsVisible = true;
    }
  }
}
